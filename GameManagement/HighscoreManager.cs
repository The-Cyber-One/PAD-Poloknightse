using System;
using System.Threading.Tasks;

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
            Highscore highscore = new Highscore { PlayerName = GameEnvironment.PlayerName, Score = score, Level = level, DateTime = DateTime.UtcNow };
            db.AddHighscore(highscore);
        }

        /// <summary>
        /// Save the new Player
        /// </summary>
        public static void SavePlayer(string name)
        {
            AppDb db = new AppDb();
            db.AddPlayer(name);
        }

        public static bool PlayerNameExists(string name)
        {
            AppDb db = new AppDb();
            return db.PlayerNameExists(name).Result;
        }

        /// <summary>
        /// Gets the whole highscore table orderd by score
        /// </summary>
        public static async Task<Table> LoadScore()
        {
            AppDb db = new AppDb();
            Table table = await db.GetTable("SELECT * FROM Highscore ORDER BY score ASC;");
            return table;
        }

        /// <summary>
        /// Gets the whole highscore table orderd by most recent
        /// </summary>
        public static async Task<Table> LoadRecent()
        {
            AppDb db = new AppDb();
            Table table = await db.GetTable("SELECT * FROM Highscore ORDER BY dateTime DESC;");
            return table;
        }
    }
}
