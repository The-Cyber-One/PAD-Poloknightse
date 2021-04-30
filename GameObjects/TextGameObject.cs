using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Poloknightse
{
    class TextGameObject : GameObject
    {
        SpriteFont font;
        string text;
        Vector2 position;
        Color color;
        Vector2 origin;
        float angle;
        float scale;

        public TextGameObject(string text, Vector2 position, Vector2 origin = new Vector2(), Color? color = null, string FontPath = "Fonts/Paragraph", float scale = 1, float angle = 0) : base()
        {
            font = GameEnvironment.ContentManager.Load<SpriteFont>(FontPath);

            this.text = text;
            this.position = position;
            if (color == null) this.color = Color.Black;
            else this.color = color.Value;
            this.origin = font.MeasureString(text) * origin;
            this.angle = angle;
            this.scale = scale;
            
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, text, position, color, angle, origin, scale, SpriteEffects.None, layer);
        }
    }
}
