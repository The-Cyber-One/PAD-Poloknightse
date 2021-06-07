using Microsoft.Xna.Framework;
using Priority_Queue;
using System;
using System.Collections.Generic;

namespace Poloknightse
{
    public static class AStar
    {
        /// <summary>
        /// Reconstruct a path from the collected A* data
        /// </summary>
        /// <param name="cameFrom">A 2d array of points where each point reffrences the previous</param>
        /// <param name="goal">The goal of the path finding</param>
        /// <returns>The reconstructed path created with A*</returns>
        private static List<Point> ReconstructPath(Point[] cameFrom, Point goal)
        {
            List<Point> totalPath = new List<Point>();
            totalPath.Add(goal);

            //Loop through all points backwards and add it to the path
            while (cameFrom[CoordsToIndex(goal)] != (-Vector2.One).ToPoint())
            {
                goal = cameFrom[CoordsToIndex(goal)];
                totalPath.Add(goal);
            }
            return totalPath;
        }

        /// <summary>
        /// Default way to calculate a heuristic value for a cell
        /// </summary>
        /// <param name="pointA">Point of a cell that needs a heuristic value</param>
        /// <param name="pointB">End point of the wanted path</param>
        private static float DefaultH(Point pointA, Point pointB) => Vector2.Distance(pointA.ToVector2(), pointB.ToVector2());

        /// <summary>
        /// Convert a grid position to a index
        /// </summary>
        /// <param name="point">The position in the grid</param>
        /// <returns>The index coresponding to the given coords</returns>
        private static int CoordsToIndex(Point point) => point.X + LevelLoader.grid.GetLength(0) * point.Y;

        /// <summary>
        /// Get the neigbours of a cell
        /// </summary>
        /// <param name="center">The center cell from where to look</param>
        /// <param name="walls">All cells to skip</param>
        /// <returns>The four neighbours of a cell</returns>
        private static Point[] GetNeighbours(Point center, Tile.TileType[,] walls = null)
        {
            List<Point> neighbors = new List<Point>();

            //Create a empty grid when no walls were given
            if (walls == null)
            {
                walls = new Tile.TileType[LevelLoader.grid.GetLength(0), LevelLoader.grid.GetLength(1)];
                for (int x = 0; x < LevelLoader.grid.GetLength(0); x++)
                {
                    for (int y = 0; y < LevelLoader.grid.GetLength(1); y++)
                    {
                        walls[x, y] = LevelLoader.grid[x, y].tileType;
                    }
                }
            }

            //Add neigbours to the list
            if (center.X != walls.GetLength(0) - 1 && walls[center.X + 1, center.Y] == Tile.TileType.GROUND)
            {
                neighbors.Add(new Point(center.X + 1, center.Y));
            }
            if (center.Y != walls.GetLength(1) - 1 && walls[center.X, center.Y + 1] == Tile.TileType.GROUND)
            {
                neighbors.Add(new Point(center.X, center.Y + 1));
            }
            if (center.X != 0 && walls[center.X - 1, center.Y] == Tile.TileType.GROUND)
            {
                neighbors.Add(new Point(center.X - 1, center.Y));
            }
            if (center.Y != 0 && walls[center.X, center.Y - 1] == Tile.TileType.GROUND)
            {
                neighbors.Add(new Point(center.X, center.Y - 1));
            }

            return neighbors.ToArray();
        }

        /// <summary>
        /// Find the shortest path using the A* algorithm
        /// </summary>
        /// <param name="start">Point to start the path at</param>
        /// <param name="goal">Point to end the path at</param>
        /// <param name="walls">Cells to walk around when looking</param>
        /// <param name="h">A function to calculate heuristic values</param>
        public static Point[] FindPath(Point start, Point goal, Tile.TileType[,] walls = null, Func<Point, Point, float> h = null)
        {
            //Set defaultH function if it's null
            if (h is null) h = DefaultH;

            //The set of discovered nodes
            SimplePriorityQueue<Point> openSet = new SimplePriorityQueue<Point>();
            openSet.Enqueue(start, 0);

            //For node n, cameFrom[n] is the node immediately preceding it on the cheapest path from start
            //To n currently known
            Point[] cameFrom = new Point[LevelLoader.grid.GetLength(0) * LevelLoader.grid.GetLength(1)];

            //For node n, gScore[n] is the cost of the cheapest path from start to n currently known
            float[] gScore = new float[LevelLoader.grid.GetLength(0) * LevelLoader.grid.GetLength(1)];

            //For node n, fScore[n] = gScore[n] + h(n). fScore[n] represents the current best guess for the path
            float[] fScore = new float[LevelLoader.grid.GetLength(0) * LevelLoader.grid.GetLength(1)];
            for (int i = 0; i < gScore.Length; i++)
            {
                cameFrom[i] = (-Vector2.One).ToPoint();
                gScore[i] = float.PositiveInfinity;
                fScore[i] = float.PositiveInfinity;
            }
            gScore[CoordsToIndex(start)] = 0;
            fScore[CoordsToIndex(start)] = h(start, goal);

            //Find the shortest path as long new cells were discovered  to look through
            while (openSet.Count > 0)
            {
                //This operation has a O(1) because it is a priority queue
                Point current = openSet.First;

                //If current is at the end a valid path is found
                if (current == goal)
                {
                    return ReconstructPath(cameFrom, current).ToArray();
                }

                //Remove current because it is being checked and should not be checked again
                openSet.Remove(current);

                //Calculate a score for all neighbours to find the best path and add it to open set
                foreach (Point neighbor in GetNeighbours(current, walls))
                {
                    //Tentative_gScore is the distance from start to the neighbor through current
                    float tentative_gScore = gScore[CoordsToIndex(current)];
                    int neighborIndex = CoordsToIndex(neighbor);

                    if (tentative_gScore < gScore[neighborIndex])
                    {
                        //This path to neighbor is better than any previous one
                        cameFrom[neighborIndex] = current;
                        gScore[neighborIndex] = tentative_gScore;
                        fScore[neighborIndex] = gScore[neighborIndex] + h(neighbor, goal);
                        if (!openSet.Contains(neighbor))
                            openSet.Enqueue(neighbor, 0);
                    }
                }
            }

            //Open set is empty but goal was never reached
            return null;
        }
    }
}
