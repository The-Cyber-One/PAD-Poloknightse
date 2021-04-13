using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Poloknightse
{
    class EnemyShooter : GameObject
    {
        private float countDuration = 5f; //Every  5s.
        private float currentTime = 0f;

        public EnemyShooter(Point gridPosition) : base(gridPosition, "GameObjects/Player/Onderbroek_ridder")
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

            gridPosition += velocity.ToPoint();

            //Check where the wall ends and inverse the velocity so it goes the opposite direction.
            if(LevelLoader.grid[(int)gridPosition.X + 1, (int)gridPosition.Y + 2].tileType != Tile.TileType.WALL||
               LevelLoader.grid[(int)gridPosition.X + 1, (int)gridPosition.Y - 2].tileType != Tile.TileType.WALL)
            {
                velocity *= -1;
            }
        }

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
            currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds; //Time passed since last Update() 

            if (currentTime >= countDuration)
            {
                currentTime -= countDuration;
                Shoot(new Vector2(1, 0));
            }
        }
    }
}
