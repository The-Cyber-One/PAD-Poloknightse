using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Poloknightse
{
    class HighscoreState : GameState
    {
        Table highscore;
        TextGameObject highscoreText;

        public HighscoreState() : base()
        {

        }

        public override void Init()
        {
            LevelLoader.LoadLevel("Menu/HighscoreMenu");
            highscoreText = new TextGameObject("", Vector2.One, Vector2.Zero, Color.Black);
            gameObjectList.Add(highscoreText);
            GetHighscore();
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || inputHelper.KeyPressed(Keys.Escape))
            {
                GameEnvironment.SwitchTo("StartState");
            }
            base.HandleInput(inputHelper);
        }

        protected async void GetHighscore()
        {
            highscore = await HighscoreManager.LoadScore();
        }

        public override void Update(GameTime gameTime)
        {
            if (highscore != null && highscoreText.text == "")
            {
                highscoreText.text = highscore.ToString();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            LevelLoader.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }
    }
}
