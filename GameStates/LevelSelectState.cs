using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Poloknightse
{
    class LevelSelectState : GameState
    {
        Point offset = new Point(8, 4);
        Point startPosition = new Point(6, 5);
        Point buttonSize = new Point(12, 12);
        Vector2 titleTextPosition = new Vector2(32, 2.5f);
        GameObjectList buttons = new GameObjectList();
        public LevelSelectState()
        {

        }

        public override void Init()
        {
            LevelLoader.LoadLevel("Menu/LevelSelectMenu");
            gameObjectList.Add(new TextGameObject("Select a level", LevelLoader.GridPointToWorld(titleTextPosition), Vector2.One / 2, Color.Black, "Fonts/Title"));
            Point convertedOffset = LevelLoader.GridPointToWorld(offset).ToPoint();
            Point convertedPosition = LevelLoader.GridPointToWorld(startPosition).ToPoint();
            Point convertedSize = LevelLoader.GridPointToWorld(buttonSize).ToPoint();
            Point positionOffset = convertedSize + convertedOffset;
            for (int i = 0; i < Game1.levels.Length / 2; i++)
            {
                Rectangle buttonLocation = new Rectangle(convertedPosition.X + positionOffset.X * i, convertedPosition.Y, convertedSize.X, convertedSize.Y);
                buttons.Add(new LevelSelectButton(buttonLocation, i));

                Rectangle buttonLocation2 = new Rectangle(convertedPosition.X + positionOffset.X * i, convertedPosition.Y + positionOffset.Y, convertedSize.X, convertedSize.Y);
                buttons.Add(new LevelSelectButton(buttonLocation2, i + Game1.levels.Length / 2));
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || inputHelper.KeyPressed(Keys.Escape))
            {
                GameEnvironment.SwitchTo("StartState");
            }

            foreach (LevelSelectButton button in buttons.Children)
            {
                if (button.clicked)
                {
                    Game1.currentLevel = button.level;
                    GameEnvironment.SwitchTo("PlayingState");
                    break;
                }
            }

            base.HandleInput(inputHelper);
        }
    }
}
