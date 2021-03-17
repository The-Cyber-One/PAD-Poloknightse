using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna;

namespace Poloknightse
{
    public static class HighscoreManager
    {
        /// <summary>
        /// Save the score after level is completed
        /// </summary>
        /// <param name="score">The achieved score</param>
        /// <param name="level">The level that was played</param>
        public static void SaveScore(int score, int level)
        {
            AppDb db = new AppDb();
            Highscore highscore = new Highscore {PlayerName = GameEnvironment.PlayerName, Score = score, Level = level, DateTime = DateTime.UtcNow };
            db.AddHighscore(highscore);
        }

        /// <summary>
        /// Gets the whole highscore table
        /// </summary>
        public static async void LoadScore()
        {
            AppDb db = new AppDb();
            List<Highscore> highscores = await db.GetHighscore();
            foreach (Highscore highscore in highscores)
            {
                Debug.WriteLine(highscore);
            }
        }
    }
}
