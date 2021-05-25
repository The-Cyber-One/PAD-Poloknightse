using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace Poloknightse
{
    class NameState : GameState
    {
        TextGameObject name;
        Point buttonPosition = new Point(30, 23);
        Point buttonSize = new Point(4, 4);
        string backButtonAssetName = "Start";
        string backButtonText = "Start Game";
        Button winStateButton;
        Vector2 infoPostion = new Vector2(32.5f, 10f), namePosition = new Vector2(18f, 18f), warningPosition = new Vector2(32.5f, 17f);
        TextGameObject warningText;

        public NameState() : base()
        {

        }

        public override void Init()
        {
            LevelLoader.LoadLevel("Menu/StandardMenu");
            name = new TextGameObject("COOL GUY", LevelLoader.GridPointToWorld(namePosition), Vector2.Zero, Color.Black, "Fonts/Title");
            gameObjectList.Add(new TextGameObject("What is your name?", LevelLoader.GridPointToWorld(infoPostion), Vector2.One / 2, Color.Black, "Fonts/Title"));
            gameObjectList.Add(name);

            Point convertedButtonPosition = LevelLoader.GridPointToWorld(buttonPosition).ToPoint();
            Point convertedButtonSize = LevelLoader.GridPointToWorld(buttonSize).ToPoint();
            Rectangle button = new Rectangle(convertedButtonPosition, convertedButtonSize);
            winStateButton = new Button(button, backButtonAssetName, backButtonText);
            gameObjectList.Add(winStateButton);

            warningText = new TextGameObject("This name already exists", LevelLoader.GridPointToWorld(warningPosition), Vector2.One/2, Color.Red);
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            base.HandleInput(inputHelper);

            if (winStateButton.clicked && !HighscoreManager.PlayerNameExists(name.text) && name.text != "")
            {
                GameEnvironment.PlayerName = name.text;
                HighscoreManager.SavePlayer(GameEnvironment.PlayerName);
                GameEnvironment.SwitchTo("StartState");
            }

            if (inputHelper.AnyKeyPressed)
            {
                Keys[] keys = inputHelper.CurrentKeyboardState.GetPressedKeys();
                bool upperCase = false;

                foreach (Keys key in keys)
                {
                    if (key == Keys.LeftShift || key == Keys.RightShift) upperCase = true;
                }

                foreach (Keys key in keys)
                {
                    if (key == Keys.Back && name.text.Length > 0)
                    {
                        name.text = name.text.Remove(name.text.Length - 1);
                    }
                    else if ((int)key > 64 && (int)key < 91 && name.text.Length < 20)
                    {
                        if (upperCase)
                        {
                            name.text += key.ToString();
                        }
                        else
                        {
                            name.text += key.ToString();
                        }
                    }
                    else if (key == Keys.Space && name.text.Length < 20)
                    {
                        name.text += " ";
                    }
                }

                //Update warning text
                if (HighscoreManager.PlayerNameExists(name.text))
                {
                    warningText.text = "This name already exists";
                }
                else if (name.text == "")
                {
                    warningText.text = " Name can not be empty";
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            LevelLoader.Draw(spriteBatch);
            base.Draw(spriteBatch);

            if (HighscoreManager.PlayerNameExists(name.text) || name.text == "")
            {
                warningText.Draw(spriteBatch);
            }
        }
    }
}
