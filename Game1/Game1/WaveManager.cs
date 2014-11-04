using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game1
{
    class WaveManager : iGameObject
    {
        private int numberOfWaves; // Сколько волн будет
        private float timeSinceLastWave; // Ккак давно последняя вола закончилась

        private Queue<Wave> waves = new Queue<Wave>(); // Очередь волн

        private Texture2D enemyTexture; // текстура врагов

        private bool waveFinished = false; // состояние волны

        private Level level; // ссылка на класс уровня игры

        public Wave CurrentWave // Получить волну в начале очереди
        {
            get { return waves.Peek(); }
        }

        public List<Enemy> Enemies // Получить список текущих врагов
        {
            get { return CurrentWave.Enemies; }
        }

        public int Round // Вернет номер волны
        {
            get { return CurrentWave.RoundNumber + 1; }
        }


        public void Update(GameTime gameTime)
        {
            CurrentWave.Update(gameTime); // Обновим волну

            if (CurrentWave.RoundOver) // проверка на конец волны
            {
                waveFinished = true;
            }

            if (waveFinished) // если волна завершилась статуем таймер отсчета
            {
                timeSinceLastWave += (float)gameTime.ElapsedGameTime.TotalSeconds; // Start the timer
            }

            if (timeSinceLastWave > 3.0f) // после х.х секунд начнем новую
            {
                waves.Dequeue(); 
                StartNextWave(); 
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            CurrentWave.Draw(spriteBatch);
        }

        public WaveManager(Player player, Level level, int numberOfWaves, Texture2D enemyTexture)
        {
            this.numberOfWaves = numberOfWaves;
            this.enemyTexture = enemyTexture;

            this.level = level;

            for (int i = 0; i < numberOfWaves; i++)
            {
                int initialNumerOfEnemies = 6;
                int numberModifier = (i / 6) + 1;

                // Передать ссылку на игрока к классу волн
                Wave wave = new Wave(i, initialNumerOfEnemies *
                    numberModifier, player, level, enemyTexture);

                waves.Enqueue(wave);
            }

            StartNextWave();
        }

        private void StartNextWave()
        {
            if (waves.Count > 0) // стартуем новые волны
            {
                waves.Peek().Start(); // Start the next one

                timeSinceLastWave = 0; // Reset timer
                waveFinished = false;
            }

        }
        //END
    }
}
