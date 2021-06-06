using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Poloknightse
{
    class MainMenuButton : Button
    {
        public string gameStateName;

        /// <summary>
        /// Make a clickable button
        /// </summary>
        /// <param name="buttonBox">A rectangle for the X-coordinates, Y-coordinates, width and height, respectively</param>
        /// <param name="assetName">Name of the image used for the button, make sure the image is in the Menu folder</param>
        /// <param name="buttonText">The text underneath the button</param>
        /// <param name="gameStateName">Which gameState should the button refer to</param>
        public MainMenuButton(Rectangle buttonBox, string assetName, string buttonText, string gameStateName) 
            : base(buttonBox, assetName, buttonText)
        {
            this.gameStateName = gameStateName;
        }
    }
}
