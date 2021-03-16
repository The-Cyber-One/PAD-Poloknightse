using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Poloknightse
{
	class StartState : GameState
	{
		public StartState()
		{

		}

        public override void Init()
        {
            base.Init();
			HighscoreManager.LoadScore();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
			base.Draw(spriteBatch);
		}

        public override void HandleInput(InputHelper inputHelper)
        {	
			if (inputHelper.AnyKeyPressed)
			{
				GameEnvironment.SwitchTo(GameEnvironment.GameStates.PLAYING_STATE);
			}
			base.HandleInput(inputHelper);
		}
	}
}
