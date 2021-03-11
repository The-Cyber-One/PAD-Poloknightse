using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace BaseProject
{
    class Bullet : GameObject
    {
        public Bullet(Vector2 gridPosition) : base(gridPosition, "GameObjects/bomb")
        {

        }
    }
}
