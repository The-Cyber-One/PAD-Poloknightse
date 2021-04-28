using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Poloknightse
{
    class Player : GameObject
    {
        private List<PlayerFollower> followers = new List<PlayerFollower>();
        private bool addFollower;
        private Point newFollowerPosition;
        int minFollowers = 3;

        public Player(Point gridPosition) : base(gridPosition, "GameObjects/Player/Onderbroek_ridder")
        {
            velocity = Vector2.Zero;
            newFollowerPosition = gridPosition;
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

                //Update position of posible new follower
                if (followers.Count > 0)
                {

                    newFollowerPosition = followers[followers.Count - 1].gridPosition;
                }
                else
                {
                    newFollowerPosition = gridPosition;
                }

                //Shift all the elements of the followers array 1 spot
                for (int i = followers.Count - 1; i > 0; i--)
                {
                    followers[i].gridPosition = followers[i - 1].gridPosition;
                }
                if (followers.Count > 0) followers[0].gridPosition = gridPosition;
            }

            //Move player
            gridPosition += velocity.ToPoint(); ;
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
            CheckPlayerCollsion();

            if (inputHelper.MouseLeftButtonPressed())
            {
                addFollower = true;
            }
        }

        /// <summary>
        /// Checks if player will hit it self and if so stop moving
        /// </summary>
        public void CheckPlayerCollsion()
        {
            bool playerHitsPlayer = false;
            foreach (PlayerFollower playerFollower in followers)
            {
                if (playerFollower.gridPosition == (gridPosition.ToVector2() + velocity).ToPoint())
                {
                    playerHitsPlayer = true;
                }
            }
            if (playerHitsPlayer) velocity = Vector2.Zero;
        }


        /// <summary>
        /// Split player at <paramref name="gridPosition"/>
        /// </summary>
        /// <param name="gridPosition">Position to take damage at</param>
        public void TakeDamage(Point gridPosition, GameTime gameTime)
        {
            //Code to split player in half
            if (followers.Count <= minFollowers)
            {
                GameEnvironment.CurrentGameState.gameObjectList.Remove(this);
                PlayingState.ChangeToGameOverState();
                return;
            }
            Player player = new Player(followers[followers.Count - 1].gridPosition);
            PlayingState.playersList.Add(player);
            for (int i = followers.Count - 1; i >= 0; i--)
            {
                Debug.WriteLine("added other playable player");
                if (followers[i].gridPosition == gridPosition)
                {
                    for(int j = followers.Count - 1; j >= i; j--)
                    {
                        player.AddFollower(gameTime, followers[j].gridPosition);
                        followers.RemoveAt(j);
                    }
                    followers.RemoveAt(followers.Count - 1);
                    player.followers.RemoveAt(0);
                    break;
                }
            }
        }

        /// <summary>
        /// Add follower to player
        /// </summary>
        public void AddFollower(GameTime gameTime)
        {
            PlayerFollower newFollower = new PlayerFollower(newFollowerPosition);
            followers.Add(newFollower);
            newFollower.Update(gameTime);
        }

        /// <summary>
        /// Add follower to player
        /// </summary>
        /// <param name="position">Start position for the new follower</param>
        public void AddFollower(GameTime gameTime, Point position)
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
            followers = SortByNeighbour(gridPosition, positionFollowerPairs, checkedNeighbours);
        }

        /// <summary>
        /// Find the center of the player with followers
        /// </summary>
        public Point GetCenter()
        {
            return followers[followers.Count / 2].gridPosition;
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
