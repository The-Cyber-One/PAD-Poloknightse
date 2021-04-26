using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;

namespace Poloknightse
{
    class PlayingState : GameState
    {
        public List<Player> players = new List<Player>();
        private int CoinAmount;

        public PlayingState()
        {
        }

        public override void Init()
        {
            LevelLoader.LoadLevel("Level-3");

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
            base.Update(gameTime);

            //Check if all coins got picked up
            if (CoinAmount <= 0)
			{
                ChangeToWinState();
			}

            //Collision detection
            for (int i = gameObjectList.Count - 1; i >= 0; i--)
            {
                foreach (Player player in players)
                {
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
    }
}
