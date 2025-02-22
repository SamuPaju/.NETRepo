using Raylib_cs;
using System.Numerics;

namespace Avaruuspeli
{
    class MainProgram
    {
        // Window
        int screenWidth;
        int screenHeight;

        // Player
        Player player;
        double shotTime = -1;
        List<Bullet> playerBullets = new List<Bullet>();

        // Enemy
        List<Enemy> enemies = new List<Enemy>();
        float enemySpeed = 10;
        EnemyFormation enemyFormation;

        public void Start()
        {
            Raylib.InitWindow(800, 600, "Avaruuspeli");

            screenWidth = Raylib.GetScreenWidth();
            screenHeight = Raylib.GetScreenHeight();

            player = new Player(new Vector2(screenWidth / 2, 500), new Vector2(25, 25), 100, Color.White);

            AddEnemies(5, 10);
            enemyFormation = new EnemyFormation(new Vector2(0, 0), new Vector2(0, 0), enemySpeed);
            ResizeEF(enemyFormation, enemies);

            while (Raylib.WindowShouldClose() == false)
            {
                Update();
                Draw();
            }
            Raylib.CloseWindow();
        }

        private void Update()
        {
            Movement(player.transform);
            KeepInsideScreen(player.transform, player.collision, screenWidth, screenHeight);
            if (Raylib.IsKeyPressed(KeyboardKey.Space))
            {
                Shoot(player.transform, player.collision);
            }
            EnemyHandler(enemies, enemyFormation, screenWidth);
        }

        private void Draw()
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);

            player.spriteRenderer.Draw();
            HandleBullets(playerBullets, enemies, screenHeight);

            Raylib.DrawRectangle((int)enemyFormation.transform.position.X, (int)enemyFormation.transform.position.Y,
                (int)enemyFormation.collision.size.X, (int)enemyFormation.collision.size.Y, Color.Green);

            foreach (Enemy enemy in enemies)
            {
                enemy.spriteRenderer.Draw();
            }

            Raylib.EndDrawing();
        }


        /// <summary>
        /// Makes the movement for player
        /// </summary>
        /// <param name="transform"></param>
        public void Movement(Transform transform)
        {
            if (Raylib.IsKeyDown(KeyboardKey.Left) || Raylib.IsKeyDown(KeyboardKey.A))
            {
                transform.position.X -= transform.speed * Raylib.GetFrameTime();
            }
            if (Raylib.IsKeyDown(KeyboardKey.Right) || Raylib.IsKeyDown(KeyboardKey.D))
            {
                transform.position.X += transform.speed * Raylib.GetFrameTime();
            }
        }

        /// <summary>
        /// Keeps player in the game area
        /// </summary>
        /// <param name="transfrom"></param>
        /// <param name="collision"></param>
        /// <param name="width">Screen width</param>
        /// <param name="height">Screen height</param>
        public void KeepInsideScreen(Transform transfrom, Collision collision, int width, int height)
        {
            float x = transfrom.position.X;
            transfrom.position.X = Math.Clamp(x, 0, width - collision.size.X);
            /*
            float y = transfrom.position.Y;
            transfrom.position.Y = Math.Clamp(y, 0, height - collision.size);
            */
        }


        /// <summary>
        /// Shoots a bullet from given objects location
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="collision"></param>
        public void Shoot(Transform transform, Collision collision)
        {
            if (Raylib.GetTime() > shotTime + 1)
            {
                Vector2 objectPos = transform.position;
                objectPos.X += collision.size.X / 2;
                playerBullets.Add(new Bullet(objectPos, new Vector2(10, 10), 200, Color.Yellow));
                shotTime = Raylib.GetTime();
            }
        }

        /// <summary>
        /// Takes care of bullet in a given list
        /// </summary>
        /// <param name="bulletList"></param>
        /// <param name="screenHeight"></param>
        public void HandleBullets(List<Bullet> bulletList, List<Enemy> enemyList, int screenHeight)
        {
            foreach (Bullet bullet in bulletList)
            {
                bullet.Handler();

                // Removes bullets that go out of window
                if (bullet.transfrom.position.Y <= -10 || bullet.transfrom.position.Y >= screenHeight + 10)
                {
                    playerBullets.Remove(bullet);
                    return;
                }

                // Checks if a bullet hits an enemy
                foreach (Enemy enemy in enemyList)
                {
                    if (Raylib.CheckCollisionRecs(bullet.spriterenderer.box, enemy.spriteRenderer.box))
                    {
                        playerBullets.Remove(bullet);
                        enemies.Remove(enemy);
                        ResizeEF(enemyFormation, enemyList);
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Adds enemies to the window
        /// </summary>
        /// <param name="enemyRows"></param>
        /// <param name="enemyColumns"></param>
        public void AddEnemies(int enemyRows, int enemyColumns)
        {
            int spawnX = 0;
            int spawnY = 0;

            int rows = enemyRows;
            int columns = enemyColumns;

            int enemySize = 25;
            int spaceBetween = 25;

            for (int row = 0; row < rows; row++)
            {
                spawnX = 0;
                for (int column = 0; column < columns; column++)
                {
                    Vector2 spawnPos = new Vector2(spawnX, spawnY);
                    enemies.Add(new Enemy(spawnPos, new Vector2(enemySize, enemySize), enemySpeed, Color.Red));
                    spawnX += enemySize + spaceBetween;
                }
                spawnY += enemySize + spaceBetween;
            }
        }


        public void EnemyHandler(List<Enemy> enemyList, EnemyFormation EF, int screenWidth)
        {
            EF.transform.position.X += EF.transform.speed * Raylib.GetFrameTime();
            foreach (Enemy enemy in enemyList)
            {
                enemy.transform.position.X += enemy.transform.speed * Raylib.GetFrameTime();
            }

            if (EF.transform.position.X + EF.collision.size.X >= screenWidth)
            {
                EF.transform.position.X -= 1;
                EF.transform.speed *= -1;
                foreach (Enemy enemy in enemyList)
                {
                    enemy.transform.position.X -= 1;
                    enemy.transform.speed *= -1;
                }
            }
            if (EF.transform.position.X <= 0)
            {
                EF.transform.position.X += 1;
                EF.transform.speed *= -1;
                foreach (Enemy enemy in enemyList)
                {
                    enemy.transform.position.X += 1;
                    enemy.transform.speed *= -1;
                }
            }
        }

        /// <summary>
        /// Resizes EnemyFormation to include all the enemies
        /// </summary>
        /// <param name="ef">EnemyFormation</param>
        /// <param name="enemies"></param>
        public void ResizeEF(EnemyFormation ef, List<Enemy> enemies)
        {
            float left = enemies[0].transform.position.X;
            float right = enemies[0].transform.position.X;
            float top = enemies[0].transform.position.Y;
            float bottom = enemies[0].transform.position.Y;

            foreach (Enemy enemy in enemies)
            {
                // Looks for all the sides
                if (enemy.transform.position.X < left)
                {
                    left = enemy.transform.position.X;
                }
                if (enemy.transform.position.X + enemy.collision.size.X > right)
                {
                    right = enemy.transform.position.X + enemy.collision.size.X;
                }
                if (enemy.transform.position.Y < top)
                {
                    //top = enemy.collision.size.Y;
                    top = enemy.transform.position.Y;
                }
                if (enemy.transform.position.Y + enemy.collision.size.Y > bottom)
                {
                    bottom = enemy.transform.position.Y + enemy.collision.size.Y;
                }
            }
            // Sets the new position and size
            ef.transform.position = new Vector2(left, top);
            ef.collision.size = new Vector2(right, bottom);
        }
    }
}
