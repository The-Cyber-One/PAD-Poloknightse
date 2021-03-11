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
        public Coin(Vector2 gridPosition) : base(gridPosition, "GameObjects/bronze_coin")
        {

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
