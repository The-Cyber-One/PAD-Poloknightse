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
		// Variables for the title text
		Vector2 titleTextPosition = new Vector2(32, 10.5f);
		TextGameObject titleTextObject;
		string titleText = "Game Over";

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
			
			//Create the title text
			Vector2 convertedTitleTextPosition = LevelLoader.GridPointToWorld(titleTextPosition);
			titleTextObject = new TextGameObject(titleText, convertedTitleTextPosition, Vector2.One / 2, Color.Red, "Fonts/Title");
			gameObjectList.Add(titleTextObject);

			//Back button
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
			// Switch back to the level select menu when the back button is clicked
			if (backButton.clicked)
			{
				GameEnvironment.SwitchTo("LevelSelectState");
			}
			base.HandleInput(inputHelper);
		}
	}
}
