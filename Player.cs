using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BaseProject
{
    class Player : GameObject
    {
        public int movementDirection;
        public Player() : base("Player/Onderbroek_Ridder")
        {
            velocity.X = 0.1f;
            velocity.Y = 0.1f;
            movementDirection = 0;
        }

        public override void Update(GameTime gameTime)
        {
            if (GameEnvironment.KeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D))
            {
                movementDirection = 1;
            } else if (GameEnvironment.KeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A))
            {
                movementDirection = 2;
            } else if (GameEnvironment.KeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W))
            {
                movementDirection = 3;
            } else if (GameEnvironment.KeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S))
            {
                movementDirection = 4;
            }

            if (movementDirection == 1)
            {
                position.X += velocity.X;
            } else if (movementDirection == 2)
            {
                position.X -= velocity.X;
            } else if (movementDirection == 3)
            {
                position.Y -= velocity.Y;
            } else if (movementDirection == 4)
            {
                position.Y += velocity.Y;
            }
            base.Update(gameTime);
        }
    }
}
