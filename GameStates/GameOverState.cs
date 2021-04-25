using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Poloknightse
{
	class GameOverState : GameState
	{
		public GameOverState()
		{
			Debug.WriteLine("GAME_OVER");
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			base.Draw(spriteBatch);
		}

		public override void HandleInput(InputHelper inputHelper)
		{
			if (inputHelper.AnyKeyPressed)
			{
				GameEnvironment.SwitchTo(GameEnvironment.GameStates.START_STATE);
			}
			base.HandleInput(inputHelper);
		}
	}
}
