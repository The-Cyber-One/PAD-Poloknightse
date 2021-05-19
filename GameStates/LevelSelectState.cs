using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Poloknightse
{
    class LevelSelectState : GameState
    {
        public LevelSelectState()
        {

        }

        public override void Init()
        {
            LevelLoader.LoadLevel("LevelSelectMenu");
            gameObjectList.Add(new TextGameObject("Select a level", new Vector2(GameEnvironment.Screen.X / 2, GameEnvironment.Screen.Y / 8), Vector2.One / 2, Color.Black, "Fonts/Title"));
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            LevelLoader.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }
        public override void HandleInput(InputHelper inputHelper)
        {
            if (inputHelper.MousePosition.X <= GameEnvironment.Screen.X/4)
            {
                GameEnvironment.SwitchTo("PlayingState");
            }
            base.HandleInput(inputHelper);
        }

        public override void Reset()
        {
            gameObjectList.Clear();
            base.Reset();
        }
    }
}
