using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace BaseProject
{
    class PlayerFollower : GameObject
    {
        public PlayerFollower(Vector2 gridPosition) : base(gridPosition, "GameObjects/Player/Helm_ridder")
        {

        }
    }
}
