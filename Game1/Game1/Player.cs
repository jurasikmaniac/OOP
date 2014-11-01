using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game1
{
    class Player:iGameObject
    {
        private int money = 50;
        private int lives = 30;
        private int cellX;
        private int cellY;

        private int tileX;
        private int tileY;

        private List<Tower> towers = new List<Tower>();

        private MouseState mouseState; // Mouse state for the current frame
        private MouseState oldState; // Mouse state for the previous frame
        private Texture2D towerTexture;
        private Texture2D bulletTexture;

        public int Money
        {
            get { return money; }
        }
        public int Lives
        {
            get { return lives; }
        }

        private Level level;

        public Player(Level level)
        {
            this.level = level;
        }

        public void Update(GameTime gameTime, List<Enemy> enemies)
        {
            mouseState = Mouse.GetState();

            cellX = (int)(mouseState.X / 32); // Convert the position of the mouse
            cellY = (int)(mouseState.Y / 32); // from array space to level space

            tileX = cellX * 32; // Convert from array space to level space
            tileY = cellY * 32; // Convert from array space to level space
            if (mouseState.LeftButton == ButtonState.Released && oldState.LeftButton == ButtonState.Pressed)
            {
                if (IsCellClear())
                {
                    ArrowTower tower = new ArrowTower(towerTexture, bulletTexture, new Vector2(tileX, tileY));
                    towers.Add(tower);
                }
            }

            foreach (Tower tower in towers)
            {
                if (tower.Target == null)
                {
                    tower.GetClosestEnemy(enemies);
                }

                tower.Update(gameTime);
            }
            oldState = mouseState; // Set the oldState so it becomes the state of the previous frame.
        }

        private bool IsCellClear()
        {
            bool inBounds = cellX >= 0 && cellY >= 0 && // Make sure tower is within limits
                cellX < level.Width && cellY < level.Height;

            bool spaceClear = true;

            foreach (Tower tower in towers) // Check that there is no tower here
            {
                spaceClear = (tower.Position != new Vector2(tileX, tileY));

                if (!spaceClear)
                    break;
            }

            bool onPath = (level.GetIndex(cellX, cellY) != 1);

            return inBounds && spaceClear && onPath; // If both checks are true return true
        }

        public Player(Level level, Texture2D towerTexture, Texture2D bulletTexture)
        {
            this.level = level;

            this.towerTexture = towerTexture;
            this.bulletTexture = bulletTexture;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Tower tower in towers)
            {
                tower.Draw(spriteBatch);
            }
        }



        public void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
