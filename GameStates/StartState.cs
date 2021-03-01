using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace BaseProject
{
	class StartState : GameState
	{
		public StartState()
		{
			gameObjectList.Add(new Player());
		}

		public override void Draw(SpriteBatch spriteBatch)
        {
			LevelLoader.Draw(spriteBatch);
			base.Draw(spriteBatch);
		}
		public override void Update(GameTime gameTime)
        {
			base.Update(gameTime);
        }
	}
}