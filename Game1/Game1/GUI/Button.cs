using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game1
{
    /// <summary>
    /// Статус кнопки
    /// </summary>
    public enum ButtonStatus
    {
        /// <summary>
        /// Состояние покоя
        /// </summary>
        Normal,
        /// <summary>
        /// Мышка наведена
        /// </summary>
        MouseOver,
        /// <summary>
        /// Кнопка нажата
        /// </summary>
        Pressed,
    }
    class Button : Sprite
    {
        // хранит MouseState из предыдущего кадра
        private MouseState previousState;

        // текстуры состояний
        private Texture2D hoverTexture;
        private Texture2D pressedTexture;

        // прямоугольник границ
        private Rectangle bounds;

        // хранит текущее состояние кнопки
        private ButtonStatus state = ButtonStatus.Normal;
        // событие клика
        public event EventHandler Clicked;

        // событие нажата но н отпущена
        public event EventHandler OnPress;

        public Button(Texture2D texture, Texture2D hoverTexture, Texture2D pressedTexture, Vector2 position)
            : base(texture, position)
        {
            this.hoverTexture = hoverTexture;
            this.pressedTexture = pressedTexture;

            this.bounds = new Rectangle((int)position.X, (int)position.Y,
                texture.Width, texture.Height);
        }

        /// <summary>
        /// Обновляем состояния кнопки
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        public override void Update(GameTime gameTime)
        {
            // Получаем состояние мыши
            MouseState mouseState = Mouse.GetState();

            int mouseX = mouseState.X;
            int mouseY = mouseState.Y;

            bool isMouseOver = bounds.Contains(mouseX, mouseY);

            // обновим состояние
            if (isMouseOver && state != ButtonStatus.Pressed)
            {
                state = ButtonStatus.MouseOver;
            }
            else if (isMouseOver == false && state != ButtonStatus.Pressed)
            {
                state = ButtonStatus.Normal;
            }
            // Проверим если зажата кнопка
            if (mouseState.LeftButton == ButtonState.Pressed &&
             previousState.LeftButton == ButtonState.Released)
            {
                if (isMouseOver == true)
                {
                    // обновим
                    state = ButtonStatus.Pressed;
                    if (OnPress != null)
                    {
                        // очистим событие
                        OnPress(this, EventArgs.Empty);
                    }
                }
            }

            // Если игрок отпустил кнопку
            if (mouseState.LeftButton == ButtonState.Released &&
             previousState.LeftButton == ButtonState.Pressed)
            {
                if (isMouseOver == true)
                {
                    // обновим статус
                    state = ButtonStatus.MouseOver;

                    if (Clicked != null)
                    {
                        // очистим
                        Clicked(this, EventArgs.Empty);
                    }
                }

                else if (state == ButtonStatus.Pressed)
                {
                    state = ButtonStatus.Normal;
                }
            }

            previousState = mouseState;

        }

        /// <summary>
        /// Нарисует кнопку
        /// </summary>
        /// <param name="spriteBatch">A SpriteBatch that has been started</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            switch (state)
            {
                case ButtonStatus.Normal:
                    spriteBatch.Draw(texture, bounds, Color.White);
                    break;
                case ButtonStatus.MouseOver:
                    spriteBatch.Draw(hoverTexture, bounds, Color.White);
                    break;
                case ButtonStatus.Pressed:
                    spriteBatch.Draw(pressedTexture, bounds, Color.White);
                    break;
                default:
                    spriteBatch.Draw(texture, bounds, Color.White);
                    break;
            }
        }
        //END

    }
}
