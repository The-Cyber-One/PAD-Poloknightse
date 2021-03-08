using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BaseProject
{
    class PlayingState : GameState
    {
        Player player = new Player(Vector2.One);

        public PlayingState()
        {
        }

        public override void Init()
        {
            LevelLoader.LoadLevel("test");
            gameObjectList.Add(player);
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
        }
    }
}
