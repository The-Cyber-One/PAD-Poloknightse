using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Poloknightse
{
    class EnemyWalking : GameObject
    {
        public EnemyWalking(Vector2 gridPosition) : base(gridPosition, "GameObjects/Player/Koning")
        {

        }
    }
}
