using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BaseProject
{
    class Player : GameObject
    {
        public Player() : base("Player/Onderbroek_Ridder")
        {
            velocity.X = 0.1f;
            velocity.Y = 0;
        }

        public override void Update(GameTime gameTime)
        { 
            position.X += velocity.X;
            base.Update(gameTime);
        }
    }
}
