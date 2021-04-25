using Microsoft.Xna.Framework;
using System;

namespace Poloknightse
{
	class EnemyShooter : GameObject
    {
        private float countDuration = 5f; //Every 5s.
        private float currentTime = 0f;
        private int xOffsetCheck, yOffsetCheck;
        private int endCalculationX, endCalculationY;
        private String enemyPos;
        private Vector2 shootDir;

        public EnemyShooter(Point gridPosition) : base(gridPosition, "GameObjects/Player/Onderbroek_Ridder")
        {
        }

        /// <summary>
        /// Shoot bullet in given <paramref name="direction"/>
        /// </summary>
        /// <param name="direction">Direction to shoot de bullet to</param>
        private void Shoot(Vector2 direction)
        {
            GameEnvironment.CurrentGameState.gameObjectList.Add(new Bullet(gridPosition, direction));
        }

        private void MovementPathCheck()
		{
            if (enemyPos == "left" || enemyPos == "right")
            {
                xOffsetCheck = 1;
                yOffsetCheck = 2;
                endCalculationX = (int)gridPosition.X + xOffsetCheck;
                endCalculationY = (int)gridPosition.Y - yOffsetCheck;
            }
			else if(enemyPos == "top" || enemyPos == "bottom")
            {
                xOffsetCheck = 2;
                yOffsetCheck = 1;
                endCalculationX = (int)gridPosition.X - xOffsetCheck;
                endCalculationY = (int)gridPosition.Y + yOffsetCheck;
            }
            
            //check depending on enemyPos what tiles to check for it to detect to move the opposite way.
            if (LevelLoader.grid[(int)gridPosition.X + xOffsetCheck, (int)gridPosition.Y + yOffsetCheck].tileType != Tile.TileType.WALL ||
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
                enemyPos = "left";
                velocity.X = 0;
                velocity.Y = 1;
                shootDir = new Vector2(1, 0);
            }
            if (LevelLoader.grid[(int)gridPosition.X - 1, (int)gridPosition.Y].tileType == Tile.TileType.WALL)
            {
                enemyPos = "right";
                velocity.X = 0;
                velocity.Y = 1;
                shootDir = new Vector2(-1, 0);
            }
            if (LevelLoader.grid[(int)gridPosition.X, (int)gridPosition.Y + 1].tileType == Tile.TileType.WALL)
            {
                enemyPos = "top";
                velocity.X = 1;
                velocity.Y = 0;
                shootDir = new Vector2(0, 1);
            }
            if (LevelLoader.grid[(int)gridPosition.X, (int)gridPosition.Y - 1].tileType == Tile.TileType.WALL)
            {
                enemyPos = "bottom";
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
            currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds; //Time passed since last Update() 

            if (currentTime >= countDuration)
            {
                currentTime -= countDuration;
                Shoot(shootDir);
            }
        }
    }
}
