using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace BaseProject
{
    class EnemyShooter : GameObject
    {


        public EnemyShooter(Vector2 gridPosition) : base(gridPosition, "GameObjects/Player/Onderbroek_ridder")
        {
            velocity.Y = 1;
        }

        /// <summary>
        /// Shoot bullet in given <paramref name="direction"/>
        /// </summary>
        /// <param name="direction">Direction to shoot de bullet to</param>
        private void Shoot(Vector2 direction)
        {
            GameEnvironment.CurrentGameState.gameObjectList.Add(new Bullet(gridPosition, direction));
        }

		public override void FixedUpdate(GameTime gameTime)
		{
			base.Update(gameTime);

            gridPosition += velocity;

            Debug.WriteLine(gridPosition);

            if(gridPosition == new Vector2(1, 26)|| gridPosition == new Vector2(1, 3))
                velocity *= -1;
			
		}
	}
}
