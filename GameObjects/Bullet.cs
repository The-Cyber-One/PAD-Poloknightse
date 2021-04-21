using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

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

        public bool CheckBulletOutOfBounds()
		{
            //check if the ball is out of bounds on either the x or y axis.
            if (this.gridPosition.X >= LevelLoader.grid.GetLength(0) || this.gridPosition.X < 0 ||
                this.gridPosition.Y >= LevelLoader.grid.GetLength(1) || this.gridPosition.Y < 0)
			{
                return true;
			}
			else
			{
                return false;
			}
		}

        public override void FixedUpdate(GameTime gameTime)
        {
            base.FixedUpdate(gameTime);

            gridPosition += velocity.ToPoint();
            
            //remove from the gameObjectList is out of bounds.
            if(CheckBulletOutOfBounds())
            {
                GameEnvironment.CurrentGameState.gameObjectList.Remove(this);
            }
        }
            
        
	}
}
