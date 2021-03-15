using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Poloknightse
{
    class PlayerFollower : GameObject
    {
        private static string[] sprites = new string[]
        {
            "GameObjects/Player/Helm_ridder",
            "GameObjects/Player/Cape_ridder",
            "GameObjects/Player/Harnas_ridder"
        };

        public PlayerFollower(Vector2 gridPosition) : base(gridPosition, sprites[GameEnvironment.Random.Next(sprites.Length)])
        {
        }
    }
}
