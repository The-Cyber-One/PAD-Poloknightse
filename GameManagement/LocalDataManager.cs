using System.IO;

namespace Poloknightse
{
    class LocalDataManager
    {
        private static readonly string saveFilePath = "Data";

        /// <summary>
        /// Save player name to local file
        /// </summary>
        /// <param name="name">Player name</param>
        public static void SaveName(string name)
        {
            StreamWriter writer = new StreamWriter(saveFilePath);
            writer.WriteLine(name);
            writer.Close();
        }

        /// <summary>
        /// Load player name from local file
        /// </summary>
        /// <returns>Player name</returns>
        public static string LoadName()
        {
            StreamReader reader = new StreamReader(saveFilePath);
            string name = reader.ReadLine();
            reader.Close();
            return name;
        }

    }
}
