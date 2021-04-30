using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Poloknightse
{
    class StartState : GameState
    {
        GameObjectList backgroundObjects = new GameObjectList();
        List<Point> secretPlaces = new List<Point>();

        public StartState()
        {

        }

        public override async void Init()
        {
            LevelLoader.LoadLevel("MainMenu");
            AddSecret(new Coin());
            AddSecret(new HealthPickup());
            AddSecret(new EnemyGhost());
            AddSecret(new EnemyWalking());

            Debug.WriteLine((await HighscoreManager.LoadScore()).ToString());
        }

        private void AddSecret(GameObject gameObject)
        {
            do
            {
                gameObject.gridPosition = new Point(GameEnvironment.Random.Next(1, LevelLoader.grid.GetLength(0) - 2), GameEnvironment.Random.Next(1, LevelLoader.grid.GetLength(1) - 2));
            }
            while (!gameObject.IsInLevel || secretPlaces.Contains(gameObject.gridPosition) || LevelLoader.grid[gameObject.gridPosition.X, gameObject.gridPosition.Y].tileType == Tile.TileType.WALL);

            secretPlaces.Add(gameObject.gridPosition);
            backgroundObjects.Add(gameObject);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            LevelLoader.Draw(spriteBatch);
            backgroundObjects.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            if (inputHelper.AnyKeyPressed)
            {
                GameEnvironment.SwitchTo("PlayingState");
            }
            base.HandleInput(inputHelper);
        }
    }
}
