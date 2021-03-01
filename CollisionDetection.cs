using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace BaseProject
{
    class CollisionDetection
    {
      public void CheckWallCollision(Player playerObject)
      {
           if(playerObject.movementDirection == 3 && LevelLoader.tiles[(int)playerObject.position.X, (int)playerObject.position.Y - GameEnvironment.gridTileSize].tileType == Tile.TileType.WALL)
            {
                playerObject.velocity = Vector2.Zero;
            }

      }
    }
}
