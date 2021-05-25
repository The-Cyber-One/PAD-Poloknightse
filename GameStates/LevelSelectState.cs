using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Poloknightse
{
    class LevelSelectState : GameState
    {
        Point offset = new Point(6, 4);
        Point startPosition = new Point(8, 5);
        Point buttonSize = new Point(12, 12);
        Vector2 titleTextPosition = new Vector2(32, 2.5f);
        GameObjectList buttons = new GameObjectList();

        //Back button
        Point backButtonPosition = new Point(2, 1);
        Point backButtonSize = new Point(4, 4);
        string backButtonAssetName = "Back";
        string backButtonText = "Main menu";
        Button backButton;
        float backButtonTextSize = 0.4f;
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

            //Back button
            Point convertedButtonPosition = LevelLoader.GridPointToWorld(backButtonPosition).ToPoint();
            Point convertedButtonSize = LevelLoader.GridPointToWorld(backButtonSize).ToPoint();
            Rectangle button = new Rectangle(convertedButtonPosition, convertedButtonSize);
            backButton = new Button(button, backButtonAssetName, backButtonText, backButtonTextSize);
            gameObjectList.Add(backButton);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            LevelLoader.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || inputHelper.KeyPressed(Keys.Escape) || inputHelper.KeyPressed(Keys.Back))
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
            
            if (backButton.clicked)
            {
                GameEnvironment.SwitchTo("StartState");
            }

            base.HandleInput(inputHelper);
        }
    }
}
