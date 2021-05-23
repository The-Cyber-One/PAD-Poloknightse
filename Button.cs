using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Poloknightse
{
    class Button : GameObject
    {
        int test = 0;
        Rectangle buttonBox;
        public bool hovering, clicked;
        public Button(Rectangle buttonBox, String buttonSprite) : base (new Point(buttonBox.X, buttonBox.Y), "Levels/" + buttonSprite)
        {
            this.buttonBox = buttonBox;
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            base.HandleInput(inputHelper);
            hovering = clicked = false;
            if (buttonBox.Contains(inputHelper.MousePosition))
            {
                hovering = true;
                Debug.WriteLine(test++);
                if (inputHelper.MouseLeftButtonPressed())
                {
                    clicked = true;
                }
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
