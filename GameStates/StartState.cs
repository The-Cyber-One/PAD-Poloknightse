using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace Poloknightse
{
	class StartState : GameState
	{
		public StartState()
		{

		}

        public override async void Init()
        {
            base.Init();
			
			Debug.WriteLine((await HighscoreManager.LoadScore()).ToString());
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
