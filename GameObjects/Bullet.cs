using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Poloknightse
{
    class Bullet : GameObject
    {
        const float SPEED = 1;

        public Bullet(Point gridPosition, Vector2 direction) : base(gridPosition, "GameObjects/bomb")
        {
            direction.Normalize();
            velocity = direction * SPEED;
        }

		public override void FixedUpdate(GameTime gameTime)
		{
			base.FixedUpdate(gameTime);

            gridPosition += velocity.ToPoint(); ;
        }
	}
}
