using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Poloknightse
{
    class PauseState : GameState
    {
        //Variables for the title text
        Vector2 titleTextPosition = new Vector2(32, 10.5f);
        TextGameObject titleTextObject;
        string titleText = "The game is paused";

        //Variables for the buttons
        Point restartButtonPosition = new Point(36, 14), levelSelectButtonPosition = new Point(20, 14);
        Point buttonSize = new Point(8, 8);
        string restartButtonAssetName = "Restart", levelSelectButtonAssetName = "Back";
        string restartButtonText = "Restart level", levelSelectButtonText = "Level select menu";
        Button restartButton, levelSelectButton;
        public PauseState()
        {

        }
        public override void Init()
        {
            LevelLoader.LoadLevel("Menu/StandardMenu");

            //Create the title text
            Vector2 convertedtitleTextPosition = LevelLoader.GridPointToWorld(titleTextPosition);
            titleTextObject = new TextGameObject(titleText, convertedtitleTextPosition, Vector2.One / 2, Color.Black, "Fonts/Title");
            gameObjectList.Add(titleTextObject);

            //Converting the gridvalues to in screen values
            Point convertedResumeButtonPosition = LevelLoader.GridPointToWorld(restartButtonPosition).ToPoint();
            Point convertedLevelSelectButtonPosition = LevelLoader.GridPointToWorld(levelSelectButtonPosition).ToPoint();
            Point convertedButtonSize = LevelLoader.GridPointToWorld(buttonSize).ToPoint();
            
            //resume button
            Rectangle resumeButtonBox = new Rectangle(convertedResumeButtonPosition, convertedButtonSize);
            restartButton = new Button(resumeButtonBox, restartButtonAssetName, restartButtonText);
            gameObjectList.Add(restartButton);

            //level select button
            Rectangle levelSelectButtonBox = new Rectangle(convertedLevelSelectButtonPosition, convertedButtonSize);
            levelSelectButton = new Button(levelSelectButtonBox, levelSelectButtonAssetName, levelSelectButtonText);
            gameObjectList.Add(levelSelectButton);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            LevelLoader.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }
        public override void HandleInput(InputHelper inputHelper)
        {
            //Switch to the corresponding gameState when a button is pressed
            if (levelSelectButton.clicked)
            {
                GameEnvironment.SwitchTo("LevelSelectState");
            } else if (restartButton.clicked)
            {
                GameEnvironment.SwitchTo("PlayingState");
            }
            base.HandleInput(inputHelper);
        }
    }
}
