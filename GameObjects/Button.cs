using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Poloknightse
{
    class Button : GameObject
    {
        // Variables for the button outline and changes when the mouse is hovering over the button
        private const int OUTLINE_THICKNESS = 10, BACKGROUND_SIZE = 10;
        int outlineSize;
        
        Color outlineColor,                                 // Variable to hold the current color of the outline
            outlineColorStart = Color.Black,                // Color the outline starts with
            outlineColorHovering = new Color(82, 101, 32);  // Color the outline will be when the mouse is hovering over it
        
        int hoveringSizeChange = 10; // How much the button will shrink when the mouse is hovering over

        public bool hovering, clicked; // Booleans to track the mouse interaction with the button

        private Texture2D outlinePixel;


        // Rectangle which contains the X-coordinates, Y-coordinates, width and height of the button
        // Together with the buttonSize, this determines the size of the button
        Rectangle buttonBox;
        int buttonSize;
        

        // Variables for the text underneath the button
        TextGameObject buttonText;
        Point textOffset = new Point(1, 1); // Determines where the text will come relative to the button, 1 grid position below the button
        Vector2 textPosition;
        float textSize = 0.6f; // Standard value for the textSize

        /// <summary>
        /// Make a clickable button
        /// </summary>
        /// <param name="buttonBox">A rectangle for the X-coordinates, Y-coordinates, width and height, respectively</param>
        /// <param name="assetName">Name of the image used for the button, make sure the image is in the Menu folder</param>
        /// <param name="buttonText">The text underneath the button</param>
        /// <param name="textSize">Size the text under the button will be, optional</param>
        public Button(Rectangle buttonBox, string assetName, string buttonText, float textSize = 0.6f) : base(new Point(buttonBox.X, buttonBox.Y),"Menu/" + assetName)
        {
            this.buttonBox = buttonBox;

            // Convert the text grid coordinates to real coordinates of the screen
            textOffset = LevelLoader.GridPointToWorld(textOffset).ToPoint();

            this.textSize = textSize;

            // Set the postion for the text
            textPosition = new Vector2((buttonBox.X + buttonBox.Width / 2), (buttonBox.Y + buttonBox.Height) + textOffset.Y);

            // Create the text for the button
            this.buttonText = new TextGameObject(buttonText, textPosition, Vector2.One / 2, Color.Black, "Fonts/Title", this.textSize);
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            base.HandleInput(inputHelper);

            hovering = clicked = false; // Reset both booleans

            if (buttonBox.Contains(inputHelper.MousePosition)) // Check if mouse is inside the button hitbox
            {
                hovering = true;
                if (inputHelper.MouseLeftButtonPressed()) // Click button if the left mouse button is pressed
                {
                    clicked = true;
                }
            }
        }

        private void CreateOutline(SpriteBatch spriteBatch)
        {
            outlinePixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            outlinePixel.SetData(new[] { Color.White });
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            // Reset the outline color, outline size and the button size back to their normal value
            outlineColor = outlineColorStart;
            outlineSize = buttonBox.Width - BACKGROUND_SIZE;
            buttonSize = buttonBox.Width - OUTLINE_THICKNESS - BACKGROUND_SIZE;
            
            if (hovering) // If the mouse is hovering over the button hitbox,
                // change the outline color, the size of the outline and change the size of the button itself
            {
                outlineColor = outlineColorHovering;
                outlineSize = buttonBox.Width - hoveringSizeChange - BACKGROUND_SIZE;
                buttonSize = buttonBox.Width - OUTLINE_THICKNESS - hoveringSizeChange - BACKGROUND_SIZE;
            }


        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (outlinePixel == null) CreateOutline(spriteBatch);

            // Draw the outline
            spriteBatch.Draw(outlinePixel, new Rectangle(buttonBox.X + (buttonBox.Width - outlineSize) / 2,
                buttonBox.Y + (buttonBox.Height - outlineSize) / 2, outlineSize, outlineSize), outlineColor);
            // Draw the button
            spriteBatch.Draw(texture, new Rectangle(buttonBox.X + (buttonBox.Width - buttonSize) / 2,
                buttonBox.Y + (buttonBox.Height - buttonSize) / 2, buttonSize, buttonSize), Color.White);
            // Draw the text underneath the button
            buttonText.Draw(spriteBatch);
        }
    }
}
