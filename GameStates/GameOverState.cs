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
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			base.Draw(spriteBatch);
		}

		public override void HandleInput(InputHelper inputHelper)
		{
			if (inputHelper.AnyKeyPressed)
			{
				Debug.WriteLine("going to start state");
				GameEnvironment.SwitchTo(GameEnvironment.GameStates.START_STATE);
			}
			base.HandleInput(inputHelper);
		}
	}
}
