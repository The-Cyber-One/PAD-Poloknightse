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
        public const int gridSize = 16;

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
            ApplyResolutionSettings();

            LevelLoader.Initialize();
        }

        public void ApplyResolutionSettings()
        {
            if (GraphicsDevice == null)
            {
                graphics.ApplyChanges();
            }
            graphics.PreferredBackBufferWidth = 1600;
            graphics.PreferredBackBufferHeight = 900;
            
            //graphics.PreferredBackBufferWidth = GraphicsDevice.Adapter.CurrentDisplayMode.Width;
            //graphics.PreferredBackBufferHeight = GraphicsDevice.Adapter.CurrentDisplayMode.Height;
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
