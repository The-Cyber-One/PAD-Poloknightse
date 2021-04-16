using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Poloknightse
{
    class EnemyGhost : GameObject
    {
        public EnemyGhost(Point gridPosition) : base(gridPosition, "GameObjects/EnemyGhost")
        {

        }

    }
}
