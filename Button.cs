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
        private const int OUTLINE_THICKNESS = 10;
        int outlineSize, levelSize;
        Color outlineColor, outlineColorStart = Color.Black, outlineColorHovering = new Color(82, 101, 32);
        int hoveringSizeChange = 10;
        Rectangle buttonBox;
        public bool hovering, clicked;
        private Texture2D pixel;
        public int level;

        public Button(Rectangle buttonBox, int level) : base(new Point(buttonBox.X, buttonBox.Y), "Levels/" + Game1.levels[level] + "Drawn")
        {
            this.buttonBox = buttonBox;
            this.level = level;
        }

        private void CreatePixel(SpriteBatch spriteBatch)
        {
            pixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            pixel.SetData(new[] { Color.White });
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            base.HandleInput(inputHelper);
            hovering = clicked = false;
            if (buttonBox.Contains(inputHelper.MousePosition))
            {
                hovering = true;
                if (inputHelper.MouseLeftButtonPressed())
                {
                    clicked = true;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            outlineColor = outlineColorStart;
            outlineSize = buttonBox.Height;
            levelSize = buttonBox.Height - OUTLINE_THICKNESS;
            if (hovering)
            {
                outlineColor = outlineColorHovering;
                outlineSize = buttonBox.Height - hoveringSizeChange;
                levelSize = buttonBox.Height - OUTLINE_THICKNESS - hoveringSizeChange;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (pixel == null) CreatePixel(spriteBatch);

            spriteBatch.Draw(pixel, new Rectangle(buttonBox.X + (buttonBox.Width - outlineSize) / 2, buttonBox.Y + (buttonBox.Height - outlineSize) / 2, outlineSize, outlineSize), outlineColor);
            spriteBatch.Draw(texture, new Rectangle(buttonBox.X + (buttonBox.Width - levelSize) / 2, buttonBox.Y + (buttonBox.Height - levelSize) / 2, levelSize, levelSize), Color.White);
        }
    }
}
