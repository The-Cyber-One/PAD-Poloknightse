using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Poloknightse
{
    class StartState : GameState
    {
        private const int TITLE_Y_OFFSET = 200;
        GameObjectList backgroundObjects = new GameObjectList();
        List<Point> secretPlaces = new List<Point>();

        public StartState()
        {

        }

        public override async void Init()
        {
            //Background
            LevelLoader.LoadLevel("MainMenu");
            AddSecret(new Coin());
            AddSecret(new HealthPickup());
            AddSecret(new EnemyGhost());
            AddSecret(new EnemyWalking());

            //Menu
            gameObjectList.Add(new TextGameObject("Poloknightse", new Vector2(GameEnvironment.Screen.X / 2, GameEnvironment.Screen.Y / 2 - TITLE_Y_OFFSET), Vector2.One / 2, Color.Black, "Fonts/Title"));
            gameObjectList.Add(new TextGameObject("Press any button to begin", new Vector2(GameEnvironment.Screen.X / 2, GameEnvironment.Screen.Y / 2), Vector2.One / 2));

            Debug.WriteLine((await HighscoreManager.LoadScore()).ToString());
        }

        private void AddSecret(GameObject gameObject)
        {
            do
            {
                gameObject.gridPosition = new Point(GameEnvironment.Random.Next(1, LevelLoader.grid.GetLength(0) - 2), GameEnvironment.Random.Next(1, LevelLoader.grid.GetLength(1) - 2));
            }
            while (!gameObject.IsInLevel || secretPlaces.Contains(gameObject.gridPosition) || LevelLoader.grid[gameObject.gridPosition.X, gameObject.gridPosition.Y].tileType == Tile.TileType.WALL);

            secretPlaces.Add(gameObject.gridPosition);
            backgroundObjects.Add(gameObject);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            LevelLoader.Draw(spriteBatch);
            backgroundObjects.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            if (inputHelper.AnyKeyPressed)
            {
                GameEnvironment.SwitchTo("LevelSelectState");
            }
            base.HandleInput(inputHelper);
        }
    }
}
