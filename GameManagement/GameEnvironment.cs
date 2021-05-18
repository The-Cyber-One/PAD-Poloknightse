using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Poloknightse
{
    class GameEnvironment : Game
    {
        protected GraphicsDeviceManager graphics;
        protected SpriteBatch spriteBatch;
        static protected ContentManager content;
        protected InputHelper inputHelper;
        protected static Point screen;
        protected static Random random;
        public static Point startGridPoint = new Point();
        static protected GameState currentGameState;
        protected static string playerName = "spelernaam hiero";

        static protected Dictionary<string, GameState> gameStateDict;

        public static string PlayerName
        {
            get { return playerName; }
        }

        public static GameState CurrentGameState
        {
            get { return currentGameState; }
        }

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

        public bool FullScreen
        {
            get { return graphics.IsFullScreen; }
            set
            {
                ApplyResolutionSettings(value);
            }
        }

        static public void SwitchTo(string gameStateName)
        {
            if (gameStateDict.ContainsKey(gameStateName))
            {
                if (currentGameState != null) currentGameState.Reset();
                currentGameState = gameStateDict.GetValueOrDefault(gameStateName);
                currentGameState.Init();
            }
        }

        static public T GetState<T>(string gameState)
        {
            return (T)Convert.ChangeType(gameStateDict[gameState], typeof(T));
        }

        public GameEnvironment()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            content = Content;
            gameStateDict = new Dictionary<string, GameState>();
            random = new Random();
            inputHelper = new InputHelper();
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

        protected void HandleInput()
        {
            inputHelper.Update();
            if (inputHelper.KeyPressed(Keys.Escape))
            {
                Exit();
            }
            if (inputHelper.KeyPressed(Keys.F11))
            {
                FullScreen = !FullScreen;
            }
            currentGameState.HandleInput(inputHelper);
        }

        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            if (currentGameState != null)
                currentGameState.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        protected override void Update(GameTime gameTime)
        {
            HandleInput();
            if (currentGameState != null)
            {
                currentGameState.Update(gameTime);
                currentGameState.FixedUpdate(gameTime);
            }

            base.Update(gameTime);
        }
    }
}
