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

		public override void Draw(SpriteBatch spriteBatch)
        {
			LevelLoader.Draw(spriteBatch);
		}
	}
}