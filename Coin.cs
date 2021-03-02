using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace BaseProject
{
    class Coin: GameObject
    {
        public int score;
        public Coin() : base("Coin/bronze_coin")
        {

        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (GameEnvironment.KeyboardState.IsKeyDown(Keys.Space))
            {
                score++;
                Debug.WriteLine(score);
            }
        }
        public void Reset()
        {
            score = 0;
        }
    }
}
