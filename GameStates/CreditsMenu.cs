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
