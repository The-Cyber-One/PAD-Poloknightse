using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BaseProject
{
    class Player : GameObject
    {
        private List<PlayerFollower> followers = new List<PlayerFollower>();
        private bool addFollower;

        public Player(Vector2 gridPosition) : base(gridPosition, "Player/Onderbroek_ridder")
        {
            velocity = Vector2.Zero;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            foreach (PlayerFollower follower in followers)
            {
                follower.Update(gameTime);
            }
        }

        public override void FixedUpdate(GameTime gameTime)
        {
            if (velocity != Vector2.Zero)
            {
                //Add follower
                if (addFollower)
                {
                    AddFollower(gameTime);
                    addFollower = false;
                }

                //Shift all the elements of the followers array 1 spot
                for (int i = followers.Count - 1; i > 0; i--)
                {
                    followers[i].gridPosition = followers[i - 1].gridPosition;
                }
                if (followers.Count > 0) followers[0].gridPosition = gridPosition;
            }

            //Move player
            gridPosition += velocity;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            foreach (PlayerFollower follower in followers)
            {
                follower.Draw(spriteBatch);
            }
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            //Change the movement direction
            if (inputHelper.KeyPressed(Keys.D))
            {
                velocity = Vector2.Zero;
                velocity.X = 1;
            }
            else if (inputHelper.KeyPressed(Keys.A))
            {
                velocity = Vector2.Zero;
                velocity.X = -1;
            }
            else if (inputHelper.KeyPressed(Keys.W))
            {
                velocity = Vector2.Zero;
                velocity.Y = -1;
            }
            else if (inputHelper.KeyPressed(Keys.S))
            {
                velocity = Vector2.Zero;
                velocity.Y = 1;
            }

            CollisionDetection.CheckWallCollision(this);

            if (inputHelper.MouseLeftButtonPressed())
            {
                addFollower = true;
            }
        }

        /// <summary>
        /// Split player at <paramref name="gridPosition"/>
        /// </summary>
        /// <param name="gridPosition">Position to take damage at</param>
        public void TakeDamage(Vector2 gridPosition)
        {
            //Code to split player in half
        }

        /// <summary>
        /// Add follower to player
        /// </summary>
        public void AddFollower(GameTime gameTime)
        {
            PlayerFollower newFollower = new PlayerFollower(gridPosition);
            followers.Add(newFollower);
            newFollower.Update(gameTime);
        }

        /// <summary>
        /// Add follower to player
        /// </summary>
        /// <param name="position">Start position for the new follower</param>
        public void AddFollower(GameTime gameTime, Vector2 position)
        {
            if (position == null)
            {
                position = gridPosition;
            }
            PlayerFollower newFollower = new PlayerFollower(position);
            followers.Add(newFollower);
            newFollower.Update(gameTime);
        }

        /// <summary>
        /// Load all the followers from given Dictionary <paramref name="positionFollowerPairs">
        /// </summary>
        /// <param name="positionFollowerPairs">Unsorted Dictionary with all de followers</param>
        public void LoadFollowers(Dictionary<Point, PlayerFollower> positionFollowerPairs)
        {
            List<Point> checkedNeighbours = new List<Point>();
            followers = SortByNeighbour(gridPosition.ToPoint(), positionFollowerPairs, checkedNeighbours);
        }

        /// <summary>
        /// Sort given Dictionary <paramref name="positionFollowerPairs"/>
        /// </summary>
        /// <param name="centerPosition">First position to start checking neighbours from</param>
        /// <param name="positionFollowerPairs">Dictionary with Vector2 position key and value of follower</param>
        /// <param name="checkedNeighbours">List with all positions that must be skiped over while checking neighbours</param>
        /// <returns>The sorted dictionary as a List</returns>
        private List<PlayerFollower> SortByNeighbour(Point centerPosition, Dictionary<Point, PlayerFollower> positionFollowerPairs, List<Point> checkedNeighbours)
        {
            checkedNeighbours.Add(centerPosition);
            List<PlayerFollower> sortedFollowers = new List<PlayerFollower>();

            //Check top neighbour
            Point checkingPosition = new Point(centerPosition.X, centerPosition.Y - 1);
            if (positionFollowerPairs.ContainsKey(checkingPosition) && !checkedNeighbours.Contains(checkingPosition))
            {
                sortedFollowers.Add(positionFollowerPairs[checkingPosition]);
                sortedFollowers.AddRange(SortByNeighbour(checkingPosition, positionFollowerPairs, checkedNeighbours));
            }

            //Check right neighbour
            checkingPosition = new Point(centerPosition.X + 1, centerPosition.Y);
            if (positionFollowerPairs.ContainsKey(checkingPosition) && !checkedNeighbours.Contains(checkingPosition))
            {
                sortedFollowers.Add(positionFollowerPairs[checkingPosition]);
                sortedFollowers.AddRange(SortByNeighbour(checkingPosition, positionFollowerPairs, checkedNeighbours));
            }

            //Check bottom neighbour
            checkingPosition = new Point(centerPosition.X, centerPosition.Y + 1);
            if (positionFollowerPairs.ContainsKey(checkingPosition) && !checkedNeighbours.Contains(checkingPosition))
            {
                sortedFollowers.Add(positionFollowerPairs[checkingPosition]);
                sortedFollowers.AddRange(SortByNeighbour(checkingPosition, positionFollowerPairs, checkedNeighbours));
            }

            //Check left neighbour
            checkingPosition = new Point(centerPosition.X - 1, centerPosition.Y);
            if (positionFollowerPairs.ContainsKey(checkingPosition) && !checkedNeighbours.Contains(checkingPosition))
            {
                sortedFollowers.Add(positionFollowerPairs[checkingPosition]);
                sortedFollowers.AddRange(SortByNeighbour(checkingPosition, positionFollowerPairs, checkedNeighbours));
            }

            return sortedFollowers;
        }
    }
}
