using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Poloknightse
{
    class Player : GameObject
    {
        public bool chosen = false;

        private List<PlayerFollower> followers = new List<PlayerFollower>();
        private Point newFollowerPosition;
        private Vector2 nextVelocity = new Vector2();

        private const int MIN_FOLLOWERS = 3;
        private readonly GameObject
            arrowLeft = new GameObject(null, "GameObjects/Player/DirectionIcon/DirectionIconLeft"),
            arrowUp = new GameObject(null, "GameObjects/Player/DirectionIcon/DirectionIconUp"),
            arrowRight = new GameObject(null, "GameObjects/Player/DirectionIcon/DirectionIconRight"),
            arrowDown = new GameObject(null, "GameObjects/Player/DirectionIcon/DirectionIconDown"),
            arrowsSelectedLeft = new GameObject(null, "GameObjects/Player/DirectionIconSelected/DirectionIconSelectedLeft"),
            arrowsSelectedUp = new GameObject(null, "GameObjects/Player/DirectionIconSelected/DirectionIconSelectedUp"),
            arrowsSelectedRight = new GameObject(null, "GameObjects/Player/DirectionIconSelected/DirectionIconSelectedRight"),
            arrowsSelectedDown = new GameObject(null, "GameObjects/Player/DirectionIconSelected/DirectionIconSelectedDown");

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
            //Check if player can activate stored velocity
            if (!CollisionDetection.CheckWallCollision(gridPosition, nextVelocity) && !CheckFollowerCollsion((gridPosition.ToVector2() + nextVelocity).ToPoint()))
            {
                velocity = nextVelocity;
            }

            if (CheckFollowerCollsion(gridPosition) || CollisionDetection.CheckWallCollision(gridPosition, velocity))
            {
                velocity = Vector2.Zero;
            }

            if (velocity != Vector2.Zero)
            {
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
            gridPosition += velocity.ToPoint();

            //Update arrow position
            arrowLeft.gridPosition = new Point(gridPosition.X - 1, gridPosition.Y);
            arrowUp.gridPosition = new Point(gridPosition.X, gridPosition.Y - 1);
            arrowRight.gridPosition = new Point(gridPosition.X + 1, gridPosition.Y);
            arrowDown.gridPosition = new Point(gridPosition.X, gridPosition.Y + 1);

            arrowsSelectedLeft.gridPosition = new Point(gridPosition.X - 1, gridPosition.Y);
            arrowsSelectedUp.gridPosition = new Point(gridPosition.X, gridPosition.Y - 1);
            arrowsSelectedRight.gridPosition = new Point(gridPosition.X + 1, gridPosition.Y);
            arrowsSelectedDown.gridPosition = new Point(gridPosition.X, gridPosition.Y + 1);
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            //Change the movement direction
            if (inputHelper.KeyPressed(Keys.D) || inputHelper.KeyPressed(Keys.Right))
            {
                nextVelocity = Vector2.Zero;
                nextVelocity.X = 1;
            }
            else if (inputHelper.KeyPressed(Keys.A) || inputHelper.KeyPressed(Keys.Left))
            {
                nextVelocity = Vector2.Zero;
                nextVelocity.X = -1;
            }
            else if (inputHelper.KeyPressed(Keys.W) || inputHelper.KeyPressed(Keys.Up))
            {
                nextVelocity = Vector2.Zero;
                nextVelocity.Y = -1;
            }
            else if (inputHelper.KeyPressed(Keys.S) || inputHelper.KeyPressed(Keys.Down))
            {
                nextVelocity = Vector2.Zero;
                nextVelocity.Y = 1;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            foreach (PlayerFollower follower in followers)
            {
                follower.Draw(spriteBatch);
            }
            
            //Draw arrow
            if (chosen)
            {
                if (nextVelocity == new Vector2(-1, 0))
                {
                    arrowsSelectedLeft.Draw(spriteBatch);
                }
                else if (nextVelocity == new Vector2(0, -1))
                {
                    arrowsSelectedUp.Draw(spriteBatch);
                }
                else if (nextVelocity == new Vector2(1, 0))
                {
                    arrowsSelectedRight.Draw(spriteBatch);
                }
                else if (nextVelocity == new Vector2(0, 1))
                {
                    arrowsSelectedDown.Draw(spriteBatch);
                }
            }
            else
            {
                if (nextVelocity == new Vector2(-1, 0))
                {
                    arrowLeft.Draw(spriteBatch);
                }
                else if (nextVelocity == new Vector2(0, -1))
                {
                    arrowUp.Draw(spriteBatch);
                }
                else if (nextVelocity == new Vector2(1, 0))
                {
                    arrowRight.Draw(spriteBatch);
                }
                else if (nextVelocity == new Vector2(0, 1))
                {
                    arrowDown.Draw(spriteBatch);
                }
            }
        }

        /// <summary>
        /// Checks if <paramref name="gridPosition"></paramref> will hit a follower
        /// </summary>
        private bool CheckFollowerCollsion(Point gridPosition)
        {
            bool playerHitsFollower = false;
            foreach (PlayerFollower playerFollower in followers)
            {
                if (playerFollower.gridPosition == gridPosition)
                {
                    return true;
                }
            }
            return playerHitsFollower;
        }

        /// <summary>
        /// Split player at <paramref name="gridPosition"/>
        /// </summary>
        /// <param name="gridPosition">Position to take damage at</param>
        public void TakeDamage(Point gridPosition, GameTime gameTime)
        {
            GameObjectList players = GameEnvironment.GetState<PlayingState>("PlayingState").players;

            //Check if player is dead
            if (followers.Count <= MIN_FOLLOWERS || !chosen || gridPosition == this.gridPosition)
            {
                players.Remove(this);

				if (chosen)
				{
                    GameEnvironment.GetState<PlayingState>("PlayingState").FindingNewChosen();
                }

                //Check if GameOver
                if (players.Children.Count == 0)
                {
                    GameEnvironment.GetState<PlayingState>("PlayingState").CalculateEndTime();
                    GameEnvironment.SwitchTo("GameOverState");
                }
                return;
            }

            //Code to split player in half
            //TODO ALS HOOFD WORD GERAAKT FIX DE SPLIT
            Player player = new Player(followers[followers.Count - 1].gridPosition);
            players.Add(player);
            for (int i = followers.Count - 1; i >= 0; i--)
            {
                if (followers[i].gridPosition == gridPosition)
                {
                    for (int j = followers.Count - 1; j >= i; j--)
                    {
                        player.AddFollower(gameTime, followers[j].gridPosition);
                        followers.RemoveAt(j);
                    }
                    if (followers.Count != 0)
                    {
                        followers.RemoveAt(followers.Count - 1);
                    }
                    else
                    {
                        player.followers.RemoveAt(player.followers.Count - 1);
                    }
                    player.followers.RemoveAt(0);
                    Debug.WriteLine("added other playable player");
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
        /// Checks if <paramref name="gameObject"></paramref> collides with any part of the player
        /// </summary>
        public override bool CheckCollision(GameObject gameObject)
        {
            bool playerHitsObject = base.CheckCollision(gameObject);

            return playerHitsObject || CheckFollowerCollsion(gameObject.gridPosition);
        }

        /// <summary>
        /// Find the center of the player with followers
        /// </summary>
        public Point GetCenter()
        {
            if (followers.Count == 0) return gridPosition;
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
