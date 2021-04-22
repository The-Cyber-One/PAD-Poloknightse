using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Poloknightse
{
    class GhostChaseState : ChaseState
    {
        int ghostCooldownSteps = 5;
        int stepsCounter;

        public GhostChaseState(GameObject gameObject, Player player) : base(gameObject, player)
        {

        }

        public override void Start()
        {
            base.Start();
            stepsCounter = ghostCooldownSteps;
        }

        public override Point[] GetNewPath()
        {
            if (stepsCounter <= 0)
            {
                stepsCounter = ghostCooldownSteps;
                return AStar.FindPath(gameObject.gridPosition, player.GetCenter(), true);
            }
            else
            {
                return base.GetNewPath();
            }
        }

        public override void FixedUpdate(GameTime gameTime)
        {
            stepsCounter--;
            base.FixedUpdate(gameTime);

        }
    }

    class GhostPatrolState : PatrolState
    {
        int ghostCooldownSteps = 5;
        int stepsCounter;

        public GhostPatrolState(GameObject gameObject) : base(gameObject)
        {

        }

        public override void Start()
        {
            base.Start();
            stepsCounter = ghostCooldownSteps;
        }

        public override Point GetRandomDirection()
        {
            if (stepsCounter > 0) return base.GetRandomDirection();

            stepsCounter = ghostCooldownSteps;
            if (GameEnvironment.Random.Next(2) == 0)
            {
                return new Point(GameEnvironment.Random.Next(2) * 2 - 1, 0);
            }
            else
            {
                return new Point(0, GameEnvironment.Random.Next(2) * 2 - 1);
            }
        }

        public override void FixedUpdate(GameTime gameTime)
        {
            stepsCounter--;
            base.FixedUpdate(gameTime);

        }
    }

    class EnemyGhost : EnemyWalking
    {
        public EnemyGhost(Point gridPosition) : base(gridPosition, "GameObjects/Enemies/EnemyGhost")
        {

        }

    }
}
