using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Poloknightse
{
    class MainMenuButton : Button
    {
        public string gameStateName;

        public MainMenuButton(Rectangle buttonBox, string assetName, string buttonText, string gameStateName) 
            : base(buttonBox, assetName, buttonText)
        {
            this.gameStateName = gameStateName;
        }
    }
}
