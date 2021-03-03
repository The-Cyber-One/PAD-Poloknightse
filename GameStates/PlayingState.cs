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
            foreach(GameObject gameobject in gameObjectList)
            {
                if(gameobject is Coin)
                {
                    if (player.CheckCollision(gameobject))
                    {
                        gameObjectRemovedList.Add(gameobject);
                    }
                }
            }
            foreach(GameObject gameObject in gameObjectRemovedList)
            {
                gameObjectList.Remove(gameObject);
            }
        }
    }
}
