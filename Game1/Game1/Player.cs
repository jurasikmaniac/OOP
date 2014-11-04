using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game1
{
    class Player : iGameObject
    {
        private int money = 50;
        private int lives = 3;
        private int cellX;
        private int cellY;

        private int tileX;
        private int tileY;

        private List<Tower> towers = new List<Tower>();

        private MouseState mouseState; // статус мышки текущий
        private MouseState oldState; // предыдущий
        // каринки пуль и башен
        private Texture2D[] towerTextures;
        private Texture2D bulletTexture;

        // тип добавляемой башни
        private string newTowerType;
        //индекс новой башни
        private int newTowerIndex;

        public int NewTowerIndex
        {
            set { newTowerIndex = value; }
        }


        public string NewTowerType
        {
            set { newTowerType = value; }
        }

        public int Money
        {
            get { return money; }
            set { money = value; }
        }
        public int Lives
        {
            get { return lives; }
            set { lives = value; }
        }

        private Level level;

        //public Player(Level level)
        //{
        //    this.level = level;
        //}

        public void DrawPreview(SpriteBatch spriteBatch)
        {
            
            if (string.IsNullOrEmpty(newTowerType) == false)
            {
                int cellX = (int)(mouseState.X / 32); // Convert the position of the mouse
                int cellY = (int)(mouseState.Y / 32); // from array space to level space

                int tileX = cellX * 32; // Convert from array space to level space
                int tileY = cellY * 32; // Convert from array space to level space

                Texture2D previewTexture = towerTextures[newTowerIndex];
                spriteBatch.Draw(previewTexture, new Rectangle(tileX, tileY,
                    previewTexture.Width, previewTexture.Height), Color.White);
            }
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
                if (string.IsNullOrEmpty(newTowerType) == false)
                {
                    AddTower();
                }
            }

            foreach (Tower tower in towers)
            {
                if (tower.HasTarget == false)
                {
                    tower.GetClosestEnemy(enemies);
                }

                tower.Update(gameTime);
            }
            oldState = mouseState; 
        }

        private bool IsCellClear()
        {
            bool inBounds = cellX >= 0 && cellY >= 0 &&                 cellX < level.Width && cellY < level.Height;

            bool spaceClear = true;

            foreach (Tower tower in towers) 
            {
                spaceClear = (tower.Position != new Vector2(tileX, tileY));

                if (!spaceClear)
                    break;
            }

            bool onPath = (level.GetIndex(cellX, cellY) != 1);

            return inBounds && spaceClear && onPath; // If both checks are true return true
        }

        /// <summary>
        /// Construct a new player.
        /// </summary>
        public Player(Level level, Texture2D[] towerTextures, Texture2D bulletTexture)
        {
            this.level = level;

            this.towerTextures = towerTextures;
            this.bulletTexture = bulletTexture;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Tower tower in towers)
            {
                tower.Draw(spriteBatch);
            }
        }


        /// <summary>
        /// Adds a tower to the player's collection.
        /// </summary>
        public void AddTower()
        {
            Tower towerToAdd = null;

            switch (newTowerType)
            {
                case "Arrow Tower":
                    {
                        towerToAdd = new ArrowTower(towerTextures[0],
                            bulletTexture, new Vector2(tileX, tileY));
                        break;
                    }
                case "Spike Tower":
                    {
                        towerToAdd = new SpikeTower(towerTextures[1],
                            bulletTexture, new Vector2(tileX, tileY));
                        break;
                    }
                case "Slow Tower":
                    {
                        towerToAdd = new SlowTower(towerTextures[2],
                            bulletTexture, new Vector2(tileX, tileY));
                        break;
                    }
            }

           
            if (IsCellClear() == true && towerToAdd.Cost <= money)
            {
                towers.Add(towerToAdd);
                money -= towerToAdd.Cost;

           
                newTowerType = string.Empty;
            }
            else
            {
                newTowerType = string.Empty;
            }
        }

        public void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
