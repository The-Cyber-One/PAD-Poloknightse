using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;

namespace BaseProject
{
    class LevelLoader
    {
        private static int gridTileSize = GameEnvironment.gridTileSize;
        public static Tile[,] tiles;

        private static Dictionary<Color, Tuple<Type, string, Tile.TileType>> colorTilePairs = new Dictionary<Color, Tuple<Type, string, Tile.TileType>>()
        {
            {
                Color.Black,                                //Color in png
                new Tuple<Type, string, Tile.TileType>(     //
                    typeof(Tile),                           //Class to instantiate
                    "LevelTiles/Wall",                      //Tile texture path
                    Tile.TileType.WALL)                     //Associated TileType
            },
            { 
                Color.White, 
                new Tuple<Type, string, Tile.TileType>(
                    typeof(Tile),
                    "LevelTiles/Ground",
                    Tile.TileType.GROUND) 
            },
            {
                Color.Brown,
                new Tuple<Type, string, Tile.TileType>(
                    typeof(Coin),
                    "LevelTiles/Ground",
                    Tile.TileType.GROUND)
            }
        };

        /// <summary>
        /// Load the level from a png file.
        /// </summary>
        /// <param name="levelName">Filename from the levels folder.</param>
        public static void LoadLevel(string levelName)
        {
            //Load the colors to a array
            Texture2D level = GameEnvironment.ContentManager.Load<Texture2D>("Levels/" + levelName);
            Color[] colors = new Color[level.Width * level.Height];
            level.GetData(colors);

            //Change the tile size and calculate the center
            GameEnvironment.gridTileSize = GameEnvironment.Screen.Y / level.Height;
            int xOffset = GameEnvironment.Screen.X / 2 - (level.Width / 2) * gridTileSize;
            int yOffset = GameEnvironment.Screen.Y / 2 - (level.Height / 2) * gridTileSize;
            GameEnvironment.startGridPoint = new Point(xOffset, yOffset);

            //Here we check the colors of the image and load the correct tiles.
            tiles = new Tile[level.Width, level.Height];

            for (int i = 0; i < colors.Length; i++)
            {
                Tuple<Type, string, Tile.TileType> tilePairs = colorTilePairs[colors[i]];
                int x = i % level.Width;
                int y = i / level.Height;
                Rectangle rectangle = new Rectangle(x * gridTileSize + xOffset, y * gridTileSize + yOffset, gridTileSize, gridTileSize);

                //Set tile
                tiles[x, y] = new Tile(tilePairs.Item2, tilePairs.Item3, rectangle);
                //Debug.WriteLine(tiles[x, y].tileType);

                //Add extra GameObject
                if (tilePairs.Item1 != typeof(Tile))
                {
                    Debug.WriteLine("Coin");
                    Vector2 gridPosition = new Vector2(x, y);
                    GameObject gameObject = Activator.CreateInstance(tilePairs.Item1, gridPosition) as GameObject;
                    GameEnvironment.CurrentGameState.gameObjectList.Add(gameObject);
                }
                
            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (Tile tile in tiles)
            {
                tile.Draw(spriteBatch);
            }
        }
    }
}
