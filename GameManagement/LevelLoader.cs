using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;

namespace Poloknightse
{
    class LevelLoader
    {
        private static int gridTileSize = GameEnvironment.gridTileSize;
        public static Tile[,] grid;

        private static Dictionary<Color, Tuple<Type, string, Tile.TileType>> colorTilePairs = new Dictionary<Color, Tuple<Type, string, Tile.TileType>>()
        {
            //Tiles
            {
                Color.Black,                                //Color in png
                new Tuple<Type, string, Tile.TileType>(     //
                    typeof(Tile),                           //Extra GameObject to instantiate (if it is Tile then no extra GameObject will be instantiated)
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
            //Pickups
            {
                Color.Brown,
                new Tuple<Type, string, Tile.TileType>(
                    typeof(Coin),
                    "LevelTiles/Ground",
                    Tile.TileType.GROUND)
            },
            {
                Color.MediumPurple,
                new Tuple<Type, string, Tile.TileType>(
                    typeof(HealthPickup),
                    "LevelTiles/Ground",
                    Tile.TileType.GROUND)
            },
            //Player
            {
                Color.Blue,
                new Tuple<Type, string, Tile.TileType>(
                    typeof(Player),
                    "LevelTiles/Ground",
                    Tile.TileType.GROUND)
            },
            {
                Color.CornflowerBlue,
                new Tuple<Type, string, Tile.TileType>(
                    typeof(PlayerFollower),
                    "LevelTiles/Ground",
                    Tile.TileType.GROUND)
            },
            //Enemies
            {
                Color.Red,
                new Tuple<Type, string, Tile.TileType>(
                    typeof(EnemyShooter),
                    "LevelTiles/Ground",
                    Tile.TileType.GROUND)
            },
            {
                Color.OrangeRed,
                new Tuple<Type, string, Tile.TileType>(
                    typeof(/*change Tile for walking enemy*/ Tile),
                    "GameObjects/LevelTiles/Ground",
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
            grid = new Tile[level.Width, level.Height];
            Player player = new Player(new Vector2());
            Dictionary<Point, PlayerFollower> positionFollowerPairs = new Dictionary<Point, PlayerFollower>();

            for (int i = 0; i < colors.Length; i++)
            {
                //Safety check
                if (!colorTilePairs.ContainsKey(colors[i]))
                {
                    Debug.WriteLine("The color" + colors[i] + " is not a valid color");
                    Debug.Indent();
                    Debug.WriteLine("THIS WILL CAUSE ERRORS");
                    Debug.Unindent();
                    continue;
                }

                //Get associated data from color
                Tuple<Type, string, Tile.TileType> tilePairs = colorTilePairs[colors[i]];

                //Get position
                int x = i % level.Width;
                int y = i / level.Height;
                Rectangle rectangle = new Rectangle(x * gridTileSize + xOffset, y * gridTileSize + yOffset, gridTileSize, gridTileSize);

                //Set tile
                grid[x, y] = new Tile(tilePairs.Item2, tilePairs.Item3, rectangle);

                //Add extra GameObject
                if (tilePairs.Item1 != typeof(Tile))
                {
                    Vector2 gridPosition = new Vector2(x, y);
                    GameObject gameObject = Activator.CreateInstance(tilePairs.Item1, gridPosition) as GameObject;
                    GameEnvironment.CurrentGameState.gameObjectList.Add(gameObject);

                    if (gameObject is Player)
                    {
                        player = gameObject as Player;
                    }
                    else if (gameObject is PlayerFollower)
                    {
                        positionFollowerPairs.Add(new Point(x, y), gameObject as PlayerFollower);
                    }
                }
            }

            if (player != null)
            {
                player.LoadFollowers(positionFollowerPairs);
                if (GameEnvironment.CurrentGameState is PlayingState)
                {
                    (GameEnvironment.CurrentGameState as PlayingState).player = player;
                }
            }
        }

        /// <summary>
        /// Draw entire level
        /// </summary>
        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (Tile tile in grid)
            {
                if (tile != null)
                    tile.Draw(spriteBatch);
            }
        }
    }
}
