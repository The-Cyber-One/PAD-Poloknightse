using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Poloknightse
{
    class LevelSelectButton : GameObject
    {
        private const int OUTLINE_THICKNESS = 10, BACKGROUND_SIZE = 10;
        int outlineSize, levelSize;
        Color outlineColor, outlineColorStart = Color.Black, outlineColorHovering = new Color(82, 101, 32);
        int hoveringSizeChange = 10;
        Rectangle buttonBox;
        public bool hovering, clicked;
        private Texture2D pixel;
        public int level;
        TextGameObject buttonText;
        Point textOffset = new Point(1, 1);

        public LevelSelectButton(Rectangle buttonBox, int level) : base(new Point(buttonBox.X, buttonBox.Y), "Menu/" + Game1.levels[level] + "Drawn")
        {
            this.buttonBox = buttonBox;
            this.level = level;
            textOffset = LevelLoader.GridPointToWorld(textOffset).ToPoint();
            buttonText = new TextGameObject(Game1.levels[level], new Vector2((buttonBox.X + buttonBox.Width / 2), (buttonBox.Y + buttonBox.Height) + textOffset.Y), Vector2.One / 2, Color.Black, "Fonts/Title", 0.6f);
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
            outlineSize = buttonBox.Width - BACKGROUND_SIZE;
            levelSize = buttonBox.Width - OUTLINE_THICKNESS - BACKGROUND_SIZE;
            if (hovering)
            {
                outlineColor = outlineColorHovering;
                outlineSize = buttonBox.Width - hoveringSizeChange - BACKGROUND_SIZE;
                levelSize = buttonBox.Width - OUTLINE_THICKNESS - hoveringSizeChange - BACKGROUND_SIZE;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (pixel == null) CreatePixel(spriteBatch);

            spriteBatch.Draw(pixel, new Rectangle(buttonBox.X + (buttonBox.Width - outlineSize) / 2, buttonBox.Y + (buttonBox.Height - outlineSize) / 2, outlineSize, outlineSize), outlineColor);
            spriteBatch.Draw(texture, new Rectangle(buttonBox.X + (buttonBox.Width - levelSize) / 2, buttonBox.Y + (buttonBox.Height - levelSize) / 2, levelSize, levelSize), Color.White);
            buttonText.Draw(spriteBatch);
        }
    }
}
