 using Microsoft.Xna.Framework;
using System;

namespace Poloknightse
{
	class EnemyShooter : GameObject
    {
        private float countDuration = 5f;
        private float currentTime = 0f;
        private int endCalculationEdgeCheckX, endCalculationEdgeCheckY;
        private Point enemyPos;
        private Vector2 shootDir;

        public EnemyShooter(Point gridPosition) : base(gridPosition, "GameObjects/Enemies/EnemyShooter")
        {
        }

		public override void Initialize()
		{
			base.Initialize();

            //Check where the enemy is placed and set the velocity, shoot direction and point position according to that.
            //Left
            if (LevelLoader.grid[(int)gridPosition.X + 1, (int)gridPosition.Y].tileType == Tile.TileType.WALL)
            {
                enemyPos = new Point(1,-2);
                velocity.X = 0;
                velocity.Y = 1;
                shootDir = new Vector2(1, 0);
            }
            //Right
            else if (LevelLoader.grid[(int)gridPosition.X - 1, (int)gridPosition.Y].tileType == Tile.TileType.WALL)
            {
                enemyPos = new Point(-1, -2);
                velocity.X = 0;
                velocity.Y = 1;
                shootDir = new Vector2(-1, 0);
            }

            //Bottom
            if (LevelLoader.grid[(int)gridPosition.X, (int)gridPosition.Y + 1].tileType == Tile.TileType.WALL)
            {
                enemyPos = new Point(-2,1);
                velocity.X = 1;
                velocity.Y = 0;
                shootDir = new Vector2(0, 1);
            }
            //Top
            else if(LevelLoader.grid[(int)gridPosition.X, (int)gridPosition.Y - 1].tileType == Tile.TileType.WALL)
            {
                enemyPos = new Point(-2,-1);
                velocity.X = 1;
                velocity.Y = 0;
                shootDir = new Vector2(0, -1);
            }
        }

        private void MovementPathCheck()
        {

            //Change the calculation for the movement depending on the position.
            if (velocity.X == 0)
            {
                endCalculationEdgeCheckX = (int)gridPosition.X + enemyPos.X;
                endCalculationEdgeCheckY = (int)gridPosition.Y - enemyPos.Y;
            }
            else
            {
                endCalculationEdgeCheckX = (int)gridPosition.X - enemyPos.X;
                endCalculationEdgeCheckY = (int)gridPosition.Y + enemyPos.Y;
            }

            //check depending on enemyPos what tiles to check for it to detect to move the opposite way.
            if (LevelLoader.grid[(int)gridPosition.X + enemyPos.X, (int)gridPosition.Y + enemyPos.Y].tileType != Tile.TileType.WALL ||
                LevelLoader.grid[endCalculationEdgeCheckX, endCalculationEdgeCheckY].tileType != Tile.TileType.WALL)
            {
                velocity *= -1;
            }

        }

        /// <summary>
        /// Shoot bullet in given <paramref name="direction"/>
        /// </summary>
        /// <param name="direction">Direction to shoot the bullet to</param>
        private void Shoot(Vector2 direction)
        {
            GameEnvironment.CurrentGameState.gameObjectList.Add(new Bullet(gridPosition, direction));
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

        public override void Draw(SpriteBatch spriteBatch)
        {
            positionSize = new Rectangle(
            gridPosition.X * LevelLoader.gridTileSize + GameEnvironment.startGridPoint.X + (int)(LevelLoader.gridTileSize / 2),
            gridPosition.Y * LevelLoader.gridTileSize + GameEnvironment.startGridPoint.Y + (int)(LevelLoader.gridTileSize),
            (int)(texture.Width * LevelLoader.scalingFactor),
            (int)(texture.Height * LevelLoader.scalingFactor));
            spriteBatch.Draw(texture, positionSize, null, Color.White, 0, new Vector2(texture.Width / 2, texture.Height), SpriteEffects.None, 1);
        }
    }
}
