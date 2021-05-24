using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Poloknightse
{
    class Button : GameObject
    {
        private const int OUTLINE_THICKNESS = 10, BACKGROUND_SIZE = 10;
        int outlineSize, buttonSize;
        
        Color outlineColor, outlineColorStart = Color.Black, outlineColorHovering = new Color(82, 101, 32);
        
        int hoveringSizeChange = 10;

        public bool hovering, clicked;
        
        Rectangle buttonBox;
        
        private Texture2D pixel;

        TextGameObject buttonText;
        Point textOffset = new Point(1, 1);

        public Button(Rectangle buttonBox, string assetName, string buttonText) : base(new Point(buttonBox.X, buttonBox.Y), assetName)
        {
            this.buttonBox = buttonBox;
            textOffset = LevelLoader.GridPointToWorld(textOffset).ToPoint();
            this.buttonText = new TextGameObject(buttonText, new Vector2((buttonBox.X + buttonBox.Width / 2), (buttonBox.Y + buttonBox.Height) + textOffset.Y), Vector2.One / 2, Color.Black, "Fonts/Title", 0.6f);
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
        private void CreatePixel(SpriteBatch spriteBatch)
        {
            pixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            pixel.SetData(new[] { Color.White });
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            outlineColor = outlineColorStart;
            outlineSize = buttonBox.Width - BACKGROUND_SIZE;
            buttonSize = buttonBox.Width - OUTLINE_THICKNESS - BACKGROUND_SIZE;
            if (hovering)
            {
                outlineColor = outlineColorHovering;
                outlineSize = buttonBox.Width - hoveringSizeChange - BACKGROUND_SIZE;
                buttonSize = buttonBox.Width - OUTLINE_THICKNESS - hoveringSizeChange - BACKGROUND_SIZE;
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (pixel == null) CreatePixel(spriteBatch);

            spriteBatch.Draw(pixel, new Rectangle(buttonBox.X + (buttonBox.Width - outlineSize) / 2, buttonBox.Y + (buttonBox.Height - outlineSize) / 2, outlineSize, outlineSize), outlineColor);
            spriteBatch.Draw(texture, new Rectangle(buttonBox.X + (buttonBox.Width - buttonSize) / 2, buttonBox.Y + (buttonBox.Height - buttonSize) / 2, buttonSize, buttonSize), Color.White);
            buttonText.Draw(spriteBatch);
        }
    }
}
