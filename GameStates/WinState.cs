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
			Debug.WriteLine("WIN");
			gameObjectList.Add(textObject);
		}

        public override void Init()
        {
            base.Init();
			textObject.text = "press any button to load level " + (Game1.currentLevel + 1);
		}

        public override void Draw(SpriteBatch spriteBatch)
		{
			base.Draw(spriteBatch);
		}

		public override void HandleInput(InputHelper inputHelper)
		{
			if (inputHelper.AnyKeyPressed)
			{
				GameEnvironment.SwitchTo("PlayingState");
			}
			base.HandleInput(inputHelper);
		}
	}
}
