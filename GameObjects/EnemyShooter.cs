 using Microsoft.Xna.Framework;
using System;

namespace Poloknightse
{
	class EnemyShooter : GameObject
    {
        private float countDuration = 5f;
        private float currentTime = 0f;
        private int endCalculationX, endCalculationY;
        private Point enemyPos;
        private Vector2 shootDir;

        public EnemyShooter(Point gridPosition) : base(gridPosition, "GameObjects/Player/Onderbroek_Ridder")
        {
        }

        /// <summary>
        /// Shoot bullet in given <paramref name="direction"/>
        /// </summary>
        /// <param name="direction">Direction to shoot the bullet to</param>
        private void Shoot(Vector2 direction)
        {
            GameEnvironment.CurrentGameState.gameObjectList.Add(new Bullet(gridPosition, direction));
        }

        private void MovementPathCheck()
		{
              
            //Change the calculation for the movement depending on the position.
            if(velocity.X == 0)
			{
                endCalculationX = (int)gridPosition.X + enemyPos.X;
                endCalculationY = (int)gridPosition.Y - enemyPos.Y;
            }
            else
			{
                endCalculationX = (int)gridPosition.X - enemyPos.X;
                endCalculationY = (int)gridPosition.Y + enemyPos.Y;
            }
            
            

            //check depending on enemyPos what tiles to check for it to detect to move the opposite way.
            if (LevelLoader.grid[(int)gridPosition.X + enemyPos.X, (int)gridPosition.Y + enemyPos.Y].tileType != Tile.TileType.WALL ||
                LevelLoader.grid[endCalculationX, endCalculationY].tileType != Tile.TileType.WALL)
            {
                velocity *= -1;
            }
			
        }

		public override void Initialize()
		{
			base.Initialize();

            if (LevelLoader.grid[(int)gridPosition.X + 1, (int)gridPosition.Y].tileType == Tile.TileType.WALL)
            {
                enemyPos = new Point(1,-2);
                velocity.X = 0;
                velocity.Y = 1;
                shootDir = new Vector2(1, 0);
            }
            else if (LevelLoader.grid[(int)gridPosition.X - 1, (int)gridPosition.Y].tileType == Tile.TileType.WALL)
            {
                enemyPos = new Point(-1, -2);
                velocity.X = 0;
                velocity.Y = 1;
                shootDir = new Vector2(-1, 0);
            }

            if (LevelLoader.grid[(int)gridPosition.X, (int)gridPosition.Y + 1].tileType == Tile.TileType.WALL)
            {
                enemyPos = new Point(-2,1);
                velocity.X = 1;
                velocity.Y = 0;
                shootDir = new Vector2(0, 1);
            }
            else if(LevelLoader.grid[(int)gridPosition.X, (int)gridPosition.Y - 1].tileType == Tile.TileType.WALL)
            {
                enemyPos = new Point(-2,-1);
                velocity.X = 1;
                velocity.Y = 0;
                shootDir = new Vector2(0, -1);
            }
        }

		public override void FixedUpdate(GameTime gameTime)
		{
			base.Update(gameTime);

            gridPosition += velocity.ToPoint();

            MovementPathCheck();
        }


		public override void Update(GameTime gameTime)
		{

            base.Update(gameTime);
            currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (currentTime >= countDuration)
            {
                currentTime -= countDuration;
                Shoot(shootDir);
            }
        }
    }
}
