using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace BaseProject
{
    class GameState
    {
        public List<GameObject> gameObjectList = new List<GameObject>();
        public List<GameObject> gameObjectRemovedList = new List<GameObject>();
        public double updateTimer;
        public const int tickTimeLength = 1;
        public virtual void Init()
        {

        }

        public virtual void HandleInput(InputHelper inputHelper)
        {
            foreach (GameObject gameObject in gameObjectList)
            {
                gameObject.HandleInput(inputHelper);
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach (GameObject gameObject in gameObjectList)
            {
                gameObject.Update(gameTime);
                if (updateTimer >= tickTimeLength)
                {
                    gameObject.FixedUpdate(gameTime);
                }
            }
            if (updateTimer >= tickTimeLength)
            {
                updateTimer = 0;
            }
            updateTimer += gameTime.ElapsedGameTime.TotalSeconds;
        }

        public virtual void FixedUpdate(GameTime gameTime)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            foreach (GameObject gameObject in gameObjectList)
                gameObject.Draw(spriteBatch);
        }
    }
}
