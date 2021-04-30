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

        public GameObject(Point? gridPosition = null, string assetName = null, int layer = 0)
        {
            if (gridPosition == null) gridPosition = new Point();
            this.gridPosition = gridPosition.Value;
            if (assetName != null)
            {
                texture = GameEnvironment.ContentManager.Load<Texture2D>(assetName);
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
            positionSize = new Rectangle(
                gridPosition.X * LevelLoader.gridTileSize + GameEnvironment.startGridPoint.X,
                gridPosition.Y * LevelLoader.gridTileSize + GameEnvironment.startGridPoint.Y,
                LevelLoader.gridTileSize,
                LevelLoader.gridTileSize);
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

        public bool IsInLevel
        {
            get
            {
                return LevelLoader.grid.GetLength(0) > gridPosition.X &&
                    LevelLoader.grid.GetLength(1) > gridPosition.Y &&
                    gridPosition.X >= 0 &&
                    gridPosition.Y >= 0;
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
