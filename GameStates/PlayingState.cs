using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;
using System;


namespace Poloknightse
{
    class PlayingState : GameState
    {
        public GameObjectList players = new GameObjectList();
        private static Stopwatch stopWatch;
        private TimeSpan timeSpan;
        private int CoinAmount;
        private TextGameObject timerText, coinsLeftText;
        private int playerAmountEnd;
        private int followerAmountEnd;
        private string elapsedTime;
        private int totalEndTime;
        private Texture2D pixel;

        public PlayingState()
        {
            Game1.currentLevel = 1;
        }

        public override void Init()
        {
            players.Clear();
            LevelLoader.LoadLevel("Levels/" + Game1.levels[Game1.currentLevel]);
            gameObjectList.Add(players);

            //Count how many coins there are in the level
            for (int i = gameObjectList.Count - 1; i >= 0; i--)
            {
                if (gameObjectList[i] is Coin)
                {
                    CoinAmount += 1;
                }
            }

            //Start the timer for score
            stopWatch = new Stopwatch();
            stopWatch.Start();

            //UI components
            gameObjectList.Add(new TextGameObject("Time:", new Vector2(GameEnvironment.Screen.X - 1420, GameEnvironment.Screen.Y / 2 - 350), Vector2.One / 2, Color.White, "Fonts/Title"));
            timerText = new TextGameObject("0", new Vector2(GameEnvironment.Screen.X - 1460, GameEnvironment.Screen.Y / 2 - 280), Vector2.One / 2, Color.White, "Fonts/Title", 0.6f);
            gameObjectList.Add(timerText);

            gameObjectList.Add(new TextGameObject("Coins Left:", new Vector2(GameEnvironment.Screen.X - 1420, GameEnvironment.Screen.Y / 2 - 130), Vector2.One / 2, Color.White, "Fonts/Title"));
            coinsLeftText = new TextGameObject("0", new Vector2(GameEnvironment.Screen.X - 1425, GameEnvironment.Screen.Y / 2 - 60), Vector2.One / 2, Color.White, "Fonts/Title", 0.6f);
            gameObjectList.Add(coinsLeftText);

            gameObjectList.Add(new TextGameObject("Put cam here", new Vector2(1425, 200), Vector2.One / 2, Color.White, "Fonts/Title", 0.5f));
        }

        private void CreatePixel(SpriteBatch spriteBatch)
        {
            pixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            pixel.SetData(new[] { Color.White });
        }

        //This function will calculate the end score after the run is finished
        public void CalculateEndTime()
		{
            //stop the timer
            stopWatch.Stop();

            //Count how many players and followers there are in the level
			for (int i = 0; i < players.Children.Count; i++)
			{
                playerAmountEnd += 1;

                for (int j = ((Player)players.Children[i]).followers.Count - 1; j >= 0; j--)
                {
                    followerAmountEnd += 1;
                }
            }

            //Subtract the time earned by how many players and followers are left in the level
            int timeSpanTotalSec = (int)timeSpan.TotalSeconds;
            timeSpanTotalSec -= 2 * followerAmountEnd;
            timeSpanTotalSec -= 5 * playerAmountEnd;

            //Calculate it from total seconds to a value we can use in the database
            int sec = timeSpanTotalSec % 60;
            int min = (int)Math.Round(timeSpanTotalSec / 60d ,0) * 100;
            totalEndTime = min + sec;
		}

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (pixel == null) CreatePixel(spriteBatch);
            spriteBatch.Draw(pixel, new Rectangle(1275, 50, 300, 300), Color.White);
            spriteBatch.Draw(pixel, new Rectangle(1285, 60, 280, 280), Color.Black);

            LevelLoader.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            //Update the text of the stopwatch
            timeSpan = stopWatch.Elapsed;
            elapsedTime = String.Format("{0:00}:{1:00}", timeSpan.Minutes, timeSpan.Seconds);
            timerText.text = elapsedTime;

            //Update the text for the amount of coins
            coinsLeftText.text = CoinAmount.ToString();

            if (players.Children.Count == 1)
            {
                (players.Children[0] as Player).chosen = true;
            }
            base.Update(gameTime);

            //Check if all coins got picked up, go to next level when all are picked-up
            if (CoinAmount <= 0)
            {
                CalculateEndTime();

                Game1.currentLevel++;
                if (Game1.currentLevel >= Game1.levels.Length) Game1.currentLevel = 0;
                GameEnvironment.SwitchTo("WinState");
            }

            //Collision detection
            for (int i = gameObjectList.Count - 1; i >= 0; i--)
            {
                for (int j = players.Children.Count - 1; j >= 0; j--)
                {
                    if(i >= gameObjectList.Count)
					{
                        i = gameObjectList.Count - 1;
					}

                    Player player = (Player)players.Children[j];
                    //Coin -> Player collision
                    if (gameObjectList[i] is Coin)
                    {
                        if (player.CheckCollision(gameObjectList[i]))
                        {
                            gameObjectList.Remove(gameObjectList[i]);
                            CoinAmount -= 1;
                            continue;
                        }
                    }

                    //HealthPickup -> Player collision
                    if (gameObjectList[i] is HealthPickup)
                    {
                        if (player.CheckCollision(gameObjectList[i]))
                        {
                            gameObjectList.Remove(gameObjectList[i]);
                            player.AddFollower(gameTime);
                            continue;
                        }
                    }

                    //Bullet -> Player collsion
                    if (gameObjectList[i] is Bullet)
                    {
                        if (player.CheckCollision(gameObjectList[i]))
                        {
                            player.TakeDamage(gameObjectList[i].gridPosition, gameTime);
                            gameObjectList.Remove(gameObjectList[i]);
                            continue;
                        }
                    }
                }
            }
        }

        public override void HandleInput(InputHelper inputHelper)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || inputHelper.KeyPressed(Keys.Escape) || inputHelper.KeyPressed(Keys.Back))
			{
				GameEnvironment.SwitchTo("LevelSelectState");
			}

			foreach (GameObject gameObject in gameObjectList)
			{
				if (gameObject != players) gameObject.HandleInput(inputHelper);
			}

            if (inputHelper.KeyPressed(Keys.E) || inputHelper.KeyPressed(Keys.Space))
            {
                FindingNewChosen();
            }
            for (int i = players.Children.Count - 1; i >= 0; i--)
            {
                Player player = players.Children[i] as Player;
                if (player.chosen)
                {
                    players.Children[i].HandleInput(inputHelper);
                }
            }
        }

		public void FindingNewChosen()
		{
			for (int i = players.Children.Count - 1; i >= 0; i--)
			{
				Player player = players.Children[i] as Player;
				if (player.chosen)
				{
					player.chosen = false;
					if (i + 1 >= players.Children.Count)
					{
						(players.Children[0] as Player).chosen = true;
						break;
					}
					else
					{
						(players.Children[i + 1] as Player).chosen = true;
						break;
					}
					
				}
			}
		}
	}
}
