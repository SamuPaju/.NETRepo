using Raylib_cs;
using System.Numerics;

namespace Avaruuspeli;

enum GameState
{
    Play,
    ScoreScreen
}

class MainProgram
{
    // Window
    int screenWidth;
    int screenHeight;

    // Player
    Player player;
    double shotTime = -1;
    List<Bullet> playerBullets = new List<Bullet>();
    int score;
    int multiplier = 1;
    int kills;
    double roundTimer;
    double timer = 0;

    // Enemy
    List<Enemy> enemies = new List<Enemy>();
    double enemyShotTime = -1;
    List<Bullet> enemyBullets = new List<Bullet>();
    float enemySpeed = 20;
    EnemyFormation enemyFormation;

    // Other variables
    GameState state;

    public void Start()
    {
        state = GameState.Play;

        Raylib.InitWindow(800, 600, "Avaruuspeli");

        screenWidth = Raylib.GetScreenWidth();
        screenHeight = Raylib.GetScreenHeight();

        player = new Player(new Vector2(screenWidth / 2, screenHeight * 0.85f), new Vector2(25, 25), 100, Color.White);
        score = 0;
        kills = 0;

        AddEnemies(5, 10);
        enemyFormation = new EnemyFormation(new Vector2(0, 0), new Vector2(0, 0), enemySpeed);
        ResizeEF(enemyFormation, enemies);

        while (Raylib.WindowShouldClose() == false)
        {
            switch (state)
            {
                case GameState.Play:
                    Update();
                    Draw();
                    break;
                case GameState.ScoreScreen:
                    ScoreScreen();
                    break;
            }
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
        EnemyShoot(enemies, enemyFormation);

        if (Raylib.IsKeyPressed(KeyboardKey.P/*Escape*/) || player.health <= 0)
        {
            roundTimer = Raylib.GetTime() - timer;
            timer = Raylib.GetTime();
            state = GameState.ScoreScreen;
        }

        if (Raylib.IsKeyPressed(KeyboardKey.M) || enemies.Count <= 0)
        {
            RestartGame(true);
        }
    }

    private void Draw()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.Black);

        player.spriteRenderer.Draw();

        // A rectangle to show where the EnemyFormations edges are
        //Raylib.DrawRectangle((int)enemyFormation.transform.position.X, (int)enemyFormation.transform.position.Y, 
        //    (int)enemyFormation.collision.size.X, (int)enemyFormation.collision.size.Y, Color.Green);


        foreach (Enemy enemy in enemies)
        {
            enemy.spriteRenderer.Draw();
        }

        HandleBullets(playerBullets, enemies, screenHeight, true);
        HandleBullets(enemyBullets, enemies, screenHeight, false);

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
    /// Makes enemies shoot
    /// </summary>
    /// <param name="enemyList"></param>
    /// <param name="EF">EnemyFormation</param>
    public void EnemyShoot(List<Enemy> enemyList, EnemyFormation EF)
    {
        if (Raylib.GetTime() > enemyShotTime + 2)
        {
            int randomEnemy = new Random().Next(0, enemyList.Count());
            Vector2 bulletPos = new Vector2(enemyList[randomEnemy].transform.position.X, EF.transform.position.Y + EF.collision.size.Y);
            bulletPos.X += enemyList[randomEnemy].collision.size.X / 2;
            enemyBullets.Add(new Bullet(bulletPos, new Vector2(10, 10), -200, Color.Yellow));
            enemyShotTime = Raylib.GetTime();
        }
    }

    /// <summary>
    /// Takes care of bullet in a given list
    /// </summary>
    /// <param name="bulletList"></param>
    /// <param name="enemyList"></param>
    /// <param name="screenHeight"></param>
    /// <param name="isPlayerShooting"></param>
    public void HandleBullets(List<Bullet> bulletList, List<Enemy> enemyList, int screenHeight, bool isPlayerShooting)
    {
        foreach (Bullet bullet in bulletList)
        {
            bullet.Handler();

            // Removes bullets that go out of window
            if (bullet.transfrom.position.Y <= -10 || bullet.transfrom.position.Y >= screenHeight + 10)
            {
                bulletList.Remove(bullet);
                return;
            }

            if (isPlayerShooting == true)
            {
                // Checks if a bullet hits an enemy
                foreach (Enemy enemy in enemyList)
                {
                    if (Raylib.CheckCollisionRecs(bullet.spriterenderer.box, enemy.spriteRenderer.box))
                    {
                        bulletList.Remove(bullet);
                        enemyList.Remove(enemy);
                        IncreaseScore(100, multiplier);
                        kills++;
                        ResizeEF(enemyFormation, enemyList);
                        return;
                    }
                }
            }
            else
            {
                if (Raylib.CheckCollisionRecs(bullet.spriterenderer.box, player.spriteRenderer.box))
                {
                    bulletList.Remove(bullet);
                    player.health--;
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
        int spawnX = 1;
        int spawnY = 0;

        int rows = enemyRows;
        int columns = enemyColumns;

        int enemySize = 25;
        int spaceBetween = 30;

        for (int row = 0; row < rows; row++)
        {
            spawnX = 1;
            for (int column = 0; column < columns; column++)
            {
                Vector2 spawnPos = new Vector2(spawnX, spawnY);
                enemies.Add(new Enemy(spawnPos, new Vector2(enemySize, enemySize), enemySpeed, Color.Red));
                spawnX += enemySize + spaceBetween;
            }
            spawnY += enemySize + spaceBetween;
        }
    }

    /// <summary>
    /// Handles enemies and the EnemyFormation
    /// </summary>
    /// <param name="enemyList"></param>
    /// <param name="EF">EnemyFormation</param>
    /// <param name="screenWidth"></param>
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
                enemy.transform.position.Y += 25;
            }
            ResizeEF(EF, enemyList);
        }
        if (EF.transform.position.X <= 0)
        {
            EF.transform.position.X += 1;
            EF.transform.speed *= -1;
            foreach (Enemy enemy in enemyList)
            {
                enemy.transform.position.X += 1;
                enemy.transform.speed *= -1;
                enemy.transform.position.Y += 25;
            }
            ResizeEF(EF, enemyList);
        }
    }

    /// <summary>
    /// Resizes EnemyFormation to include all the enemies
    /// </summary>
    /// <param name="ef">EnemyFormation</param>
    /// <param name="enemies"></param>
    public void ResizeEF(EnemyFormation ef, List<Enemy> enemies)
    {
        if (enemies.Count <= 0) { return; }

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
                top = enemy.transform.position.Y;
            }
            if (enemy.transform.position.Y + enemy.collision.size.Y > bottom)
            {
                bottom = enemy.transform.position.Y + enemy.collision.size.Y;
            }
        }
        // Sets the new position and size
        ef.transform.position = new Vector2(left, top);
        ef.collision.size = new Vector2(right - left, bottom - top);
    }

    /// <summary>
    /// Restarts the game
    /// </summary>
    public void RestartGame(bool isNewLevel)
    {
        playerBullets = new List<Bullet>();
        enemyBullets = new List<Bullet>();
        enemies = new List<Enemy>();
        state = GameState.Play;

        player = new Player(new Vector2(screenWidth / 2, screenHeight * 0.85f), new Vector2(25, 25), 100, Color.White);
        
        if (!isNewLevel)
        {
            score = 0;
            multiplier = 1;
            kills = 0;
        }

        AddEnemies(5, 10);
        enemyFormation = new EnemyFormation(new Vector2(0, 0), new Vector2(0, 0), enemySpeed);
        ResizeEF(enemyFormation, enemies);
    }

    /// <summary>
    /// Increases score
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="multiplier"></param>
    public void IncreaseScore(int amount, int multiplier)
    {
        if (multiplier == 0) { multiplier = 1; }
        score += amount * multiplier;
    }

    /// <summary>
    /// Show player stats from the last run
    /// </summary>
    public void ScoreScreen()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.Black);

        Vector2 gameoverTextSize = Raylib.MeasureTextEx(Raylib.GetFontDefault(), $"Game Over", 70, 3);
        Raylib.DrawTextEx(Raylib.GetFontDefault(), $"Game Over", 
            new Vector2(screenWidth / 2 - gameoverTextSize.X / 2, screenHeight / 6), 70, 3, Color.Red);

        Vector2 scoreTextSize = Raylib.MeasureTextEx(Raylib.GetFontDefault(), $"Score: {score}", 50, 3);
        Raylib.DrawTextEx(Raylib.GetFontDefault(), $"Score: {score}", 
            new Vector2(screenWidth / 2 - scoreTextSize.X / 2, screenHeight / 2 - 100), 50, 3, Color.White);
        
        Vector2 killsTextSize = Raylib.MeasureTextEx(Raylib.GetFontDefault(), $"Kills: {kills}", 50, 3);
        Raylib.DrawTextEx(Raylib.GetFontDefault(), $"Kills: {kills}", 
            new Vector2(screenWidth / 2 - killsTextSize.X / 2, screenHeight / 2 - 50), 50, 3, Color.White);
       
        Vector2 timerTextSize = Raylib.MeasureTextEx(Raylib.GetFontDefault(), $"Time: {Math.Round(roundTimer)}", 50, 3);
        Raylib.DrawTextEx(Raylib.GetFontDefault(), $"Time: {Math.Round(roundTimer)}", 
            new Vector2(screenWidth / 2 - timerTextSize.X / 2, screenHeight / 2), 50, 3, Color.White);

        Vector2 guideTextSize = Raylib.MeasureTextEx(Raylib.GetFontDefault(), $"Press any key to continue", 50, 3);
        Raylib.DrawTextEx(Raylib.GetFontDefault(), $"Press any key to continue",
            new Vector2(screenWidth / 2 - guideTextSize.X / 2, screenHeight / 2 + 100), 50, 3, Color.White);

        if (Raylib.GetKeyPressed() != 0) { RestartGame(false); }

        Raylib.EndDrawing();
    }
}
