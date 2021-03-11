using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace BaseProject
{
	class StartState : GameState
	{
		public StartState()
		{

		}

        public override void Init()
        {
            base.Init();
			gameObjectList.Add(new Bullet(Vector2.One));
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
