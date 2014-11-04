using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game1
{
    class Wave : iGameObject
    {
        private int numOfEnemies; // Количество врагов
        private int waveNumber; // номер волны
        private float spawnTimer = 0; // Когда мы должны создать врагов
        private int enemiesSpawned = 0; // сколько врагов делать

        private bool enemyAtEnd; // Дошел до конца или нет?
        private bool spawningEnemies; // продолжать порождать врагов?
        private Level level; // ссылка на уровень игры
        private Texture2D enemyTexture; // текстуры врагов
        public List<Enemy> enemies = new List<Enemy>(); // список врагов
        private Player player; // ссылка на игрока

        public bool RoundOver
        {
            get
            {
                return enemies.Count == 0 && enemiesSpawned == numOfEnemies;
            }
        }
        public int RoundNumber
        {
            get { return waveNumber; }
        }

        public bool EnemyAtEnd
        {
            get { return enemyAtEnd; }
            set { enemyAtEnd = value; }
        }
        public List<Enemy> Enemies
        {
            get { return enemies; }
        }

        public Wave(int waveNumber, int numOfEnemies, Player player, Level level, Texture2D enemyTexture)
        {
            this.waveNumber = waveNumber;
            this.numOfEnemies = numOfEnemies;

            this.player = player; // Reference the player.
            this.level = level;

            this.enemyTexture = enemyTexture;
        }


        private void AddEnemy()
        {
            Enemy enemy = new Enemy(enemyTexture, level.Waypoints.Peek(), 100, 1, 0.6f);
            enemy.SetWaypoints(level.Waypoints);
            enemies.Add(enemy);
            spawnTimer = 0;

            enemiesSpawned++;
        }

        public void Start()
        {
            spawningEnemies = true;
        }



        public void Update(GameTime gameTime)
        {
            if (enemiesSpawned == numOfEnemies)
                spawningEnemies = false; 
            if (spawningEnemies)
            {
                spawnTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (spawnTimer > 2)
                    AddEnemy(); 
            }
            for (int i = 0; i < enemies.Count; i++)
            {
                Enemy enemy = enemies[i];
                enemy.Update(gameTime);
                if (enemy.IsDead)
                {
                    if (enemy.CurrentHealth > 0) 
                    {
                        enemyAtEnd = true;
                        player.Lives -= 1;
                    }
                    else
                    {
                        player.Money += enemy.BountyGiven;
                    }

                    enemies.Remove(enemy);
                    i--;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Enemy enemy in enemies)
                enemy.Draw(spriteBatch);
        }
        //END CLASS
    }
}
