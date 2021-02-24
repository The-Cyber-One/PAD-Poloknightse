using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BaseProject
{
    class GameEnvironment : Game
    {
        protected GraphicsDeviceManager graphics;
        protected SpriteBatch spriteBatch;
        static protected ContentManager content;
        protected static Point screen;
        protected static Random random;
        public const int gridTileSize = 16;

        static protected Dictionary<GameStates, GameState> gameStateDict;
        public enum GameStates
        {
            START_STATE,
            PLAYING_STATE,
            WIN_STATE,
            GAME_OVER_STATE
        }
        static protected GameState currentGameState;

        public static KeyboardState KeyboardState
        {
            get { return Keyboard.GetState(); }
        }

        public static Point Screen
        {
            get { return screen; }
        }

        public static Random Random
        {
            get { return random; }
        }

        public static ContentManager ContentManager
        {
            get { return content; }
        }

        static public void SwitchTo(GameStates gameStateName)
        {
            if (gameStateDict.ContainsKey(gameStateName))
            {
                currentGameState = gameStateDict.GetValueOrDefault(gameStateName);
                currentGameState.Init();
            }
        }

        public GameEnvironment()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            content = Content;
            gameStateDict = new Dictionary<GameStates, GameState>();
            random = new Random();
        }

        /// <summary>
        /// Apply all new settings from graphics
        /// </summary>
        /// <param name="fullScreen">Boolean to set the game to fullscreen.</param>
        public void ApplyResolutionSettings(bool fullScreen = false)
        {
            if (GraphicsDevice == null)
            {
                graphics.ApplyChanges();
            }

            if (fullScreen)
            {
                graphics.IsFullScreen = true;
                screen.X = GraphicsDevice.Adapter.CurrentDisplayMode.Width;
                screen.Y = GraphicsDevice.Adapter.CurrentDisplayMode.Height;
            }

            graphics.PreferredBackBufferWidth = screen.X;
            graphics.PreferredBackBufferHeight = screen.Y;

            graphics.ApplyChanges();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            if (currentGameState != null)
                currentGameState.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        protected override void Update(GameTime gameTime)
        {
            if (currentGameState != null)
                currentGameState.Update(gameTime);

            base.Update(gameTime);
        }
    }
}
