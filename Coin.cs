using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace BaseProject
{
    class Coin: GameObject
    {
        public Coin() : base("Coin/bronze_coin")
        {

        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
