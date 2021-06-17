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

        /// <summary>
        /// Make a clickable level select button
        /// </summary>
        /// <param name="buttonBox">A rectangle for the X-coordinates, Y-coordinates, width and height, respectively</param>
        /// <param name="level">Which level should the button refer to</param>
        public LevelSelectButton(Rectangle buttonBox, int level) : base(buttonBox, Game1.levels[level] + "Drawn", Game1.levels[level])
        {
            this.level = level;
        }
    }
}
