using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Text;

namespace Poloknightse
{
    public class Tile : GameObject
    {
        public Rectangle rectangle;
        public TileType tileType;

        public Texture2D TileTexture
        {
            get { return texture; }
        }

        public enum TileType
        {
            WALL,
            GROUND
        }

        public Tile(string assetPath, TileType tileType, Rectangle rectangle) : base(Vector2.Zero.ToPoint())
        {
            texture = GameEnvironment.ContentManager.Load<Texture2D>(assetPath);
            this.tileType = tileType;
            this.rectangle = rectangle;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, Color.White);
        }
    }
}
