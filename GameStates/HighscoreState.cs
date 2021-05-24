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
        Table highscore;
        int currentHighscorePosition = 0, previousHighscorePosition;
        GameObjectList highscoreTable = new GameObjectList();
        int visableScoreAmount = 15;
        int startYPosition = 5;
        int rankStartPosition = 14, nameStartPosition = 17, scoreStartPosition = 29, levelStartPosition = 35, dateStartPosition = 39, timeStartPosition = 45;

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
        }

        protected async void GetHighscore()
        {
            highscore = await HighscoreManager.LoadScore();
            ConstructNewTable();
        }

        protected void ConstructNewTable()
        {
            highscoreTable.Clear();
            for (int i = 0; i < visableScoreAmount; i++)
            {
                if (currentHighscorePosition < 0 || currentHighscorePosition >= highscore.RowCount) break;

                GameObjectList row = new GameObjectList();
                row.Add(new TextGameObject((i + currentHighscorePosition).ToString(), LevelLoader.GridPointToWorld(new Point(rankStartPosition, i + startYPosition)))); ;
                row.Add(new TextGameObject(highscore.GetRow(i + currentHighscorePosition)[0].ToString(), LevelLoader.GridPointToWorld(new Point(nameStartPosition, i + startYPosition))));
                string[] textDateTime = highscore.GetRow(i + currentHighscorePosition)[1].ToString().Split(new char[]{ ' ', '/', ':'});
                DateTime dateTime = new DateTime(int.Parse(textDateTime[2]),
                    int.Parse(textDateTime[1]),
                    int.Parse(textDateTime[0]),
                    int.Parse(textDateTime[3]),
                    int.Parse(textDateTime[4]),
                    int.Parse(textDateTime[5]));
                dateTime = TimeZoneInfo.ConvertTime(dateTime, TimeZoneInfo.Local);
                row.Add(new TextGameObject($"{dateTime.Day}/{dateTime.Month}/{dateTime.Year}", LevelLoader.GridPointToWorld(new Point(dateStartPosition, i + startYPosition))));
                row.Add(new TextGameObject(dateTime.TimeOfDay.ToString(), LevelLoader.GridPointToWorld(new Point(timeStartPosition, i + startYPosition))));

                row.Add(new TextGameObject(highscore.GetRow(i + currentHighscorePosition)[2].ToString(), LevelLoader.GridPointToWorld(new Point(scoreStartPosition, i + startYPosition))));
                row.Add(new TextGameObject(highscore.GetRow(i + currentHighscorePosition)[3].ToString(), LevelLoader.GridPointToWorld(new Point(levelStartPosition, i + startYPosition))));

                highscoreTable.Add(row);
            }
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || inputHelper.KeyPressed(Keys.Escape))
            {
                GameEnvironment.SwitchTo("StartState");
            }

            if (highscore != null)
            {
                currentHighscorePosition += (int)inputHelper.FrameScrollWheelValue;
                if (currentHighscorePosition < 0) currentHighscorePosition = 0;
                if (currentHighscorePosition >= highscore.RowCount) currentHighscorePosition = highscore.RowCount - 1;
            }
            Debug.WriteLine(currentHighscorePosition);
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

            if (highscore == null)
            {
                //Draw loading symbol
            }
            else
            {
                //Draw highscore
                highscoreTable.Draw(spriteBatch);
            }
        }
    }
}
