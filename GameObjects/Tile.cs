using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Text;

namespace BaseProject
{
    class Tile : GameObject
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

        public Tile(Tuple<Texture2D, TileType> tuple, Rectangle rectangle)
        {
            texture = tuple.Item1;
            tileType = tuple.Item2;
            this.rectangle = rectangle;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, Color.White);
        }
    }
}
