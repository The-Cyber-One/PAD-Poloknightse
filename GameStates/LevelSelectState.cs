using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Poloknightse
{
    class LevelSelectState : GameState
    {
        Point offset = new Point(4, 4);
        Point startPosition = new Point(4, 8);
        Point buttonSize = new Point(16, 10);
        GameObjectList buttons = new GameObjectList();
        public LevelSelectState()
        {

        }

        public override void Init()
        {
            LevelLoader.LoadLevel("LevelSelectMenu");
            gameObjectList.Add(new TextGameObject("Select a level", new Vector2(GameEnvironment.Screen.X / 2, GameEnvironment.Screen.Y / 8), Vector2.One / 2, Color.Black, "Fonts/Title"));
            Point convertedOffset = LevelLoader.GridPointToWorld(offset).ToPoint();
            Point convertedPosition = LevelLoader.GridPointToWorld(startPosition).ToPoint();
            Point convertedSize = LevelLoader.GridPointToWorld(buttonSize).ToPoint();
            Point positionOffset = convertedSize + convertedOffset;
            for (int i = 0; i < Game1.levels.Length / 2; i++)
            {
                    Rectangle buttonLocation = new Rectangle(convertedPosition.X + positionOffset.X * i, convertedPosition.Y, convertedSize.X, convertedSize.Y);
                    buttons.Add(new Button(buttonLocation));

                    Rectangle buttonLocation2 = new Rectangle(convertedPosition.X + positionOffset.X * i , convertedPosition.Y + positionOffset.Y, convertedSize.X, convertedSize.Y);
                    buttons.Add(new Button(buttonLocation2));                           
            }
            gameObjectList.Add(buttons);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            LevelLoader.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }
        public override void HandleInput(InputHelper inputHelper)
        {
            for (int i = 0; i < buttons.Children.Count; i++)
            {
                if (((Button)buttons.Children[i]).clicked)
                {
                    GameEnvironment.SwitchTo("PlayingState");
                }
            }
            base.HandleInput(inputHelper);
        }
    }
}
