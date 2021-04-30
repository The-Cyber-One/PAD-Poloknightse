using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;

namespace Poloknightse
{
    class PlayingState : GameState
    {
        public GameObjectList players = new GameObjectList(Vector2.Zero.ToPoint());
        private int CoinAmount;

        public PlayingState()
        {
        }

        public override void Init()
        {
            LevelLoader.LoadLevel("Level-5");
            gameObjectList.Add(players);
            //Count how many coins there are in the level
            for (int i = gameObjectList.Count - 1; i >= 0; i--)
            {
                if (gameObjectList[i] is Coin)
                {
                    CoinAmount += 1;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            LevelLoader.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }

        public static void ChangeToGameOverState()
        {
            Debug.WriteLine("going to game over state");
            GameEnvironment.SwitchTo(GameEnvironment.GameStates.GAME_OVER_STATE);
        }

        public static void ChangeToWinState()
        {
            Debug.WriteLine("going to win state");
            GameEnvironment.SwitchTo(GameEnvironment.GameStates.WIN_STATE);
        }

        public override void Update(GameTime gameTime)
        {
            if (players.Children.Count == 1)
            {
                players.Children[0].chosen = true;
            }
            base.Update(gameTime);

            //Check if all coins got picked up
            if (CoinAmount <= 0)
            {
                ChangeToWinState();
            }

            //Collision detection
            for (int i = gameObjectList.Count - 1; i >= 0; i--)
            {
                for (int j = 0; j < players.Children.Count; j++)
                {
                    Player player = players.Children[j] as Player;
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
            foreach (GameObject gameObject in gameObjectList)
            {
                if (!(gameObject is Player)) gameObject.HandleInput(inputHelper);
            }

            for (int i = 0; i < players.Children.Count; i++)
            {
                if (players.Children[i].chosen)
                {
                    players.Children[i].HandleInput(inputHelper);
                    if (inputHelper.KeyPressed(Keys.E) || inputHelper.KeyPressed(Keys.Space))
                    {
                        players.Children[i].chosen = false;
                        players.Children[i].velocity = Vector2.Zero;
                        if (i + 1 >= players.Children.Count)
                        {
                            players.Children[i].chosen = true;
                        }
                        else
                        {
                            players.Children[i + 1].chosen = true;
                        }
                    }
                }
            }
        }
    }
}
