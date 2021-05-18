using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Poloknightse
{
    class PatrolState : State
    {
        public int stamina;
        private const int MAX_STAMINA = 5;
        protected GameObject gameObject;

        public PatrolState(GameObject gameObject, string stateName = "Patrol") : base(stateName)
        {
            this.gameObject = gameObject;
        }

        public override void Start()
        {
            stamina = MAX_STAMINA;
        }

        public override void FixedUpdate(GameTime gameTime)
        {
            stamina--;

            Point direction = GetRandomDirection();

            gameObject.gridPosition += direction;
        }

        /// <summary>
        /// Gets a random orthogonal direction
        /// </summary>
        public virtual Point GetRandomDirection()
        {
            Point direction;
            bool foundDirection = false;
            do
            {
                if (GameEnvironment.Random.Next(2) == 0)
                {
                    direction = new Point(GameEnvironment.Random.Next(2) * 2 - 1, 0);
                }
                else
                {
                    direction = new Point(0, GameEnvironment.Random.Next(2) * 2 - 1);
                }

                bool inbounds = LevelLoader.grid.GetLength(0) > gameObject.gridPosition.X + direction.X &&
                    LevelLoader.grid.GetLength(1) > gameObject.gridPosition.Y + direction.Y &&
                    gameObject.gridPosition.X + direction.X >= 0 &&
                    gameObject.gridPosition.Y + direction.Y >= 0;
                if (inbounds)
                {
                    foundDirection = LevelLoader.grid[gameObject.gridPosition.X + direction.X, gameObject.gridPosition.Y + direction.Y].tileType == Tile.TileType.GROUND;
                }
            }
            while (!foundDirection);
            return direction;
        }
    }

    class ChaseState : State
    {
        public int stamina;
        private const int MAX_STAMINA = 100;
        protected GameObject gameObject;
        public Player player;
        Point[] path;
        StateMachine stateMachine;

        public ChaseState(GameObject gameObject, StateMachine stateMachine, string stateName = "Chase") : base(stateName)
        {
            this.stateMachine = stateMachine;
            this.gameObject = gameObject;
            float closestPlayer = float.PositiveInfinity;
            List<GameObject> players = GameEnvironment.GetState<PlayingState>("PlayingState").players.Children;
            foreach (Player player in players)
            {
                float distance = Vector2.Distance(player.gridPosition.ToVector2(), gameObject.gridPosition.ToVector2());
                if (distance <= 10 && distance < closestPlayer)
                {
                    closestPlayer = distance;
                    this.player = player;
                }
            }
            if (float.IsInfinity(closestPlayer))
            {
                player = players[GameEnvironment.Random.Next(players.Count)] as Player;
            }
        }

        public override void Start()
        {
            path = GetNewPath();
            if (path == null) return;

            if (MAX_STAMINA >= path.Length) stamina = path.Length - 1;
            else stamina = MAX_STAMINA;
        }

        public override void FixedUpdate(GameTime gameTime)
        {
            path = GetNewPath();

            stamina--;

            if (path != null)
            {
                int currentStep = path.Length - 2;
                if (currentStep >= 0)
                {
                    gameObject.gridPosition = path[currentStep];
                }

                if (player.CheckCollision(gameObject))
                {
                    stamina = 0;
                    player.TakeDamage(gameObject.gridPosition, gameTime);
                }
                for (int i = player.followers.Count - 1; i >= 0; i--)
                {
                    PlayerFollower follower = player.followers[i];

                    if (gameObject.gridPosition == follower.gridPosition)
                    {
                        stamina = 0;
                        player.TakeDamage(player.GetCenter(), gameTime);
                        stateMachine.SetState("Attacked");
                        break;
                    }
                }
            }
        }

        public virtual Point[] GetNewPath()
        {
            return AStar.FindPath(gameObject.gridPosition, player.GetCenter());
        }
    }

    class ReturnState : State
    {
        public int stamina;
        Point startPosition;
        GameObject gameObject;
        Point[] path;

        public ReturnState(GameObject gameObject, string stateName = "Return") : base(stateName)
        {
            startPosition = gameObject.gridPosition;
            this.gameObject = gameObject;
        }

        public override void Start()
        {
            Point gridPosition = gameObject.gridPosition;
            path = AStar.FindPath(gridPosition, startPosition);
            stamina = path.Length - 1;
        }

        public override void FixedUpdate(GameTime gameTime)
        {
            stamina--;
            gameObject.gridPosition = path[stamina];
        }
    }

    class AttackedState : ReturnState
    {
        public AttackedState(GameObject gameObject) : base(gameObject, "Attacked")
        {

        }
    }

    class CryingState : State
    {
        GameObject gameObject;

        public CryingState(GameObject gameObject) : base("Crying")
        {
            this.gameObject = gameObject;
        }

        public override void Start()
        {
            System.Diagnostics.Debug.WriteLine(":(");
        }
    }

    class EnemyWalking : GameObject
    {
        private const int TrackingDistance = 10;
        protected StateMachine stateMachine;

        public EnemyWalking(Point gridPosition, string spritePath) : base(gridPosition, spritePath)
        {

        }

        public EnemyWalking(Point gridPosition = new Point()) : base(gridPosition, "GameObjects/Player/Koning")
        {

        }

        public override void Initialize()
        {
            stateMachine = new StateMachine();

            //Add states to stateMachine
            stateMachine.AddState(new PatrolState(this));
            stateMachine.AddState(new ChaseState(this, stateMachine));
            stateMachine.AddState(new ReturnState(this));
            stateMachine.AddState(new AttackedState(this));
            stateMachine.AddState(new CryingState(this));

            //Add connections between states
            stateMachine.AddConnection("Patrol", "Return", (object state) => (state as PatrolState).stamina <= 0, stateMachine.GetState("Patrol"));
            stateMachine.AddConnection("Chase", "Return", (object state) => (state as ChaseState).stamina <= 0, stateMachine.GetState("Chase"));
            stateMachine.AddConnection("Return", "Patrol", (object state) => (state as ReturnState).stamina <= 0, stateMachine.GetState("Return"));
            stateMachine.AddConnection("Attacked", "Patrol", (object state) => (state as ReturnState).stamina <= 0, stateMachine.GetState("Attacked"));

            stateMachine.AddConnectionToAll("Crying", () => !CanMove());

            //Set state to Patrol
            stateMachine.SetState("Chase");
        }

        public override void FixedUpdate(GameTime gameTime)
        {
            base.FixedUpdate(gameTime);

            stateMachine.FixedUpdate(gameTime);

            if (stateMachine.CurrentState.name == "Attacked") return;

            //Find the closest player
            float closestPlayer = float.PositiveInfinity;
            if (GameEnvironment.GetState<PlayingState>("PlayingState").players.Children.Count > 0)
            {
                foreach (Player player in GameEnvironment.GetState<PlayingState>("PlayingState").players.Children)
                {
                    float distance = Vector2.Distance(player.gridPosition.ToVector2(), gridPosition.ToVector2());
                    if (distance <= TrackingDistance && distance < closestPlayer)
                    {
                        closestPlayer = distance;
                        (stateMachine.GetState("Chase") as ChaseState).player = player;
                    }
                }
            }

            //If a player was found in range then attack it
            if (float.IsFinite(closestPlayer))
            {
                stateMachine.SetState("Chase");
            }
        }

        /// <summary>
        /// Checks if enemy can walk orthogonaly to any place
        /// </summary>
        protected bool CanMove()
        {
            bool canMove = false;
            if (gridPosition.X + 1 < LevelLoader.grid.GetLength(0))
                canMove = LevelLoader.grid[gridPosition.X + 1, gridPosition.Y].tileType == Tile.TileType.GROUND;

            if (gridPosition.X - 1 >= 0)
                canMove = LevelLoader.grid[gridPosition.X - 1, gridPosition.Y].tileType == Tile.TileType.GROUND || canMove;

            if (gridPosition.Y + 1 < LevelLoader.grid.GetLength(1))
                canMove = LevelLoader.grid[gridPosition.X, gridPosition.Y + 1].tileType == Tile.TileType.GROUND || canMove;

            if (gridPosition.Y - 1 >= 0)
                canMove = LevelLoader.grid[gridPosition.X, gridPosition.Y - 1].tileType == Tile.TileType.GROUND || canMove;

            return canMove;
        }
    }
}
