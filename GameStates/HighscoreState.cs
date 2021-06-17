using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Poloknightse
{
    class HighscoreState : GameState
    {
        Table dbHighscore;
        string latest;
        int currentHighscorePosition = 0, previousHighscorePosition;
        GameObjectList highscoreTable = new GameObjectList();
        TextGameObject EmptyHighscoreText;
        int visableScoreAmount = 28;
        int startYPosition = 5;
        int rankStartPosition = 14, nameStartPosition = 17, scoreStartPosition = 29, levelStartPosition = 35, dateStartPosition = 39, timeStartPosition = 45;
        Color normalTextColor = new Color(100, 100, 100), playerColor = Color.Black, mostRecentColor = new Color(87, 142, 24);

        //Back button
        Point buttonPosition = new Point(3, 22);
        Point buttonSize = new Point(6, 6);
        string backButtonAssetName = "Back";
        string backButtonText = "Main menu";
        Button backButton;

        public HighscoreState() : base()
        {

        }

        public override void Init()
        {
            LevelLoader.LoadLevel("Menu/HighscoreMenu");
            gameObjectList.Add(new TextGameObject("Rank", LevelLoader.GridPointToWorld(new Point(rankStartPosition, startYPosition - 2))));
            gameObjectList.Add(new TextGameObject("Name", LevelLoader.GridPointToWorld(new Point(nameStartPosition, startYPosition - 2))));
            gameObjectList.Add(new TextGameObject("Score", LevelLoader.GridPointToWorld(new Point(scoreStartPosition, startYPosition - 2))));
            gameObjectList.Add(new TextGameObject("Level", LevelLoader.GridPointToWorld(new Point(levelStartPosition, startYPosition - 2))));
            gameObjectList.Add(new TextGameObject("Date", LevelLoader.GridPointToWorld(new Point(dateStartPosition, startYPosition - 2))));
            gameObjectList.Add(new TextGameObject("Time", LevelLoader.GridPointToWorld(new Point(timeStartPosition, startYPosition - 2))));
            GetHighscore();
            EmptyHighscoreText = new TextGameObject("Well this game seems popular. \nThere seems to be no highscore, maybe try again?", LevelLoader.GridPointToWorld(new Point(rankStartPosition, startYPosition)));

            //Back button
            Point convertedButtonPosition = LevelLoader.GridPointToWorld(buttonPosition).ToPoint();
            Point convertedButtonSize = LevelLoader.GridPointToWorld(buttonSize).ToPoint();
            Rectangle button = new Rectangle(convertedButtonPosition, convertedButtonSize);
            backButton = new Button(button, backButtonAssetName, backButtonText);
            gameObjectList.Add(backButton);
        }

        protected async void GetHighscore()
        {
            dbHighscore = await HighscoreManager.LoadScore();

            //Find latest recieved score of player
            Table dbLatest = await HighscoreManager.LoadRecent();
            for (int i = 0; i < dbLatest.RowCount; i++)
            {
                if (dbLatest.GetRow(i)[0].ToString() == GameEnvironment.PlayerName)
                {
                    latest = dbLatest.GetRow(i)[1].ToString();
                    break;
                }
            }

            for (int i = 0; i < dbHighscore.RowCount; i++)
            {
                if (dbHighscore.GetRow(i)[0].ToString() == GameEnvironment.PlayerName && dbHighscore.GetRow(i)[1].ToString() == latest)
                {
                    currentHighscorePosition = i - 1;
                    break;
                }
            }

            //Create TextGameObject table
            ConstructNewTable();
        }

        protected void ConstructNewTable()
        {
            highscoreTable.Clear();
            for (int i = 0; i < visableScoreAmount; i++)
            {
                if (currentHighscorePosition < 0 || i + currentHighscorePosition >= dbHighscore.RowCount) break;

                //Determain the right color for the row
                Color rowColor = normalTextColor;
                if (dbHighscore.GetRow(i + currentHighscorePosition)[0].ToString() == GameEnvironment.PlayerName)
                {
                    if (dbHighscore.GetRow(i + currentHighscorePosition)[1].ToString() == latest)
                    {
                        rowColor = mostRecentColor;
                    }
                    else
                    {
                        rowColor = playerColor;
                    }
                }

                GameObjectList row = new GameObjectList();

                //Fill row with items
                row.Add(new TextGameObject((i + currentHighscorePosition).ToString(), LevelLoader.GridPointToWorld(new Point(rankStartPosition, i + startYPosition)), Vector2.Zero, rowColor));
                row.Add(new TextGameObject(dbHighscore.GetRow(i + currentHighscorePosition)[0].ToString(), LevelLoader.GridPointToWorld(new Point(nameStartPosition, i + startYPosition)), Vector2.Zero, rowColor));

                DateTime dateTime = (DateTime)Convert.ChangeType(dbHighscore.GetRow(i + currentHighscorePosition)[1], typeof(DateTime));
                dateTime = TimeZoneInfo.ConvertTime(dateTime, TimeZoneInfo.Utc, TimeZoneInfo.Local);
                row.Add(new TextGameObject($"{dateTime.Day}/{dateTime.Month}/{dateTime.Year}", LevelLoader.GridPointToWorld(new Point(dateStartPosition, i + startYPosition)), Vector2.Zero, rowColor));
                row.Add(new TextGameObject(dateTime.TimeOfDay.ToString(), LevelLoader.GridPointToWorld(new Point(timeStartPosition, i + startYPosition)), Vector2.Zero, rowColor));

                row.Add(new TextGameObject(dbHighscore.GetRow(i + currentHighscorePosition)[2].ToString(), LevelLoader.GridPointToWorld(new Point(scoreStartPosition, i + startYPosition)), Vector2.Zero, rowColor));
                row.Add(new TextGameObject(dbHighscore.GetRow(i + currentHighscorePosition)[3].ToString(), LevelLoader.GridPointToWorld(new Point(levelStartPosition, i + startYPosition)), Vector2.Zero, rowColor));

                highscoreTable.Add(row);
            }
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || inputHelper.KeyPressed(Keys.Escape) || inputHelper.KeyPressed(Keys.Back))
            {
                GameEnvironment.SwitchTo("StartState");
            }

            if (dbHighscore != null)
            {
                currentHighscorePosition += (int)inputHelper.FrameScrollWheelValue;
                if (currentHighscorePosition < 0) currentHighscorePosition = 0;
                if (currentHighscorePosition >= dbHighscore.RowCount) currentHighscorePosition = dbHighscore.RowCount - 1;
            }
           
            //Switch to the main menu if the back button is pressed
            if (backButton.clicked)
            {
                GameEnvironment.SwitchTo("StartState");
            }

            base.HandleInput(inputHelper);

            base.HandleInput(inputHelper);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (currentHighscorePosition != previousHighscorePosition)
            {
                ConstructNewTable();
            }
            previousHighscorePosition = currentHighscorePosition;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            LevelLoader.Draw(spriteBatch);
            base.Draw(spriteBatch);

            if (dbHighscore == null)
            {
                //Draw loading symbol
            }
            else if (dbHighscore.RowCount == 0)
            {
                EmptyHighscoreText.Draw(spriteBatch);
            }
            else
            {
                //Draw highscore
                highscoreTable.Draw(spriteBatch);
            }
        }
    }
}
