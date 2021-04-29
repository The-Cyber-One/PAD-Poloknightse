﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Poloknightse
{
    class GhostPatrolState : PatrolState
    {
        int ghostCooldownSteps = 5;
        int stepsCounter;

        public GhostPatrolState(GameObject gameObject) : base(gameObject, "GhostPatrol")
        {

        }

        public override void Start()
        {
            base.Start();
            stepsCounter = ghostCooldownSteps;
        }

        public override Point GetRandomDirection()
        {
            //Return basic direction
            if (stepsCounter > 0)
                return base.GetRandomDirection();

            //Return ghost direction
            if (GameEnvironment.Random.Next(2) == 0)
                return new Point(GameEnvironment.Random.Next(2) * 2 - 1, 0);

            return new Point(0, GameEnvironment.Random.Next(2) * 2 - 1);
        }

        public override void FixedUpdate(GameTime gameTime)
        {
            stepsCounter--;
            base.FixedUpdate(gameTime);
            if (LevelLoader.grid[gameObject.gridPosition.X, gameObject.gridPosition.Y].tileType == Tile.TileType.WALL)
            {
                stepsCounter = ghostCooldownSteps;
            }
        }
    }

    class GhostChaseState : ChaseState
    {
        int ghostCooldownSteps = 10;
        int stepsCounter;

        public GhostChaseState(GameObject gameObject) : base(gameObject, "GhostChase")
        {

        }

        public override void Start()
        {
            base.Start();
            stepsCounter = ghostCooldownSteps;
        }

        public override Point[] GetNewPath()
        {
            if (stepsCounter > 0)
                return base.GetNewPath();

            return AStar.FindPath(gameObject.gridPosition, player.GetCenter(), GetGhostWalls());
        }

        public override void FixedUpdate(GameTime gameTime)
        {
            stepsCounter--;
            base.FixedUpdate(gameTime);
            if (LevelLoader.grid[gameObject.gridPosition.X, gameObject.gridPosition.Y].tileType == Tile.TileType.WALL)
            {
                stepsCounter = ghostCooldownSteps;
            }
        }

        private Tile.TileType[,] GetGhostWalls()
        {
            Tile.TileType[,] grid = new Tile.TileType[LevelLoader.grid.GetLength(0), LevelLoader.grid.GetLength(1)];
            for (int x = 0; x < LevelLoader.grid.GetLength(0); x++)
            {
                for (int y = 0; y < LevelLoader.grid.GetLength(1); y++)
                {
                    grid[x, y] = LevelLoader.grid[x, y].tileType;
                }
            }
            Point center = gameObject.gridPosition;

            grid[center.X + 1, center.Y] = Tile.TileType.GROUND;
            grid[center.X - 1, center.Y] = Tile.TileType.GROUND;
            grid[center.X, center.Y + 1] = Tile.TileType.GROUND;
            grid[center.X, center.Y - 1] = Tile.TileType.GROUND;

            return grid;
        }
    }

    class GhostReturnState : State
    {
        public bool updated;
        GameObject gameObject;

        public GhostReturnState(GameObject gameObject) : base("GhostReturn")
        {
            this.gameObject = gameObject;
        }

        public override void Start()
        {
            updated = false;
        }

        public override void FixedUpdate(GameTime gameTime)
        {
            if (!updated)
            {
                updated = true;

                Point newPosition;
                do
                {
                    newPosition = new Point(GameEnvironment.Random.Next(LevelLoader.grid.GetLength(0)), GameEnvironment.Random.Next(LevelLoader.grid.GetLength(1)));
                }
                while (LevelLoader.grid[newPosition.X, newPosition.Y].tileType == Tile.TileType.GROUND);

                gameObject.gridPosition = newPosition;
            }
        }
    }
    class EnemyGhost : EnemyWalking
    {
        private const int TrackingDistance = 10;

        public EnemyGhost(Point gridPosition) : base(gridPosition, "GameObjects/Enemies/EnemyGhost")
        {

        }

        public override void Initialize()
        {
            stateMachine = new StateMachine();

            //Add states to stateMachine
            stateMachine.AddState(new GhostPatrolState(this));
            stateMachine.AddState(new GhostChaseState(this));
            stateMachine.AddState(new GhostReturnState(this));
            stateMachine.AddState(new CryingState(this));

            //Add connections between states
            stateMachine.AddConnection("GhostPatrol", "GhostReturn", (object state) => (state as GhostPatrolState).stamina <= 0, stateMachine.GetState("GhostPatrol"));
            stateMachine.AddConnection("GhostChase", "GhostReturn", (object state) => (state as GhostChaseState).stamina <= 0, stateMachine.GetState("GhostChase"));
            stateMachine.AddConnection("GhostReturn", "GhostPatrol", (object state) => (state as GhostReturnState).updated, stateMachine.GetState("GhostReturn"));
            //stateMachine.AddConnectionToAll("GhostChase", (object state) =>
            //{
            //    float closestPlayer = float.PositiveInfinity;
            //    foreach (Player player in GameEnvironment.GetState<PlayingState>("PlayingState").players)
            //    {
            //        float distance = Vector2.Distance(player.gridPosition.ToVector2(), gridPosition.ToVector2());
            //        if (distance <= 10 && distance < closestPlayer)
            //        {
            //            closestPlayer = distance;
            //            (state as ChaseState).player = player;
            //        }
            //    }
            //    return float.IsFinite(closestPlayer);
            //}, stateMachine.GetState("GhostChase"));
            //stateMachine.AddConnectionToAll("Crying", () => !CanMove());

            //Set state to Patrol
            stateMachine.SetState("GhostPatrol");
        }

        public override void FixedUpdate(GameTime gameTime)
        {
            base.FixedUpdate(gameTime);

            stateMachine.FixedUpdate(gameTime);

            //Find closest player
            float closestPlayer = float.PositiveInfinity;
            if (GameEnvironment.GetState<PlayingState>("PlayingState").players.Count > 0)
            {
                foreach (Player player in GameEnvironment.GetState<PlayingState>("PlayingState").players)
                {
                    float distance = Vector2.Distance(player.gridPosition.ToVector2(), gridPosition.ToVector2());
                    if (distance <= TrackingDistance && distance < closestPlayer)
                    {
                        closestPlayer = distance;
                        (stateMachine.GetState("GhostChase") as GhostChaseState).player = player;
                    }
                }
            }

            //If a player was found in range then attack it
            if (float.IsFinite(closestPlayer))
            {
                stateMachine.SetState("GhostChase");
            }
        }
    }
}
