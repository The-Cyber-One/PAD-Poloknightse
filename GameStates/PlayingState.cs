using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BaseProject
{
    class PlayingState : GameState
    {
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
            //removes coin when player is in contact
            for (int i = gameObjectList.Count - 1; i >= 0; i--)
            {
                if (gameObjectList[i] is Coin)
                {
                    if (player.CheckCollision(gameObjectList[i]))
                    {
                        gameObjectList.Remove(gameObjectList[i]);
                    }
                }
            }
        }
    }
}
