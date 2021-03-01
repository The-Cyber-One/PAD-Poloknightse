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
        public int movementDirection;
        public int totalFollowers;
        private List<GameObject> followers = new List<GameObject>();
        private bool addFollower;
        private MouseState lastMouseState;
        private MouseState currentMouseState;
        private bool clickOccurred;

        public Player() : base("Player/Onderbroek_ridder")
        {
            velocity.X = 1;
            velocity.Y = 1;
            movementDirection = 0;
            totalFollowers = 0;
            addFollower = false;
            gridPosition = Vector2.One;
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            KeyHandling();
            // The active state from the last frame is now old
            lastMouseState = currentMouseState;

            // Get the mouse state relevant for this frame
            currentMouseState = Mouse.GetState();

            // Recognize a single click of the left mouse button
            if (lastMouseState.LeftButton == ButtonState.Released && currentMouseState.LeftButton == ButtonState.Pressed)
            {
                // React to the click
                // ...
                clickOccurred = true;

            }
        }
        public override void FixedUpdate(GameTime gameTime)
        {
            Move();

            //Shift all the elements of the followers array 1 spot
            for (int i = 0; i < followers.Count - 1; i++)
            {
                followers[i].gridPosition = followers[i + 1].gridPosition;
            }
            if (followers.Count > 0)
            {
                followers[totalFollowers - 1].gridPosition = gridPosition;
            }
            if (clickOccurred && !addFollower)
            {
                totalFollowers++;
                GameObject newFollower = new GameObject("Player/Helm_ridder");
                followers.Add(newFollower);
                newFollower.gridPosition = gridPosition;

                Debug.WriteLine(followers.Count);
                clickOccurred = false;
                addFollower = true;
            }

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            foreach (GameObject follower in followers)
            {
                follower.Draw(spriteBatch);
            }
        }

        /// <summary>
        /// Decide where the player is going to move according to the keys pressed
        /// </summary>
        public void Move()
        {
            /* Depending on the movement direction, let the player move a certain direction. 
                These two are seperate if statements in order te create movement where the player keeps moving, even when he releases the button */
            gridPosition += velocity;
        }
        public void KeyHandling()
        {
            //Change the movement direction
            if (GameEnvironment.KeyboardState.IsKeyDown(Keys.D))
            {
                velocity = Vector2.Zero;
                velocity.X = 1;
            }
            else if (GameEnvironment.KeyboardState.IsKeyDown(Keys.A))
            {
                velocity = Vector2.Zero;
                velocity.X = -1;
            }
            else if (GameEnvironment.KeyboardState.IsKeyDown(Keys.W))
            {
                velocity = Vector2.Zero;
                velocity.Y = -1;
            }
            else if (GameEnvironment.KeyboardState.IsKeyDown(Keys.S))
            {
                velocity = Vector2.Zero;
                velocity.Y = 1;
            }

            CollisionDetection.CheckWallCollision(this);
        }
    }
}
