using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game1
{
    class SpikeTower : Tower
    {
        // Список направлений для стрельбы
        private Vector2[] directions = new Vector2[8];
        // Все враги в радиусе стрельбы
        private List<Enemy> targets = new List<Enemy>();

        /// <summary>
        /// Constructs a new Spike Tower object.
        /// </summary>
        public SpikeTower(Texture2D texture, Texture2D bulletTexture, Vector2 position)
            : base(texture, bulletTexture, position)
        {
            this.damage = 20;
            this.cost = 40;  

            this.radius = 48; 
            
            directions = new Vector2[]
            {
               new Vector2(-1, -1), // North West
               new Vector2( 0, -1), // North
               new Vector2( 1, -1), // North East
               new Vector2(-1,  0), // West
               new Vector2( 1,  0), // East
               new Vector2(-1,  1), // South West
               new Vector2( 0,  1), // South
               new Vector2( 1,  1), // South East
            };
        }

        public override void Update(GameTime gameTime)
        {
            bulletTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Настало время палить!!!
            if (bulletTimer >= 1.0f && targets.Count != 0)
            {
                // Пуляй по всем
                for (int i = 0; i < directions.Length; i++)
                {
                    // пали пали пали
                    Bullet bullet = new Bullet(bulletTexture, Vector2.Subtract(center,
                        new Vector2(bulletTexture.Width / 2)), directions[i], 6, damage);

                    bulletList.Add(bullet);
                }

                bulletTimer = 0;
            }

            // проходим через все пули
            for (int i = 0; i < bulletList.Count; i++)
            {
                Bullet bullet = bulletList[i];
                bullet.Update(gameTime);

                // убить пули за пределами радиуса стрельбы
                if (!IsInRange(bullet.Center))
                {
                    bullet.Kill();
                }

                // проход по всем целям
                for (int t = 0; t < targets.Count; t++)
                {
                    // если в рдиусе то ПАЛИ ПАЛИ!
                    if (targets[t] != null && Vector2.Distance(bullet.Center, targets[t].Center) < 12)
                    {
                        // раз удар и два удар
                        targets[t].CurrentHealth -= bullet.Damage;
                        bullet.Kill();

                        // пуля ломается после попадания
                        break;
                    }
                }

                // Remove the bullet if it is dead.
                if (bullet.IsDead())
                {
                    bulletList.Remove(bullet);
                    i--;
                }
            }
        }

        public override bool HasTarget
        {
            // Башня никогда не будет с одной целью
            get { return false; }
        }

        public override void GetClosestEnemy(List<Enemy> enemies)
        {
            // чистим дуло
            targets.Clear();

            // прогоняем по всем врагам
            foreach (Enemy enemy in enemies)
            {
                // если в радиусе выстрела, то в список на отстрел
                if (IsInRange(enemy.Center))
                {
                    
                    targets.Add(enemy);
                }
            }
        }
//END
    }
}
