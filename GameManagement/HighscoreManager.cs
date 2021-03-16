using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data.SqlClient;
using System.Security;
using System.Net;
using System.Diagnostics;
using Microsoft.Xna;
using System.Linq;

namespace Poloknightse
{
    public static class HighscoreManager
    {
        private static string saveFilePath = "Data";
        private static string connectionString = "jdbc:mysql://oege.ie.hva.nl/dekkerm51?serverTimezone=UTC; Database = zdekkerm51;";
        private static readonly SecureString secureString = new NetworkCredential("", "nC8uQl1Muz#Oa7K#").SecurePassword;
        private static SqlCredential credential;

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

        public static void SaveScore(int score, int level)
        {
            using (SqlConnection connection = new SqlConnection(connectionString, credential))
            {
                SqlCommand command = new SqlCommand("SELECT * FROM Highscore", connection);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }


        public static void LoadScore()
        {
            secureString.MakeReadOnly();
            credential = new SqlCredential("dekkerm51", secureString);
            using (SqlConnection connection = new SqlConnection(connectionString, credential))
            {
                SqlCommand command = new SqlCommand("SELECT * FROM Highscore", connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Debug.WriteLine(reader[0]);
                }
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}
