using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using C5;

namespace Poloknightse
{
    class EnemyWalking : GameObject
    {
        int stamina = 5;
        int maxPatrolDistance = 3;
        Point startPosition;
        int patrolDistance;

        State currentState;

        enum State
        {
            Patrol,
            Chase,
            Return,
            Crying
        }

        public EnemyWalking(Point gridPosition) : base(gridPosition, "GameObjects/Player/Koning")
        {
            startPosition = gridPosition;
            SetState(State.Patrol);
        }

        public override void FixedUpdate(GameTime gameTime)
        {
            base.FixedUpdate(gameTime);

            if (!canMove())
            {
                SetState(State.Crying);
            }

            switch (currentState)
            {
                case State.Patrol:
                    {
                        if (patrolDistance <= 0)
                        {
                            SetState(State.Return);
                            break;
                        }
                        patrolDistance--;

                        Point direction = getRandomDirection();
                        while (LevelLoader.grid[gridPosition.X + direction.X, gridPosition.Y + direction.Y].tileType == Tile.TileType.WALL)
                        {
                            direction = getRandomDirection();
                        }
                        gridPosition += direction;
                        break;
                    }
                case State.Chase:
                    break;
                case State.Return:
                    break;
            }
        }

        private Point getRandomDirection()
        {
            if (GameEnvironment.Random.Next(2) == 0)
            {
                return new Point(GameEnvironment.Random.Next(2) * 2 - 1, 0);
            }
            else
            {
                return new Point(0, GameEnvironment.Random.Next(2) * 2 - 1);
            }
        }

        private bool canMove()
        {
            return LevelLoader.grid[gridPosition.X + 1, gridPosition.Y].tileType == Tile.TileType.GROUND ||
                LevelLoader.grid[gridPosition.X - 1, gridPosition.Y].tileType == Tile.TileType.GROUND ||
                LevelLoader.grid[gridPosition.X, gridPosition.Y + 1].tileType == Tile.TileType.GROUND ||
                LevelLoader.grid[gridPosition.X, gridPosition.Y - 1].tileType == Tile.TileType.GROUND;
        }

        private void SetState(State newState)
        {
            currentState = newState;
            patrolDistance = maxPatrolDistance;
        }

        private List<Point> ReconstructPath(Point[] cameFrom, Point current)
        {
            List<Point> totalPath = new List<Point>();
            totalPath.Add(current);

            while (current in cameFrom.Keys){
            current:= cameFrom[current];
                totalPath.Add(current);
                return totalPath;
            }
        }

        private float h(Point pointA, Point pointB) => Vector2.Distance(pointA.ToVector2(), pointB.ToVector2());

        // A* finds a path from start to goal.
        // h is the heuristic function. h(n) estimates the cost to reach goal from node n.
        private Point[] A_Star(Point start, Point goal, Func<float> h)
        {
            // The set of discovered nodes that may need to be (re-)expanded.
            // Initially, only the start node is known.
            // This is usually implemented as a min-heap or priority queue rather than a hash-set.
            IntervalHeap<Point> openSet = new IntervalHeap<Point>();
            openSet.Add(start);

            // For node n, cameFrom[n] is the node immediately preceding it on the cheapest path from start
            // to n currently known.
            List<Point> cameFrom = new List<Point>();

            // For node n, gScore[n] is the cost of the cheapest path from start to n currently known.
            float[] gScore = new float[LevelLoader.grid.GetLength(0) * LevelLoader.grid.GetLength(1)];
            for (int i = 0; i < gScore.Length; i++)
            {
                gScore[i] = float.PositiveInfinity;
            }
            gScore[start.X + LevelLoader.grid.GetLength(0) * start.Y] = 0;

            // For node n, fScore[n] := gScore[n] + h(n). fScore[n] represents our current best guess as to
            // how short a path from start to finish can be if it goes through n.
            float[] fScore = new float[LevelLoader.grid.GetLength(0) * LevelLoader.grid.GetLength(1)];
            for (int i = 0; i < fScore.Length; i++)
            {
                fScore[i] = float.PositiveInfinity;
            }
            fScore[start.X + LevelLoader.grid.GetLength(0) * start.Y] = h();

            while (openSet.Count > 0)
            {
                // This operation can occur in O(1) time if openSet is a min-heap or a priority queue
                Point current = openSet.FindMin();
                if (current == goal)
                {
                    return ReconstructPath(cameFrom, current);
                }

                    openSet.Remove(current)
                    for each neighbor of current
                        // d(current,neighbor) is the weight of the edge from current to neighbor
                        // tentative_gScore is the distance from start to the neighbor through current
                        tentative_gScore := gScore[current] + d(current, neighbor)
                        if tentative_gScore < gScore[neighbor]
                            // This path to neighbor is better than any previous one. Record it!
                            cameFrom[neighbor] := current
                            gScore[neighbor] := tentative_gScore
                            fScore[neighbor] := gScore[neighbor] + h(neighbor)
                            if neighbor not in openSet
                                openSet.add(neighbor)
            }
            // Open set is empty but goal was never reached
            SetState(State.Crying);
            return null;
        }
    }
}
