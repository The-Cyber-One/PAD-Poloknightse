using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Poloknightse
{
    class mainMenuButton : Button
    {
        public string gameStateName;
        public mainMenuButton(Rectangle buttonBox, string assetName, string buttonText, string gameStateName) 
            : base(buttonBox, assetName, buttonText)
        {
            this.gameStateName = gameStateName;
        }
    }
}
