using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace BaseProject
{
    class Bullet : GameObject
    {
        const float SPEED = 1;

        public Bullet(Vector2 gridPosition, Vector2 direction) : base(gridPosition, "GameObjects/bomb")
        {
            direction.Normalize();
            velocity = direction * SPEED;
        }
    }
}
