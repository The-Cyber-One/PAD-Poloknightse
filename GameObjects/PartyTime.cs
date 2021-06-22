using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Poloknightse
{
    class PartyTime : GameObject
    {
        static public bool partyOn = false;

        public PartyTime(Point gridPosition = new Point ()) : base(gridPosition, "GameObjects/clover")
        {

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
