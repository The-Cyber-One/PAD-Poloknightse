using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Poloknightse
{
    class StartState : GameState
    {
        Vector2 titleTextPosition = new Vector2(32, 10.5f);
        Point buttonPosition = new Point(28, 14);
        Point buttonSize = new Point(8, 8);
        string startButtonAssetName = "Menu/Start";
        string startButtonText = "Click button to start";
        Button mainMenuButton;

        GameObjectList backgroundObjects = new GameObjectList();
        List<Point> secretPlaces = new List<Point>();

        public StartState()
        {

        }

        public override async void Init()
        {
            //Background
            LevelLoader.LoadLevel("Menu/MainMenu");
            AddSecret(new Coin());
            AddSecret(new HealthPickup());
            AddSecret(new EnemyGhost());
            AddSecret(new EnemyWalking());

            //Menu
            Vector2 convertedtitleTextPosition = LevelLoader.GridPointToWorld(titleTextPosition);
            Point convertedButtonPosition = LevelLoader.GridPointToWorld(buttonPosition).ToPoint();
            Point convertedButtonSize = LevelLoader.GridPointToWorld(buttonSize).ToPoint();
            gameObjectList.Add(new TextGameObject("Poloknightse", convertedtitleTextPosition, Vector2.One / 2, Color.Black, "Fonts/Title"));
            Rectangle button = new Rectangle(convertedButtonPosition, convertedButtonSize);
            mainMenuButton = new Button(button, startButtonAssetName, startButtonText);
            gameObjectList.Add(mainMenuButton);

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
            Game1.exit = GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || inputHelper.KeyPressed(Keys.Escape);

            if (mainMenuButton.clicked)
            {
                GameEnvironment.SwitchTo("LevelSelectState");
            }

            base.HandleInput(inputHelper);
        }
    }
}
