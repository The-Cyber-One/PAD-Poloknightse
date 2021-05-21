using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        public PlayerFollower(Point gridPosition) : base(gridPosition, sprites[GameEnvironment.Random.Next(sprites.Length)])
        {
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            positionSize = new Rectangle(
            gridPosition.X * LevelLoader.gridTileSize + GameEnvironment.startGridPoint.X + (int)(LevelLoader.gridTileSize / 2),
                gridPosition.Y * LevelLoader.gridTileSize + GameEnvironment.startGridPoint.Y + (int)(LevelLoader.gridTileSize),
                (int)(texture.Width * LevelLoader.scalingFactor),
                (int)(texture.Height * LevelLoader.scalingFactor));
            spriteBatch.Draw(texture, positionSize, null, Color.White, 0, new Vector2(texture.Width / 2, texture.Height), SpriteEffects.None, 1);
        }
    }
}
