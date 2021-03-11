using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BaseProject
{
    class PlayingState : GameState
    {
        public Player player;

        public PlayingState()
        {
        }

        public override void Init()
        {
            LevelLoader.LoadLevel("test");
            gameObjectList.Add(new Coin(Vector2.One));
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            LevelLoader.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
            //Collision detection
            for (int i = gameObjectList.Count - 1; i >= 0; i--)
            {
                //Coin -> Player collision
                if (gameObjectList[i] is Coin)
                {
                    if (player.CheckCollision(gameObjectList[i]))
                    {
                        gameObjectList.Remove(gameObjectList[i]);
                    }
                }

                //Bullet -> Player collsion
                if (gameObjectList[i] is Bullet)
                {
                    if (player.CheckCollision(gameObjectList[i]))
                    {
                        gameObjectList.Remove(gameObjectList[i]);
                        player.TakeDamage(gameObjectList[i].gridPosition);
                    }
                }
            }
        }
    }
}
