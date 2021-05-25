using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Poloknightse
{
    class CreditsMenu : GameState
    {
        public CreditsMenu() : base()
        {

        }

        public override void Init()
        {
            LevelLoader.LoadLevel("Menu/CreditsMenu");
            gameObjectList.Add(new TextGameObject("Poloknightse\nBy Stinky Koala's", new Vector2(GameEnvironment.Screen.X / 2, GameEnvironment.Screen.Y / 2), Vector2.One / 2, Color.Wheat, "Fonts/Title"));
            gameObjectList.Add(new TextGameObject("Mees Dekker, Robin de Graaff, Joshua Knaven, Saad Zetouny, Martijn Zwart", new Vector2(GameEnvironment.Screen.X / 2, GameEnvironment.Screen.Y * 2 / 3), Vector2.One / 2, Color.Wheat, "Fonts/Title", 0.7f));
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            LevelLoader.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || inputHelper.KeyPressed(Keys.Escape) || inputHelper.KeyPressed(Keys.Back))
            {
                GameEnvironment.SwitchTo("StartState");
            }
            base.HandleInput(inputHelper);
        }
    }
}
