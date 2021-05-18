using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Poloknightse
{
	class GameOverState : GameState
	{
		private const int TITLE_Y_OFFSET = 200;

		public GameOverState()
		{
		}

		public override void Init()
		{
			base.Init();
			LevelLoader.LoadLevel("GameOver");
			gameObjectList.Add(new TextGameObject("Game Over", new Vector2(GameEnvironment.Screen.X / 2, GameEnvironment.Screen.Y / 2 - TITLE_Y_OFFSET), Vector2.One / 2, Color.Red, "Fonts/Title"));
			gameObjectList.Add(new TextGameObject("Press any button to go to the main menu", new Vector2(GameEnvironment.Screen.X / 2, GameEnvironment.Screen.Y / 2), Vector2.One / 2));
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			base.Draw(spriteBatch);
			LevelLoader.Draw(spriteBatch);
			base.Draw(spriteBatch);
		}

		public override void HandleInput(InputHelper inputHelper)
		{
			if (inputHelper.AnyKeyPressed)
			{
				GameEnvironment.SwitchTo("StartState");
			}
			base.HandleInput(inputHelper);
		}
	}
}
