using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;

namespace BaseProject
{
    class LevelLoader : GameObject
	{
        static Texture2D wallTile;
        static Texture2D groundTile;
        int placeX = 0;
        int placeY = 0;

        public LevelLoader() : base("LevelTiles/Cell03")
        {

        }

        public static void LoadLevel(string levelName)
        {
            Texture2D level = GameEnvironment.ContentManager.Load<Texture2D>("Levels/" + levelName);
            Color[] colors = new Color[level.Width * level.Height];
            level.GetData<Color>(colors);

            Color wall = new Color(0, 0, 0, 255);
            Color ground = new Color(255, 255, 255, 0);

            wallTile = GameEnvironment.ContentManager.Load<Texture2D>("LevelTiles/Cell20");
            groundTile = GameEnvironment.ContentManager.Load<Texture2D>("LevelTiles/Cell03");

            foreach (Color pixel in colors)
            {
                if (pixel == wall)
                {

                }
                else if (pixel == ground)
                {
                    
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                wallTile,           //Texture
                position,           //Positie
                null,               //Rectangle size
                Color.White,        //Kleur
                0f,                 //Rotatie
                Vector2.Zero,       //Origin 
                0.2f,               //Scale in %
                SpriteEffects.None, //Texture effecten
                0f);                //Layer

            spriteBatch.Draw(
                groundTile,           
                new Vector2(placeX+wallTile.Width,placeY),           
                null,              
                Color.White,        
                0f,                 
                Vector2.Zero,       
                0.2f,              
                SpriteEffects.None, 
                0f);                
        }
    }
}
