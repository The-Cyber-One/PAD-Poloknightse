using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace BaseProject
{
    class Bullet : GameObject
    {
        const float SPEED = 1;

        public Bullet(Vector2 gridPosition, Vector2 direction) : base(gridPosition, "GameObjects/bomb")
        {
            direction.Normalize();
            velocity = direction * SPEED;
        }

        public bool CheckBulletOutOfBounds()
		{
            if (this.gridPosition.X >= LevelLoader.grid.GetLength(0) ||
                this.gridPosition.X < 0 ||
                this.gridPosition.Y >= LevelLoader.grid.GetLength(1) ||
                this.gridPosition.Y < 0)
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

            gridPosition += velocity;
                Debug.WriteLine("balls");
                //Delete bullet
                for (int i = GameEnvironment.CurrentGameState.gameObjectList.Count - 1; i >= 0; i--)
                {
                    if (GameEnvironment.CurrentGameState.gameObjectList[i] is Bullet)
                    {
                    //check bounds right & left & up & down
                    if(CheckBulletOutOfBounds())
                    {
                        GameEnvironment.CurrentGameState.gameObjectList.Remove(GameEnvironment.CurrentGameState.gameObjectList[i]);
                        continue;
                    }
                }
            }
        }
	}
}
