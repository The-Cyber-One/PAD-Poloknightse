﻿using Microsoft.Xna.Framework;
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
        private const int MAX_STAMINA = 10;
        protected GameObject gameObject;
        public Player player;
        Point[] path;

        public ChaseState(GameObject gameObject, string stateName = "Chase") : base(stateName)
        {
            this.gameObject = gameObject;
            float closestPlayer = float.PositiveInfinity;
            foreach (Player player in (GameEnvironment.CurrentGameState as PlayingState).players)
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
                player = (GameEnvironment.CurrentGameState as PlayingState).players[GameEnvironment.Random.Next((GameEnvironment.CurrentGameState as PlayingState).players.Count)];
            }
        }

        public override void Start()
        {
            path = GetNewPath();

            if (MAX_STAMINA >= path.Length) stamina = path.Length - 1;
            else stamina = MAX_STAMINA;
        }

        public override void FixedUpdate(GameTime gameTime)
        {
            path = GetNewPath();

            stamina--;
            int currentStep = path.Length - 2;

            if (currentStep >= 0)
                gameObject.gridPosition = path[currentStep];

            if (gameObject.gridPosition == player.GetCenter())
            {
                stamina = 0;
                player.TakeDamage(gameObject.gridPosition, gameTime);
                Debug.WriteLine("sda");
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

        public EnemyWalking(Point gridPosition) : base(gridPosition, "GameObjects/Player/Koning")
        {

        }

        public override void Initialize()
        {
            stateMachine = new StateMachine();

            //Add states to stateMachine
            stateMachine.AddState(new PatrolState(this));
            stateMachine.AddState(new ChaseState(this));
            stateMachine.AddState(new ReturnState(this));
            stateMachine.AddState(new CryingState(this));

            //Add connections between states
            stateMachine.AddConnection("Patrol", "Return", (object state) => (state as PatrolState).stamina <= 0, stateMachine.GetState("Patrol"));
            stateMachine.AddConnection("Chase", "Return", (object state) => (state as ChaseState).stamina <= 0, stateMachine.GetState("Chase"));
            stateMachine.AddConnection("Return", "Patrol", (object state) => (state as ReturnState).stamina <= 0, stateMachine.GetState("Return"));
            
            //stateMachine.AddConnection("Patrol", "Chase", () =>
            //{
            //    foreach (Player player in (GameEnvironment.CurrentGameState as PlayingState).players)
            //    {
            //        float distance = Vector2.Distance(player.gridPosition.ToVector2(), gridPosition.ToVector2());
            //        if (distance <= 10)
            //        {
            //            return true;
            //        }
            //    }
            //    return false;
            //});

            //stateMachine.AddConnection("Patrol", "Chase", (object state) =>
            //{
            //    float closestPlayer = float.PositiveInfinity;
            //    if ((GameEnvironment.CurrentGameState as PlayingState).players.Count > 0)
            //    {
            //        foreach (Player player in (GameEnvironment.CurrentGameState as PlayingState).players)
            //        {
            //            float distance = Vector2.Distance(player.gridPosition.ToVector2(), gridPosition.ToVector2());
            //            if (distance <= 10 && distance < closestPlayer)
            //            {
            //                closestPlayer = distance;
            //                (state as ChaseState).player = player;
            //            }
            //        }
            //    }
            //    return float.IsFinite(closestPlayer);
            //}, stateMachine.GetState("Chase"));
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
            if ((GameEnvironment.CurrentGameState as PlayingState).players.Count > 0)
            {
                foreach (Player player in (GameEnvironment.CurrentGameState as PlayingState).players)
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
            if(float.IsFinite(closestPlayer))
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
