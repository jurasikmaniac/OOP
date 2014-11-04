﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    class Toolbar:Sprite
    {
       
        // Магический шрифт моногейма
        private SpriteFont font;

        protected Rectangle destrect;
        // Позиция текста
        private Vector2 textPosition;

        

        public Toolbar(Texture2D texture, SpriteFont font, Vector2 position, int screenWidth):base(texture, position)
        {
            

            this.texture = texture;
            this.font = font;

            this.position = position;

            destrect = new Rectangle();
            destrect.X = (int)position.X;
            destrect.Y = (int)position.Y;
            destrect.Width = screenWidth;
            destrect.Height = texture.Height;

            // Здвиг от края окна
            textPosition = new Vector2(130, position.Y + 10);
        }

        public void Draw(SpriteBatch spriteBatch, Player player)
        {

            spriteBatch.Draw(texture, destrect, Color.White);

            string text = string.Format("Adena : {0} Bottles : {1}", player.Money, player.Lives);
            spriteBatch.DrawString(font, text, textPosition, Color.White);
            //spriteBatch.DrawString(font, "10", textPosition, Color.White);
        }
    }
}
