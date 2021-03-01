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
        int movementDirection;
        
        public int totalFollowers;
        List<Rectangle> followers = new List<Rectangle>();
        bool addFollower;
        public Player() : base("Player/Onderbroek_Ridder")
        {
            velocity.X = 1;
            velocity.Y = 1;
            movementDirection = 0;
            totalFollowers = 0;
            addFollower = false;
        }
        int test = 0;

        MouseState lastMouseState;
        MouseState currentMouseState;
        bool clickOccurred;
        public override void Update(GameTime gameTime)
        {
            //Shift all the elements of the followers array 1 spot
            if (totalFollowers == followers.Count)
            {
                for (int i = 0; i < followers.Count - 1; i++)
                {
                    followers[i] = followers[i + 1];
                }
            }
            if (totalFollowers > 0) 
            {
                followers[totalFollowers - 1] = new Rectangle((int)position.X * GameEnvironment.gridTileSize,
                (int)position.Y * GameEnvironment.gridTileSize, GameEnvironment.gridTileSize, GameEnvironment.gridTileSize);
            }
            Move(gameTime);
            base.Update(gameTime);
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
             if (clickOccurred && !addFollower)
            {
                totalFollowers++;
                followers.Add(new Rectangle((int)position.X * GameEnvironment.gridTileSize,
                (int)position.Y * GameEnvironment.gridTileSize, GameEnvironment.gridTileSize, GameEnvironment.gridTileSize));
                
                Debug.WriteLine(test++);
                clickOccurred = false;
                addFollower = true;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            for (int i = 0; i < followers.Count; i++)
            {
                spriteBatch.Draw(texture, followers[i], Color.White);
            }
        }

        /// <summary>
        /// Decide where the player is going to move according to the keys pressed
        /// </summary>
        public void Move(GameTime gameTime)
        {
            //Change the movement direction
            if (GameEnvironment.KeyboardState.IsKeyDown(Keys.D))
            {
                movementDirection = 1;
            }
            else if (GameEnvironment.KeyboardState.IsKeyDown(Keys.A))
            {
                movementDirection = 2;
            }
            else if (GameEnvironment.KeyboardState.IsKeyDown(Keys.W))
            {
                movementDirection = 3;
            }
            else if (GameEnvironment.KeyboardState.IsKeyDown(Keys.S))
            {
                movementDirection = 4;
            }

            /* Depending on the movement direction, let the player move a certain direction. 
                These two are seperate if statements in order te create movement where the player keeps moving, even when he releases the button */
            if (movementDirection == 1)
            {
                position.X += velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
                addFollower = false;
            }
            else if (movementDirection == 2)
            {
                position.X -= velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
                addFollower = false;
            }
            else if (movementDirection == 3)
            {
                position.Y -= velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;
                addFollower = false;
            }
            else if (movementDirection == 4)
            {
                position.Y += velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;
                addFollower = false;
            }
        }
    }
}
