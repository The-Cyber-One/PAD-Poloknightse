using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Poloknightse
{
    class PatrolState : State
    {
        public int stamina;
        private const int MAX_STAMINA = 5;
        GameObject gameObject;

        public PatrolState(GameObject gameObject) : base("Patrol")
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
            while (LevelLoader.grid[gameObject.gridPosition.X + direction.X, gameObject.gridPosition.Y + direction.Y].tileType == Tile.TileType.WALL)
            {
                direction = GetRandomDirection();
            }
            gameObject.gridPosition += direction;
        }

        /// <summary>
        /// Gets a random orthogonal direction
        /// </summary>
        private Point GetRandomDirection()
        {
            if (GameEnvironment.Random.Next(2) == 0)
            {
                return new Point(GameEnvironment.Random.Next(2) * 2 - 1, 0);
            }
            else
            {
                return new Point(0, GameEnvironment.Random.Next(2) * 2 - 1);
            }
        }
    }

    class ChaseState : State
    {
        public int stamina;
        private const int MAX_STAMINA = 100;
        GameObject gameObject;
        Player player;
        Point[] path;

        public ChaseState(GameObject gameObject, Player player) : base("Chase")
        {
            this.gameObject = gameObject;
            this.player = player;
        }

        public override void Start()
        {
            path = AStar.FindPath(gameObject.gridPosition, player.GetCenter());

            if (MAX_STAMINA >= path.Length) stamina = path.Length - 1;
            else stamina = MAX_STAMINA;
        }

        public override void FixedUpdate(GameTime gameTime)
        {
            int old = path.Length;
            path = AStar.FindPath(gameObject.gridPosition, player.GetCenter());
            System.Diagnostics.Debug.WriteLine("1: " + stamina);
            stamina += path.Length - old;
            System.Diagnostics.Debug.WriteLine("2: " + stamina);
            stamina--;

            gameObject.gridPosition = path[stamina];
            if (player.CheckCollision(gameObject))
            {
                player.TakeDamage(gameObject.gridPosition);
            }
        }
    }

    class ReturnState : State
    {
        public int stamina;
        Point startPosition;
        GameObject gameObject;
        Point[] path;

        public ReturnState(GameObject gameObject) : base("Return")
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
        StateMachine stateMachine;
        bool isLoaded = false;

        public EnemyWalking(Point gridPosition) : base(gridPosition, "GameObjects/Player/Koning")
        {

        }

        public override void Initialize()
        {
            stateMachine = new StateMachine();

            //Add states to stateMachine
            stateMachine.AddState(new PatrolState(this));
            stateMachine.AddState(new ChaseState(this, (GameEnvironment.CurrentGameState as PlayingState).player));
            stateMachine.AddState(new ReturnState(this));
            stateMachine.AddState(new CryingState(this));
            //stateMachine.AddState(new SearchState());

            //Add connections between states
            stateMachine.AddConnection("Patrol", "Return", (object state) => (state as PatrolState).stamina <= 0, stateMachine.GetState("Patrol"));
            stateMachine.AddConnection("Chase", "Return", (object state) => (state as ChaseState).stamina <= 0, stateMachine.GetState("Chase"));
            stateMachine.AddConnection("Return", "Patrol", (object state) => (state as ReturnState).stamina <= 0, stateMachine.GetState("Return"));
            stateMachine.AddConnectionToAll("Crying", () => !CanMove());

            //Set state to Patrol
            stateMachine.SetState("Chase");
        }

        public override void FixedUpdate(GameTime gameTime)
        {
            base.FixedUpdate(gameTime);

            stateMachine.FixedUpdate(gameTime);
        }

        /// <summary>
        /// Checks if enemy can walk orthogonaly to any place
        /// </summary>
        private bool CanMove()
        {
            return LevelLoader.grid[gridPosition.X + 1, gridPosition.Y].tileType == Tile.TileType.GROUND ||
                LevelLoader.grid[gridPosition.X - 1, gridPosition.Y].tileType == Tile.TileType.GROUND ||
                LevelLoader.grid[gridPosition.X, gridPosition.Y + 1].tileType == Tile.TileType.GROUND ||
                LevelLoader.grid[gridPosition.X, gridPosition.Y - 1].tileType == Tile.TileType.GROUND;
        }
    }
}
