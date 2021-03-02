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
            gameObjectList.Add(new Coin());
        }
        public override void Draw(SpriteBatch spriteBatch)
        {

            LevelLoader.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
