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
        //Variables for the title text
        Vector2 titleTextPosition = new Vector2(32, 10.5f);
        TextGameObject titleTextObject;
        string titleText = "PoloKnightse";

        //Variables for the buttons
        Point buttonStartPosition = new Point(18, 14);
        Point buttonSize = new Point(8, 8);
        Point offset = new Point(2, 4);
        string[] buttonAssetName = //Array of button asset names
            {
            "Highscore",
            "Start",
            "Credits"
            };
        string[] gameStateNames = //Array of the game states the buttons refer to
        {
            "Highscore",
            "LevelSelect",
            "Credits"
        };
        GameObjectList buttons = new GameObjectList();
        int numberOfButtons = 3;


        //Variables for the main menu secrets
        GameObjectList backgroundObjects = new GameObjectList();
        List<Point> secretPlaces = new List<Point>();

        public StartState()
        {
        }

        public override async void Init()
        {
            //Background
            LevelLoader.LoadLevel("Menu/StandardMenu");
            AddSecret(new Coin());
            AddSecret(new HealthPickup());
            AddSecret(new EnemyGhost());
            AddSecret(new EnemyWalking());


            //Create the title text
            Vector2 convertedtitleTextPosition = LevelLoader.GridPointToWorld(titleTextPosition); //Convert the grid position to real screen coordinates
            titleTextObject = new TextGameObject(titleText, convertedtitleTextPosition, Vector2.One / 2, Color.Black, "Fonts/Title");
            gameObjectList.Add(titleTextObject);


            //Buttons
            //First convert the grid positions to real screen coordinates
            Point convertedButtonPosition = LevelLoader.GridPointToWorld(buttonStartPosition).ToPoint();
            Point convertedButtonSize = LevelLoader.GridPointToWorld(buttonSize).ToPoint();
            Point convertedOffset = LevelLoader.GridPointToWorld(offset).ToPoint();
            Point positionOffset = convertedButtonSize + convertedOffset;
            //Create the buttons
            for (int i = 0; i < numberOfButtons; i++)
            {
                Rectangle button = new Rectangle(convertedButtonPosition.X + positionOffset.X * i, convertedButtonPosition.Y, convertedButtonSize.X, convertedButtonSize.Y);
                buttons.Add(new MainMenuButton(button, buttonAssetName[i], buttonAssetName[i], gameStateNames[i]));
            }
            gameObjectList.Add(buttons);
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
            Game1.exit = GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || inputHelper.KeyPressed(Keys.Escape) || inputHelper.KeyPressed(Keys.Back);
            
            foreach (MainMenuButton button in buttons.Children)
            {
                //Switch to the corresponding gameState when a button is clicked
                if (button.clicked)
                {
                    
                    GameEnvironment.SwitchTo(button.gameStateName +"State");
                    break;
                }
            }
            base.HandleInput(inputHelper);
        }
    }
}
