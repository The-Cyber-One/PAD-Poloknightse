using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Poloknightse
{
	class WinState : GameState
	{
		// Variables for the title text
		Vector2 titleTextPosition = new Vector2(32, 12f);
		Vector2 titleTextPositionCongratulations = new Vector2(32, 9f);

		//Back button
		Point buttonPosition = new Point(28, 14);
		Point buttonSize = new Point(8, 8);
		string backButtonAssetName = "Back";
		string backButtonText = "Level select menu";
		Button winStateButton;

		public WinState()
		{
			
		}

        public override void Init()
        {
			LevelLoader.LoadLevel("Menu/StandardMenu");

			//Create the title text
			Vector2 convertedtitleTextPosition = LevelLoader.GridPointToWorld(titleTextPosition);
			Vector2 convertedtitleTextCongratulationsPosition = LevelLoader.GridPointToWorld(titleTextPositionCongratulations);
			gameObjectList.Add(new TextGameObject("Congratulations!", convertedtitleTextCongratulationsPosition, Vector2.One / 2, Color.Black, "Fonts/Title"));
			gameObjectList.Add(new TextGameObject("you completed the level in: " + PlayingState.timeSpanTotalSec + " seconds!", convertedtitleTextPosition, Vector2.One / 2, Color.Black, "Fonts/Title", 0.5f));

			//Back button
			Point convertedButtonPosition = LevelLoader.GridPointToWorld(buttonPosition).ToPoint();
			Point convertedButtonSize = LevelLoader.GridPointToWorld(buttonSize).ToPoint();
			Rectangle button = new Rectangle(convertedButtonPosition, convertedButtonSize);
			winStateButton = new Button(button, backButtonAssetName, backButtonText);
			gameObjectList.Add(winStateButton);
		}

        public override void Draw(SpriteBatch spriteBatch)
		{
			LevelLoader.Draw(spriteBatch);
			base.Draw(spriteBatch);
		}

		public override void HandleInput(InputHelper inputHelper)
		{
			//Switch to the level select state if the button is pressed
			if (winStateButton.clicked)
			{
				GameEnvironment.SwitchTo("LevelSelectState");
			}
			base.HandleInput(inputHelper);
		}
	}
}
