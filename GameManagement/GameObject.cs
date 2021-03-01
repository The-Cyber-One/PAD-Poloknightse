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

		public GameObject(String assetName)
		{
			texture = GameEnvironment.ContentManager.Load<Texture2D>(assetName);
			positionSize = new Rectangle(0, 0, GameEnvironment.gridTileSize, GameEnvironment.gridTileSize);
		}

		public virtual void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(texture, positionSize, Color.White);
		}

		public virtual void Update(GameTime gameTime)
        {
			positionSize = new Rectangle((int)position.X * GameEnvironment.gridTileSize, (int)position.Y * GameEnvironment.gridTileSize, 
				GameEnvironment.gridTileSize, GameEnvironment.gridTileSize);
        }
		public virtual void fixedUpdate(GameTime gameTime)
        {

        }
	}
}
