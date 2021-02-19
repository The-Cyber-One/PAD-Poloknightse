using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;


namespace BaseProject
{
	public class GameObject
	{
		protected const int gridSize = 8;
		protected Texture2D texture;
		protected Vector2 position;
		protected Vector2 velocity;

		public GameObject(String assetName)
		{
			texture = GameEnvironment.ContentManager.Load<Texture2D>(assetName);
		}

		public virtual void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(texture, position, Color.White);
		}
	}
}
