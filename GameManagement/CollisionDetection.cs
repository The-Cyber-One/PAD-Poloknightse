using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Poloknightse
{
    class CollisionDetection
    {
        public static bool CheckWallCollision(Point gridPosition, Vector2 velocity)
        {
            //Check above player
            if (velocity.Y == -1 && LevelLoader.grid[gridPosition.X, gridPosition.Y - 1].tileType == Tile.TileType.WALL)
            {
                return true;
            }

            //check below player
            if (velocity.Y == 1 && LevelLoader.grid[gridPosition.X, gridPosition.Y + 1].tileType == Tile.TileType.WALL)
            {
                return true;
            }

            //Check right of player
            if (velocity.X == 1 && LevelLoader.grid[gridPosition.X + 1, gridPosition.Y].tileType == Tile.TileType.WALL)
            {
                return true;
            }

            //Check left of player
            if (velocity.X == -1 && LevelLoader.grid[gridPosition.X - 1, gridPosition.Y].tileType == Tile.TileType.WALL)
            {
                return true;
            }
            return false;
        }

        public static bool ObjectWillHitOther(GameObject gameObjectA, GameObject gameObjectB)
        {
            return gameObjectA.gridPosition + gameObjectA.velocity.ToPoint() == gameObjectB.gridPosition + gameObjectB.velocity.ToPoint();
        }
    }
}
