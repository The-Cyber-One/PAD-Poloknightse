using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Poloknightse
{
    class EnemyWalking : GameObject
    {
        int stamina = 5;
        int maxPatrolDistance = 3;
        Point startPosition;
        int patrolDistance;

        State currentState;

        enum State
        {
            Patrol,
            Chase,
            Return
        }

        public EnemyWalking(Point gridPosition) : base(gridPosition, "GameObjects/Player/Koning")
        {
            startPosition = gridPosition;
            //SetState(State.Patrol);
        }

        public override void FixedUpdate(GameTime gameTime)
        {
            base.FixedUpdate(gameTime);
            switch (currentState)
            {
                case State.Patrol:
                    {
                        if (patrolDistance <= 0)
                        {
                            SetState(State.Return);
                            break;
                        }

                        Point direction = new Point(GameEnvironment.Random.Next(2) * 2 - 1, GameEnvironment.Random.Next(1) * 2 - 1);
                        while (LevelLoader.grid[gridPosition.X + direction.X, gridPosition.Y + direction.Y].tileType == Tile.TileType.WALL)
                        {
                            direction = new Point(GameEnvironment.Random.Next(2) * 2 - 1, GameEnvironment.Random.Next(1) * 2 - 1);
                        }
                        gridPosition += direction;
                        break;
                    }
                case State.Chase:
                    break;
                case State.Return:

                    break;
            }
        }

        private void SetState(State newState)
        {
            currentState = newState;
            patrolDistance = maxPatrolDistance;
        }
    }
}
