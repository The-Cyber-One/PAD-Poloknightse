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
        private List<GameObject> followers = new List<GameObject>();
        private MouseState lastMouseState;
        private MouseState currentMouseState;
        private bool clickOccurred;

        public Player() : base("Player/Onderbroek_ridder")
        {
            velocity.X = 1;
            velocity.Y = 1;
            movementDirection = 0;
            gridPosition = Vector2.One;
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            KeyHandling();

            foreach (GameObject follower in followers)
            {
                follower.Update(gameTime);
            }

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
            //Add follower
            if (clickOccurred)
            {
                GameObject newFollower = new GameObject("Player/Helm_ridder");
                followers.Add(newFollower);
                newFollower.gridPosition = gridPosition;
                newFollower.Update(gameTime);

                Debug.WriteLine(followers.Count);
                clickOccurred = false;
            }

            //Shift all the elements of the followers array 1 spot
            for (int i = followers.Count - 1; i > 0; i--)
            {
                followers[i].gridPosition = followers[i - 1].gridPosition;
            }
            if (followers.Count > 0)
                followers[0].gridPosition = gridPosition;

            //Move player
            gridPosition += velocity;
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
