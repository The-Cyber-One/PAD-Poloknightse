using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Poloknightse
{
    class LevelSelectButton : Button
    {
        public int level;

        public LevelSelectButton(Rectangle buttonBox, int level) : base(buttonBox, "Menu/" + Game1.levels[level] + "Drawn", Game1.levels[level])
        {
            this.level = level;
        }
    }
}
