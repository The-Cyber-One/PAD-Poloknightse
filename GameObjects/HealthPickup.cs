﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Poloknightse
{
    class HealthPickup : GameObject
    {
        public HealthPickup(Point gridPosition = new Point()) : base(gridPosition, "GameObjects/HealthPickup")
        {

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
