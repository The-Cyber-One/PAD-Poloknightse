using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Poloknightse
{
    class Game1 : GameEnvironment
    {
        public static int currentLevel;
        public static bool exit = false;

        public static string[] levels =
            {
            "Tutorial",
            "Level-1",
            "Level-2",
            "Level-3",
            "Level-4",
            "Level-5"
            };

        public Game1()
        {
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            screen = new Point(1600, 900);
            ApplyResolutionSettings();
            base.Initialize();
            

            //Initialize GameStates
            gameStateDict.Add("StartState", new StartState());
            gameStateDict.Add("PlayingState", new PlayingState());
            gameStateDict.Add("WinState", new WinState());
            gameStateDict.Add("GameOverState", new GameOverState());
            gameStateDict.Add("LevelSelectState", new LevelSelectState());
            gameStateDict.Add("HighscoreState", new HighscoreState());
            gameStateDict.Add("CreditsState", new CreditsState());
            gameStateDict.Add("PauseState", new PauseState());
            gameStateDict.Add("NameState", new NameState());

            SwitchTo("NameState");
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            if (exit) Exit();
            // TODO: Add your update logic here
            
            base.Update(gameTime);
        }
    }
}
