using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;

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
            bool isGroundTile = false;
            List<Point> possibleDirections = new List<Point> { 
                new Point(1, 0),
                new Point(-1, 0),
                new Point(0, 1),
                new Point(0, -1)
            }; 

            //Loop until a valid ground tile is found or no possible directions remain
            do
            {
                //Check if there are possible directions
                if (possibleDirections.Count == 0)
                {
                    direction = Point.Zero;
                    break;
                }

                //Check a possible direction
                direction = possibleDirections[GameEnvironment.Random.Next(possibleDirections.Count)];
                possibleDirections.Remove(direction);

                bool inBounds = LevelLoader.grid.GetLength(0) > gameObject.gridPosition.X + direction.X &&
                    LevelLoader.grid.GetLength(1) > gameObject.gridPosition.Y + direction.Y &&
                    gameObject.gridPosition.X + direction.X >= 0 &&
                    gameObject.gridPosition.Y + direction.Y >= 0;

                if (inBounds)
                {
                    isGroundTile = LevelLoader.grid[gameObject.gridPosition.X + direction.X, gameObject.gridPosition.Y + direction.Y].tileType == Tile.TileType.GROUND;
                }
            }
            while (!isGroundTile);

            return direction;
        }
    }

    class ChaseState : State
    {
        public int stamina;
        public Player player;

        protected GameObject gameObject;

        private const int MAX_STAMINA = 15;
        private readonly StateMachine stateMachine;
        private Point[] path;

        public ChaseState(GameObject gameObject, StateMachine stateMachine, string stateName = "Chase") : base(stateName)
        {
            this.stateMachine = stateMachine;
            this.gameObject = gameObject;

            //Find closest player as a target
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

            //Choose a random player
            if (float.IsInfinity(closestPlayer))
            {
                player = players[GameEnvironment.Random.Next(players.Count)] as Player;
            }
        }

        public override void Start()
        {
            stamina = MAX_STAMINA;
        }

        public override void FixedUpdate(GameTime gameTime)
        {
            path = GetNewPath();

            stamina--;

            if (path != null)
            {
                //Move the enemy
                int currentStep = path.Length - 2; //-2 because conversion to index and the end is current position
                if (currentStep >= 0)
                {
                    gameObject.gridPosition = path[currentStep];
                }

                if (player.CheckCollision(gameObject))
                {
                    stamina = 0;
                    player.TakeDamage(gameObject.gridPosition, gameTime);
                    stateMachine.SetState("Attacked");
                }
            }
        }

        /// <summary>
        /// Get a path to the player
        /// </summary>
        /// <returns>A list of points to form a path</returns>
        protected virtual Point[] GetNewPath() => AStar.FindPath(gameObject.gridPosition, player.GetCenter());
    }

    class ReturnState : State
    {
        public int stamina;

        private readonly GameObject gameObject;
        private readonly Point startPosition;
        private Point[] path;

        public ReturnState(GameObject gameObject, string stateName = "Return") : base(stateName)
        {
            startPosition = gameObject.gridPosition;
            this.gameObject = gameObject;
        }

        public override void Start()
        {
            Point gridPosition = gameObject.gridPosition;
            path = AStar.FindPath(gridPosition, startPosition);

            if (path != null)
            {
                stamina = path.Length - 1;
            }
        }

        public override void FixedUpdate(GameTime gameTime)
        {
            stamina--;
            gameObject.gridPosition = path[stamina];
        }
    }

    class AttackedState : ReturnState
    {
        public AttackedState(GameObject gameObject) : base(gameObject, "Attacked") { }
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
            Debug.WriteLine(":(");
        }
    }

    class ScaredState : State
    {
        protected GameObject gameObject;
        private Vector2[] direction = new Vector2[4] {
        new Vector2(0,1),
        new Vector2(0, -1),
        new Vector2(1, 0),
        new Vector2(-1, 0) };
        private float oldDistance;
        private Vector2 tileChecker;
        Player player;

        public ScaredState(GameObject gameObject) : base("Scared") 
        {
            this.gameObject = gameObject;
        }

        public override void FixedUpdate(GameTime gameTime)
        {
            for (int partyOnTime = 8; partyOnTime > -1; partyOnTime--)
            {
                Debug.WriteLine(PartyTime.partyOn);
                List<GameObject> players = GameEnvironment.GetState<PlayingState>("PlayingState").players.Children;
                foreach (Player player in players)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        tileChecker = gameObject.gridPosition.ToVector2() + direction[i];
                        if (LevelLoader.grid[(int)tileChecker.X, (int)tileChecker.Y].tileType != Tile.TileType.GROUND)
                        {
                            continue;
                        }

                        if (Vector2.Distance(gameObject.gridPosition.ToVector2() + direction[i], player.gridPosition.ToVector2()) > oldDistance)
                        {
                            gameObject.gridPosition += direction[i].ToPoint();
                            oldDistance = Vector2.Distance(gameObject.gridPosition.ToVector2() + direction[i], player.gridPosition.ToVector2());
                            this.player = player;
                        }

                        if (i == 4)
                        {
                            i = 0;
                        }
                    }
                }
                if (partyOnTime <= 0)
                {
                    PartyTime.partyOn = false;
                }
            }
        }

    }

    class EnemyWalking : GameObject
    {
        private const int TrackingDistance = 10;
        protected StateMachine stateMachine;

        public EnemyWalking(Point gridPosition, string spritePath) : base(gridPosition, spritePath)
        {

        }

        public EnemyWalking(Point gridPosition = new Point()) : base(gridPosition, "GameObjects/Enemies/Koning")
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
            stateMachine.AddState(new ScaredState(this));

            //Add connections between states
            stateMachine.AddConnection("Patrol", "Return", (object state) => (state as PatrolState).stamina <= 0, stateMachine.GetState("Patrol"));
            stateMachine.AddConnection("Chase", "Attacked", (object state) => (state as ChaseState).stamina <= 0, stateMachine.GetState("Chase"));
            stateMachine.AddConnection("Return", "Patrol", (object state) => (state as ReturnState).stamina <= 0, stateMachine.GetState("Return"));
            stateMachine.AddConnection("Attacked", "Patrol", (object state) => (state as ReturnState).stamina <= 0, stateMachine.GetState("Attacked"));

            stateMachine.AddConnectionToAll("Crying", () => !CanMove());

            //Set state to Patrol
            stateMachine.SetState("Patrol");
        }

        public override void FixedUpdate(GameTime gameTime)
        {
            base.FixedUpdate(gameTime);

            stateMachine.FixedUpdate(gameTime);

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
            if (float.IsFinite(closestPlayer) && stateMachine.CurrentState != stateMachine.GetState("Attacked") && stateMachine.CurrentState != stateMachine.GetState("Chase"))
            {
                stateMachine.SetState("Chase");
            }

            //Update State to Scared
            if (PartyTime.partyOn)
            {
                stateMachine.SetState("Scared");
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            positionSize = new Rectangle(
            gridPosition.X * LevelLoader.gridTileSize + GameEnvironment.startGridPoint.X + (int)(LevelLoader.gridTileSize / 2),
                gridPosition.Y * LevelLoader.gridTileSize + GameEnvironment.startGridPoint.Y + (int)(LevelLoader.gridTileSize),
                (int)(texture.Width * LevelLoader.scalingFactor),
                (int)(texture.Height * LevelLoader.scalingFactor));
            spriteBatch.Draw(texture, positionSize, null, Color.White, 0, new Vector2(texture.Width / 2, texture.Height), SpriteEffects.None, 1);
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
