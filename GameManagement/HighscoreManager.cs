using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Diagnostics;
using Microsoft.Xna;
using System.IO.IsolatedStorage;

namespace Poloknightse
{
    public static class HighscoreManager
    {
        private static string saveFilePath = "Data";

        public static void SaveName(string name)
        {
            StreamWriter writer = new StreamWriter(saveFilePath);
            writer.WriteLine(name);
            writer.Close();

            Debug.WriteLine(LoadName());
        }

        public static string LoadName()
        {
            StreamReader reader = new StreamReader(saveFilePath);
            string name = reader.ReadLine();
            reader.Close();
            return name;
        }
    }
}
