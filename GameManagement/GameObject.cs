using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;


namespace Poloknightse
{
	public class GameObject
	{
		protected GameObject parent;
		protected Texture2D texture;
		protected int layer;
		protected string id;
		public Point gridPosition;
		public Vector2 velocity;
		protected Rectangle positionSize;

		public GameObject(Point gridPosition, string assetName = null, int layer = 0)
		{
			this.gridPosition = gridPosition;
			if (assetName != null)
			{
				texture = GameEnvironment.ContentManager.Load<Texture2D>(assetName);
				positionSize = new Rectangle(0, 0, GameEnvironment.gridTileSize, GameEnvironment.gridTileSize);
			}
			this.layer = layer;
		}

		/// <summary>
		/// This will be called to reset the gameobject
		/// </summary>
		public virtual void Reset()
		{

		}

		/// <summary>
		/// This will be called after the level is loaded
		/// </summary>
		public virtual void Initialize()
        {

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

		public virtual void Reset() { }

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
		public GameObject Root
		{
			get
			{
				if (parent != null)
				{
					return parent.Root;
				}
				else
				{
					return this;
				}
			}
		}
		public virtual GameObject Parent
		{
			get { return parent; }
			set { parent = value; }
		}
		public virtual int Layer
		{
			get { return layer; }
			set { layer = value; }
		}
		public string Id
		{
			get { return id; }
		}
	}
}
