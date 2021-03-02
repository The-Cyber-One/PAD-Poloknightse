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
        private List<GameObject> followers = new List<GameObject>();
        private bool addFollower;

        public Player() : base("Player/Onderbroek_ridder")
        {
            velocity = Vector2.Zero;
            gridPosition = Vector2.One;
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            foreach (GameObject follower in followers)
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
                    GameObject newFollower = new GameObject("Player/Helm_ridder");
                    followers.Add(newFollower);
                    newFollower.gridPosition = gridPosition;
                    newFollower.Update(gameTime);

                    addFollower = false;
                }

                //Shift all the elements of the followers array 1 spot
                for (int i = followers.Count - 1; i > 0; i--)
                {
                    followers[i].gridPosition = followers[i - 1].gridPosition;
                }
                if (followers.Count > 0)
                    followers[0].gridPosition = gridPosition;
            }

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
    }
}
