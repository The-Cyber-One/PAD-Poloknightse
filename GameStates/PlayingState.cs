using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;

namespace Poloknightse
{
    class PlayingState : GameState
    {
        public GameObjectList players = new GameObjectList();
        private int CoinAmount;

        public PlayingState()
        {
            Game1.currentLevel = 0;
        }

        public override void Reset()
        {
            base.Reset();
            players.Clear();
            //Init();
        }

        public override void Init()
        {
            gameObjectList.Clear();

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
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            LevelLoader.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //Check if all coins got picked up, go to next level when all are picked-up
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
        }
    }
}
