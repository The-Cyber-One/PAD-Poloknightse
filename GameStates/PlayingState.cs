﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;


namespace Poloknightse
{
    class PlayingState : GameState
    {
        public GameObjectList players = new GameObjectList();
        private int CoinAmount;
        private float ElapsedTimeSec = 0;
        private float ElapsedTimeMin = 0;
        private TextGameObject timerText;

        public PlayingState()
        {
            Game1.currentLevel = 1;
        }

        public override void Init()
        {
            players.Clear();
            LevelLoader.LoadLevel(Game1.levels[Game1.currentLevel]);
            gameObjectList.Add(players);

            //Count how many coins there are in the level
            for (int i = gameObjectList.Count - 1; i >= 0; i--)
            {
                if (gameObjectList[i] is Coin)
                {
                    CoinAmount += 1;
                }
            }

            timerText = new TextGameObject("0", new Vector2(GameEnvironment.Screen.X / 4, GameEnvironment.Screen.Y / 2), Vector2.One / 2, Color.White);
            gameObjectList.Add(timerText);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            LevelLoader.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            ElapsedTimeSec += (float)gameTime.ElapsedGameTime.TotalSeconds;

			if (ElapsedTimeSec >= 60)
			{
                ElapsedTimeSec = 0;
                ElapsedTimeMin += 1;
			}

            timerText.text = ElapsedTimeSec.ToString("0");


            if (players.Children.Count == 1)
            {
                (players.Children[0] as Player).chosen = true;
            }
            base.Update(gameTime);

            //Check if all coins got picked up
            if (CoinAmount <= 0)
            {
                Game1.currentLevel++;
                if (Game1.currentLevel >= Game1.levels.Length) Game1.currentLevel = 0;
                GameEnvironment.SwitchTo("WinState");
            }

            //Collision detection
            for (int i = gameObjectList.Count - 1; i >= 0; i--)
            {
                for (int j = players.Children.Count - 1; j >= 0; j--)
                {
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
            Debug.WriteLine(ElapsedTimeMin +" , "+  (int)ElapsedTimeSec);
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            foreach (GameObject gameObject in gameObjectList)
            {
                if (gameObject != players) gameObject.HandleInput(inputHelper);
            }

            for (int i = players.Children.Count - 1; i >= 0; i--)
            {
                Player player = players.Children[i] as Player;
                if (player.chosen)
                {
                    players.Children[i].HandleInput(inputHelper);

                    if (inputHelper.KeyPressed(Keys.E) || inputHelper.KeyPressed(Keys.Space))
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
}
