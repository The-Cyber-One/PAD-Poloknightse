using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
                buttons.Add(new Button(buttonLocation, Game1.levels[i] + "Drawn"));

                Rectangle buttonLocation2 = new Rectangle(convertedPosition.X + positionOffset.X * i, convertedPosition.Y + positionOffset.Y, convertedSize.X, convertedSize.Y);
                buttons.Add(new Button(buttonLocation2, Game1.levels[i * 2] + "Drawn"));
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
            if (((Button)buttons.Children[0]).clicked)
            {
                Game1.currentLevel = 0;
                GameEnvironment.SwitchTo("PlayingState");
            } else if (((Button)buttons.Children[1]).clicked)
            {
                Game1.currentLevel = 3;
                GameEnvironment.SwitchTo("PlayingState");
            } else if (((Button)buttons.Children[2]).clicked)
            {
                Game1.currentLevel = 1;
                GameEnvironment.SwitchTo("PlayingState");
            } else if (((Button)buttons.Children[3]).clicked)
            {
                Game1.currentLevel = 4;
                GameEnvironment.SwitchTo("PlayingState");
            } else if (((Button)buttons.Children[4]).clicked)
            {
                Game1.currentLevel = 2;
                GameEnvironment.SwitchTo("PlayingState");
            } else if (((Button)buttons.Children[5]).clicked)
            {
                Game1.currentLevel = 5;
                GameEnvironment.SwitchTo("PlayingState");
            }
            base.HandleInput(inputHelper);
        }
    }
}
