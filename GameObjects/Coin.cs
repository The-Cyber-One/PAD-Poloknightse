using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Poloknightse
{
    class Coin: GameObject
    {
        public Coin(Point gridPosition = new Point()) : base(gridPosition, "GameObjects/bronze_coin")
        {

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
