using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;

namespace BaseProject
{
    class LevelLoader : GameObject
	{
        static Texture2D wallTile;
        static Texture2D groundTile;
        static Tile[,] tiles;
        
        class Tile
        {
            public Texture2D tileTexture;
            public Rectangle rectangle;

            public Tile(Texture2D tileTexture, Rectangle rectangle)
            {
                this.tileTexture = tileTexture;
                this.rectangle = rectangle;
            }
        }

        public LevelLoader() : base("LevelTiles/Cell03")
        {

        }

        public static void LoadLevel(string levelName)
        {
            Texture2D level = GameEnvironment.ContentManager.Load<Texture2D>("Levels/" + levelName);
            Color[] colors = new Color[level.Width * level.Height];
            level.GetData<Color>(colors);

            Color wall = new Color(0, 0, 0, 255);
            Color ground = new Color(255, 255, 255, 255);

            wallTile = GameEnvironment.ContentManager.Load<Texture2D>("LevelTiles/Cell20");
            groundTile = GameEnvironment.ContentManager.Load<Texture2D>("LevelTiles/Cell03");

            tiles = new Tile[level.Width, level.Height];

            for (int i = 0; i < colors.Length; i++)
            {
                Color pixel = colors[i];
                int x = i % level.Width;
                int y = i / level.Height;
                Rectangle rectangle = new Rectangle(x * gridSize, y * gridSize, gridSize, gridSize);

                Debug.WriteLine(x + ", " + y);
                if (pixel == wall)
                {
                    tiles[x,y] = new Tile(wallTile, rectangle);
                }
                else if (pixel == ground)
                {
                    tiles[x,y] = new Tile(groundTile, rectangle);
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (Tile tile in tiles)
            {
                spriteBatch.Draw(tile.tileTexture, tile.rectangle, Color.White);
            }
        }
    }
}
