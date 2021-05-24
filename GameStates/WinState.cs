using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Poloknightse
{
	class WinState : GameState
	{
		TextGameObject textObject = new TextGameObject("", GameEnvironment.Screen.ToVector2() / 2, Vector2.One / 2, Color.White);

		public WinState()
		{
			gameObjectList.Add(textObject);
		}

        public override void Init()
        {
            base.Init();
			textObject.text = $"Level {Game1.currentLevel} completed! \n Press any button to continue";
		}

        public override void Draw(SpriteBatch spriteBatch)
		{
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
