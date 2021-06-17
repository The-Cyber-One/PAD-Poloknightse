using Microsoft.Xna.Framework;

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

        public PlayerFollower(Point gridPosition) : base(gridPosition, sprites[GameEnvironment.Random.Next(sprites.Length)])
        {
        }
    }
}
