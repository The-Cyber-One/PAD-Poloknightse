using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna;
using System.Data;

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
            Table highscores = await db.GetTable("SELECT * FROM Highscore;");

            Debug.WriteLine(highscores.ToString());
            //foreach (DataRow row in highscores.Rows)
            //{
            //    foreach(object item in row.ItemArray)
            //    {
            //        Debug.Write(item.ToString() + " ");
            //    }
            //    Debug.WriteLine("");
            //}
            //string test = highscores.Rows[0].ItemArray[1].ToString();
            //Debug.WriteLine(test);
        }
    }
}
