using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;


namespace BaseProject
{
	public class GameObject
	{
		protected Texture2D texture;
		public Vector2 gridPosition;
		public Vector2 velocity;
		protected Rectangle positionSize;

		public GameObject(string assetName = null)
		{
			if (assetName != null)
			{
				texture = GameEnvironment.ContentManager.Load<Texture2D>(assetName);
				positionSize = new Rectangle(0, 0, GameEnvironment.gridTileSize, GameEnvironment.gridTileSize);
			}
		}

		/// <summary>
		/// HandleInput will be called every frame before <see cref="Update(GameTime)"/>
		/// </summary>
		/// <param name="inputHelper">Will help with getting input data</param>
		public virtual void HandleInput(InputHelper inputHelper)
		{
		}

		/// <summary>
		/// Update will be called every frame
		/// </summary>
		/// <param name="gameTime">Stores game time data</param>
		public virtual void Update(GameTime gameTime)
		{
			positionSize = new Rectangle(
				(int)gridPosition.X * GameEnvironment.gridTileSize + GameEnvironment.startGridPoint.X,
				(int)gridPosition.Y * GameEnvironment.gridTileSize + GameEnvironment.startGridPoint.Y,
				GameEnvironment.gridTileSize,
				GameEnvironment.gridTileSize);
		}

		/// <summary>
		/// FixedUpdate will be called on a set time frame
		/// </summary>
		/// <param name="gameTime">Stores game time data</param>
		public virtual void FixedUpdate(GameTime gameTime)
		{
		}

		public virtual void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(texture, positionSize, Color.White);
		}
		// boolean to check if to objects collide
		public bool CheckCollision(GameObject gameObject)
		{
			float x0 = this.gridPosition.X,
				  y0 = this.gridPosition.Y,
				  x1 = gameObject.gridPosition.X,
				  y1 = gameObject.gridPosition.Y;
			if (x0 == x1 && y0 == y1)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}
