using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;

namespace BaseProject
{
    class LevelLoader
    {
        public static void LoadLevel(string levelName)
        {
            Texture2D level = GameEnvironment.ContentManager.Load<Texture2D>("Levels/" + levelName);
            Color[] colors = new Color[level.Width * level.Height];
            level.GetData<Color>(colors);
            Debug.WriteLine(colors[0]);
        }
    }
}
