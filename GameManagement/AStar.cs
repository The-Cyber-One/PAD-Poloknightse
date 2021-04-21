using Microsoft.Xna.Framework;
using Priority_Queue;
using System;
using System.Collections.Generic;
using System.Text;

namespace Poloknightse
{
    public static class AStar
    {
        private static List<Point> ReconstructPath(Point[] cameFrom, Point current)
        {
            List<Point> totalPath = new List<Point>();
            totalPath.Add(current);

            while (cameFrom[CoordsToIndex(current)] != (-Vector2.One).ToPoint())
            {
                current = cameFrom[CoordsToIndex(current)];
                totalPath.Add(current);
            }
            return totalPath;
        }

        private static float defaultH(Point pointA, Point pointB) => Vector2.Distance(pointA.ToVector2(), pointB.ToVector2());

        private static Point[] GetNeighbors(Point center)
        {
            List<Point> neighbors = new List<Point>();
            if (center.X != LevelLoader.grid.GetLength(0) - 1 && LevelLoader.grid[center.X + 1, center.Y].tileType == Tile.TileType.GROUND)
            {
                neighbors.Add(new Point(center.X + 1, center.Y));
            }
            if (center.Y != LevelLoader.grid.GetLength(1) - 1 && LevelLoader.grid[center.X, center.Y + 1].tileType == Tile.TileType.GROUND)
            {
                neighbors.Add(new Point(center.X, center.Y + 1));
            }
            if (center.X != 0 && LevelLoader.grid[center.X - 1, center.Y].tileType == Tile.TileType.GROUND)
            {
                neighbors.Add(new Point(center.X - 1, center.Y));
            }
            if (center.Y != 0 && LevelLoader.grid[center.X, center.Y - 1].tileType == Tile.TileType.GROUND)
            {
                neighbors.Add(new Point(center.X, center.Y - 1));
            }

            return neighbors.ToArray();
        }

        private static int CoordsToIndex(Point point) => point.X + LevelLoader.grid.GetLength(0) * point.Y;

        // A* finds a path from start to goal.
        // h is the heuristic function. h(n) estimates the cost to reach goal from node n.
        public static Point[] FindPath(Point start, Point goal, Func<Point, Point, float> h = null)
        {
            //Set defaultH function if it's null
            if (h is null) h = defaultH;

            // The set of discovered nodes that may need to be (re-)expanded.
            // Initially, only the start node is known.
            // This is usually implemented as a min-heap or priority queue rather than a hash-set.
            SimplePriorityQueue<Point> openSet = new SimplePriorityQueue<Point>();
            openSet.Enqueue(start, 0);

            // For node n, cameFrom[n] is the node immediately preceding it on the cheapest path from start
            // to n currently known.
            Point[] cameFrom = new Point[LevelLoader.grid.GetLength(0) * LevelLoader.grid.GetLength(1)];

            // For node n, gScore[n] is the cost of the cheapest path from start to n currently known.
            float[] gScore = new float[LevelLoader.grid.GetLength(0) * LevelLoader.grid.GetLength(1)];

            // For node n, fScore[n] := gScore[n] + h(n). fScore[n] represents our current best guess as to
            // how short a path from start to finish can be if it goes through n.
            float[] fScore = new float[LevelLoader.grid.GetLength(0) * LevelLoader.grid.GetLength(1)];
            for (int i = 0; i < gScore.Length; i++)
            {
                cameFrom[i] = (-Vector2.One).ToPoint();
                gScore[i] = float.PositiveInfinity;
                fScore[i] = float.PositiveInfinity;
            }
            gScore[CoordsToIndex(start)] = 0;
            fScore[CoordsToIndex(start)] = h(start, goal);

            while (openSet.Count > 0)
            {
                // This operation can occur in O(1) time if openSet is a min-heap or a priority queue
                Point current = openSet.First;
                if (current == goal)
                {
                    return ReconstructPath(cameFrom, current).ToArray();
                }
                openSet.Remove(current);
                foreach (Point neighbor in GetNeighbors(current))
                {
                    // d(current,neighbor) is the weight of the edge from current to neighbor
                    // tentative_gScore is the distance from start to the neighbor through current
                    float tentative_gScore = gScore[CoordsToIndex(current)];// + 1;
                    int neighborIndex = CoordsToIndex(neighbor);
                    if (tentative_gScore < gScore[neighborIndex])
                    {
                        // This path to neighbor is better than any previous one. Record it!
                        cameFrom[neighborIndex] = current;
                        gScore[neighborIndex] = tentative_gScore;
                        fScore[neighborIndex] = gScore[neighborIndex] + h(neighbor, goal);
                        if (!openSet.Contains(neighbor))
                            openSet.Enqueue(neighbor, 0);
                    }
                }
            }
            // Open set is empty but goal was never reached
            return null;
        }
    }
}
