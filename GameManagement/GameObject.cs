using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;


namespace BaseProject
{
	public class GameObject
	{
		protected Texture2D texture;
		protected Vector2 position;
		protected Vector2 velocity;
		protected Rectangle positionSize;

		public Vector2 Position
        {
			get { return position; }
			set { position = value; }
        }

		public GameObject(String assetName = null)
		{
			if (assetName != null)
			{
				texture = GameEnvironment.ContentManager.Load<Texture2D>(assetName);
				positionSize = new Rectangle(0, 0, GameEnvironment.gridTileSize, GameEnvironment.gridTileSize);
			}
		}

		public virtual void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(texture, positionSize, Color.White);
		}

		public virtual void Update(GameTime gameTime)
        {
			positionSize = new Rectangle(
				(int)position.X * GameEnvironment.gridTileSize + GameEnvironment.startGridPoint.X, 
				(int)position.Y * GameEnvironment.gridTileSize + GameEnvironment.startGridPoint.Y, 
				GameEnvironment.gridTileSize, 
				GameEnvironment.gridTileSize);
        }
		public virtual void FixedUpdate(GameTime gameTime)
        {
		}
	}
}
