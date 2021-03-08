using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BaseProject
{
    class PlayingState : GameState
    {
        Player player = new Player();

        public PlayingState()
        {
        }

        public override void Init()
        {
            gameObjectList.Add(player);
            gameObjectList.Add(new Coin(new Vector2(5,5)));
            gameObjectList.Add(new Coin(new Vector2(7, 5)));
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
