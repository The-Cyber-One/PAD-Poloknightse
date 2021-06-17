using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Poloknightse
{
    class CreditsState : GameState
    {
        //Back button
        Point buttonPosition = new Point(28, 25);
        Point buttonSize = new Point(8, 8);
        string backButtonAssetName = "Back";
        string backButtonText = "Main menu";
        Button creditsStateButton;
        public CreditsState() : base()
        {

        }

        public override void Init()
        {
            LevelLoader.LoadLevel("Menu/CreditsMenu");

            gameObjectList.Add(new TextGameObject("Poloknightse\nBy Stinky Koala's", new Vector2(GameEnvironment.Screen.X / 2, GameEnvironment.Screen.Y / 10 * 4), Vector2.One / 2, Color.Wheat, "Fonts/Title"));
            gameObjectList.Add(new TextGameObject("Mees Dekker, Robin de Graaff, Joshua Knaven, Saad Zetouny, Martijn Zwart", new Vector2(GameEnvironment.Screen.X / 2, GameEnvironment.Screen.Y / 10 * 6), Vector2.One / 2, Color.Wheat, "Fonts/Title", 0.7f));
            
            //Back button
            Point convertedButtonPosition = LevelLoader.GridPointToWorld(buttonPosition).ToPoint();
            Point convertedButtonSize = LevelLoader.GridPointToWorld(buttonSize).ToPoint();
            Rectangle button = new Rectangle(convertedButtonPosition, convertedButtonSize);
            creditsStateButton = new Button(button, backButtonAssetName, backButtonText);
            gameObjectList.Add(creditsStateButton);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            LevelLoader.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            //Switch back to the main menu when the back button is clicked
            if (creditsStateButton.clicked)
            {
                GameEnvironment.SwitchTo("StartState");
            }
            base.HandleInput(inputHelper);
        }
    }
}
