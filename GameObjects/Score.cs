using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Poloknightse
{
    class Score : GameObject
    {
        SpriteFont spriteFont;
        public Vector2 position;
        public string text;
        public int score = 0;
        
        public Score() : base(new Point(0, 0))
        {
            spriteFont = GameEnvironment.ContentManager.Load<SpriteFont>("GameObjects/GameFont");
            position.X = 10;
            position.Y = 10;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            text = "score: " + score;
            spriteBatch.DrawString(spriteFont, text, position, Color.White);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
        }
    }
}
