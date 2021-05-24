using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Poloknightse
{
    class CreditsMenu : GameState
    {
        public CreditsMenu()
        {

        }
        public override void Init()
        {
            LevelLoader.LoadLevel("CreditsMenu");
            gameObjectList.Add(new TextGameObject("Poloknightse\nBy Stinky Koala's", new Vector2(GameEnvironment.Screen.X / 2, GameEnvironment.Screen.Y / 2), Vector2.One / 2, Color.Wheat, "Fonts/Title"));
            gameObjectList.Add(new TextGameObject("Mees Dekker, Joshua Knaven, Robin de Graaff, Saad Zetouny, Martijn Zwart", new Vector2(GameEnvironment.Screen.X / 2, GameEnvironment.Screen.Y * 2 / 3), Vector2.One / 2, Color.Wheat, "Fonts/Title", 0.7f));
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            LevelLoader.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }
        public override void HandleInput(InputHelper inputHelper)
        {
            if (inputHelper.AnyKeyPressed)
            {
                GameEnvironment.SwitchTo("StartState");
            }
            base.HandleInput(inputHelper);
        }
    }
}
