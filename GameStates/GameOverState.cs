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

		//Back button
		Point buttonPosition = new Point(28, 14);
		Point buttonSize = new Point(8, 8);
		string backButtonAssetName = "Back";
		string backButtonText = "Level Select Menu";
		Button backButton;

		public GameOverState()
		{
		}

		public override void Init()
		{
			base.Init();
			LevelLoader.LoadLevel("Menu/GameOver");
			gameObjectList.Add(new TextGameObject("Game Over", new Vector2(GameEnvironment.Screen.X / 2, GameEnvironment.Screen.Y / 2 - TITLE_Y_OFFSET), Vector2.One / 2, Color.Red, "Fonts/Title"));

			Point convertedButtonPosition = LevelLoader.GridPointToWorld(buttonPosition).ToPoint();
			Point convertedButtonSize = LevelLoader.GridPointToWorld(buttonSize).ToPoint();

			Rectangle button = new Rectangle(convertedButtonPosition, convertedButtonSize);
			backButton = new Button(button, backButtonAssetName, backButtonText);
			gameObjectList.Add(backButton);
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			LevelLoader.Draw(spriteBatch);
			base.Draw(spriteBatch);
		}

		public override void HandleInput(InputHelper inputHelper)
		{
			if (backButton.clicked)
			{
				GameEnvironment.SwitchTo("LevelSelectState");
			}
			base.HandleInput(inputHelper);
		}
	}
}
