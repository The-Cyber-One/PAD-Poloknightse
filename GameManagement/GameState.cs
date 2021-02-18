using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace BaseProject
{
    class GameState
    {
        public List<GameObject> gameObjectList = new List<GameObject>();
        public void Init()
        {

        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (GameObject gameObject in gameObjectList)
                gameObject.Draw(spriteBatch);
        }
    }
}
