﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Poloknightse
{
    class Game1 : GameEnvironment
    {
        public Game1()
        {
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            screen = new Point(1600, 900);
            ApplyResolutionSettings();
            base.Initialize();
            

            //Initialize GameStates
            gameStateDict.Add(GameStates.START_STATE, new StartState());
            gameStateDict.Add(GameStates.PLAYING_STATE, new PlayingState());
            //gameStateDict.Add(GameStates.WIN_STATE, new WinState());
            //gameStateDict.Add(GameStates.GAME_OVER_STATE, new GameOverState());

            SwitchTo(GameStates.START_STATE);
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
        
            // TODO: Add your update logic here

            base.Update(gameTime);
        }
    }
}
