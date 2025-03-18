using Raylib_cs;
using System;
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

    // Camera
    Camera2D camera;
    Vector2 cameraPos;
    float forwardSpeed = -25;

    // Game stats
    int score;
    int multiplier = 1;
    int kills;
    double roundTimer;
    double timer = 0;

    // Enemy Tyrian
    List<Enemy> enemyList = new List<Enemy>();
    List<Bullet> enemyBullets = new List<Bullet>();
    List<Vector2> enemySpawnLocations = new List<Vector2>()
    {
        // Boss location
        new Vector2(275, -400),

        new Vector2(100, -200), new Vector2(300, -200),
        new Vector2(100, -100), new Vector2(200, -100),
        new Vector2(300, -300), new Vector2(100, -300),
        new Vector2(200, -500), new Vector2(600, -500),
        new Vector2(200, -600), new Vector2(300, -600)
    };
    Boss boss1;

    // Sprites
    Texture2D playerImage;
    Texture2D bulletImage;
    Texture2D enemyImage;

    // Audio
    Sound shootSound;
    Sound explotionSound;
    Music backgroundMusic;

    // Other variables
    GameState state;
    bool win = false;

    /// <summary>
    /// Sets everything ready to start the game
    /// </summary>
    public void Start()
    {
        state = GameState.Play;

        Raylib.InitWindow(800, 600, "Avaruuspeli");
        Raylib.InitAudioDevice();

        // Image loading
        playerImage = Raylib.LoadTexture("Data/Images/newshf.shp.000000.png");
        bulletImage = Raylib.LoadTexture("Data/Images/tyrian.shp.000000.png");
        enemyImage = Raylib.LoadTexture("Data/Images/tyrian.shp.007D3C.png");

        // Audio loading
        shootSound = Raylib.LoadSound("Data/Audio/shotSound1.mp3");
        explotionSound = Raylib.LoadSound("Data/Audio/explosion1.mp3");
        backgroundMusic = Raylib.LoadMusicStream("Data/Audio/backgroundMusic1.mp3");
        Raylib.PlayMusicStream(backgroundMusic);

        // Volume adjusting
        Raylib.SetSoundVolume(shootSound, 0.6f);

        screenWidth = Raylib.GetScreenWidth();
        screenHeight = Raylib.GetScreenHeight();

        // Player stuff
        SetPlayer();
        score = 0;
        kills = 0;

        // Camera stuff
        cameraPos = new Vector2(0, 0);
        camera.Target = cameraPos;
        camera.Offset = new Vector2(0, 0);
        camera.Rotation = 0f;
        camera.Zoom = 1f;

        // Enemy stuff        
        AddEnemies();
        //boss1 = new Boss(new Rectangle(275, 5, 250, 250), 10, playerImage, false, new Rectangle(4, 112, 137, 112), 15, 1f);

        while (Raylib.WindowShouldClose() == false)
        {
            switch (state)
            {
                case GameState.Play:
                    Update();
                    Draw();
                    break;
                case GameState.ScoreScreen:
                    ScoreScreen(win);
                    break;
            }
        }

        // Unload pictures
        Raylib.UnloadTexture(playerImage);
        Raylib.UnloadTexture(bulletImage);
        Raylib.UnloadTexture(enemyImage);

        // Unload audio
        Raylib.UnloadSound(shootSound);
        Raylib.UnloadSound(explotionSound);
        Raylib.UnloadMusicStream(backgroundMusic);
        Raylib.CloseAudioDevice();

        Raylib.CloseWindow();
    }

    /// <summary>
    /// Updates the game
    /// </summary>
    private void Update()
    {
        if (boss1.active)
        {
            boss1.Movement();
            BossShoot(boss1);
        }

        player.Movement();
        player.KeepInsideScreen(screenWidth, screenHeight, cameraPos);

        camera.Target = cameraPos;
        cameraPos.Y += forwardSpeed * Raylib.GetFrameTime();
        //Console.WriteLine(cameraPos);

        foreach (Enemy enemy in enemyList) { enemy.Update(); }

        // Shoot
        if (Raylib.IsKeyPressed(KeyboardKey.Space)) { Shoot(player.transform, player.collision); }
        EnemyShoot(enemyList);

        // Game over
        if (Raylib.IsKeyPressed(KeyboardKey.Escape) || player.health <= 0)
        {
            roundTimer = Raylib.GetTime() - timer;
            timer = Raylib.GetTime();
            state = GameState.ScoreScreen;
        }
        // New round
        if (Raylib.IsKeyPressed(KeyboardKey.M) || boss1.health <= 0)
        {
            //RestartGame(true);
            roundTimer = Raylib.GetTime() - timer;
            timer = Raylib.GetTime();
            win = true;
            state = GameState.ScoreScreen;
        }

        // Background audio
        Raylib.UpdateMusicStream(backgroundMusic);
    }

    /// <summary>
    /// Draws the game
    /// </summary>
    private void Draw()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.Black);

        Raylib.BeginMode2D(camera);

        player.spriteRenderer.Draw();

        foreach (Enemy enemy in enemyList)
        {
            Vector2 enemyScreenPos = Raylib.GetWorldToScreen2D(enemy.transform.position, camera);

            if (enemyScreenPos.Y + enemy.collision.size.Y > cameraPos.Y && enemyScreenPos.Y < screenHeight
                && enemyScreenPos.X + enemy.collision.size.X > cameraPos.X && enemyScreenPos.X < screenWidth)
            {
                enemy.active = true;
                enemy.spriteRenderer.Draw();
            }
            else { enemy.active = false; }
        }

        Vector2 bossScreenPos = Raylib.GetWorldToScreen2D(boss1.transform.position, camera);
        Vector2 cameraScreenPos = Raylib.GetWorldToScreen2D(cameraPos, camera);
        if (bossScreenPos.Y + boss1.collision.size.Y > cameraPos.Y && bossScreenPos.Y < screenHeight)
        {
            boss1.spriteRenderer.Draw();
        }

        // Bullets are handled here because I have the bullets position changes and drawing
        // in the same method in bullet script
        HandleBullets(playerBullets, enemyList, boss1, screenHeight, true);
        HandleBullets(enemyBullets, enemyList, boss1, screenHeight, false);

        Raylib.EndMode2D();
        Raylib.EndDrawing();
    }

    /// <summary>
    /// Shoots a bullet (going upwards) from given objects location
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="collision"></param>
    void Shoot(Transform transform, Collision collision)
    {
        // Limit how fast can be shot again
        if (Raylib.GetTime() > shotTime + 0.5f)
        {
            Vector2 objectPos = transform.position;
            objectPos.X += collision.size.X / 2;
            playerBullets.Add(new Bullet(objectPos, new Vector2(12, 12), 200, bulletImage, false, new Rectangle(2, 42, 8, 11)));
            Raylib.PlaySound(shootSound);
            shotTime = Raylib.GetTime();
        }
    }

    /// <summary>
    /// Shoots a bullet from enemy in enemyList
    /// </summary>
    /// <param name="enemyList">A list of enemies</param>
    void EnemyShoot(List<Enemy> enemyList)
    {
        foreach (Enemy enemy in enemyList)
        {
            if (enemy.active && enemy.shooter && Raylib.GetTime() > enemy.lastShotTime + enemy.fireRate)
            {
                Vector2 bulletPos = enemy.transform.position + enemy.collision.size;
                bulletPos.X -= enemy.collision.size.X / 2;
                enemyBullets.Add(new Bullet(bulletPos, new Vector2(12, 12), -200, bulletImage, true, new Rectangle(2, 42, 8, 11)));
                Raylib.PlaySound(shootSound);
                enemy.lastShotTime = Raylib.GetTime();
            }
        }
    }

    void BossShoot(Boss boss)
    {
        if (boss.active && Raylib.GetTime() > boss.lastShotTime + boss.fireRate)
        {
            Vector2 bulletPos = boss.transform.position + boss.collision.size;
            bulletPos.X -= boss.collision.size.X / 2;
            enemyBullets.Add(new Bullet(new Vector2(bulletPos.X - 45, bulletPos.Y), 
                new Vector2(20, 20), -180, bulletImage, true, new Rectangle(2, 42, 8, 11)));
            enemyBullets.Add(new Bullet(new Vector2(bulletPos.X + 25, bulletPos.Y), 
                new Vector2(20, 20), -180, bulletImage, true, new Rectangle(2, 42, 8, 11)));
            Raylib.PlaySound(shootSound);
            boss.lastShotTime = Raylib.GetTime();
        }
    }

    /// <summary>
    /// Takes care of bullet in a given list
    /// </summary>
    /// <param name="bulletList"></param>
    /// <param name="enemyList"></param>
    /// <param name="screenHeight"></param>
    /// <param name="isPlayerShooting">Determines if we check does the bullet hit enemy or player</param>
    void HandleBullets(List<Bullet> bulletList, List<Enemy> enemyList, Boss boss, int screenHeight, bool isPlayerShooting)
    {
        foreach (Bullet bullet in bulletList)
        {
            // Drawing and moving
            bullet.Handler();

            // Remove bullets that go out of window
            if (bullet.transfrom.position.Y <= (cameraPos.Y - 10) || bullet.transfrom.position.Y >= (screenHeight + cameraPos.Y + 10))
            {
                bulletList.Remove(bullet);
                return;
            }

            // Player bullets
            if (isPlayerShooting == true)
            {
                // Checks if a bullet hits an enemy
                foreach (Enemy enemy in enemyList)
                {
                    if (enemy.active && Raylib.CheckCollisionRecs(bullet.spriterenderer.box, enemy.spriteRenderer.box))
                    {
                        bulletList.Remove(bullet);
                        enemyList.Remove(enemy);
                        Raylib.PlaySound(explotionSound);
                        IncreaseScore(100, multiplier);
                        kills++;
                        return;
                    }
                }

                if (boss.active && Raylib.CheckCollisionRecs(bullet.spriterenderer.box, boss.spriteRenderer.box))
                {
                    bulletList.Remove(bullet);
                    boss.health--;
                    if (boss.health <= 0) { boss.active = false; }
                    Raylib.PlaySound(explotionSound);
                    return;
                }
            }
            // Enemy bullets
            else
            {
                // Checks if the bullet hits the player
                if (Raylib.CheckCollisionRecs(bullet.spriterenderer.box, player.spriteRenderer.box))
                {
                    bulletList.Remove(bullet);
                    Raylib.PlaySound(explotionSound);
                    player.health--;
                    return;
                }
            }
        }
    }

    /// <summary>
    /// Adds enemies to the level
    /// </summary>
    void AddEnemies()
    {
        foreach (Vector2 spot in enemySpawnLocations)
        {
            if (spot == enemySpawnLocations[0])
            {
                boss1 = new Boss(new Rectangle(spot, 250, 250), 20, playerImage, false, new Rectangle(4, 112, 137, 112), 15, 2.5f);
            }
            else
            {
                int type = new Random().Next(3);
                enemyList.Add(SetEnemy(spot, type));
            }
        }
    }

    /// <summary>
    /// Restarts the game
    /// </summary>
    void RestartGame(bool isNewLevel)
    {
        playerBullets = new List<Bullet>();
        enemyBullets = new List<Bullet>();
        enemyList = new List<Enemy>();
        state = GameState.Play;

        SetPlayer();

        cameraPos = new Vector2(0, 0);

        // If new level starts don't reset stats
        if (!isNewLevel)
        {
            score = 0;
            multiplier = 1;
            kills = 0;
        }

        win = false;

        AddEnemies();
    }

    /// <summary>
    /// Increases score
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="multiplier"></param>
    void IncreaseScore(int amount, int multiplier)
    {
        if (multiplier == 0) { multiplier = 1; }
        score += amount * multiplier;
    }

    /// <summary>
    /// Show player stats from the last run
    /// </summary>
    void ScoreScreen(bool win)
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.Black);

        if (win)
        {
            Vector2 gameoverTextSize = Raylib.MeasureTextEx(Raylib.GetFontDefault(), $"Victory", 70, 3);
            Raylib.DrawTextEx(Raylib.GetFontDefault(), $"Victory",
                new Vector2(screenWidth / 2 - gameoverTextSize.X / 2, screenHeight / 6), 70, 3, Color.Blue);
        }
        else
        {
            Vector2 gameoverTextSize = Raylib.MeasureTextEx(Raylib.GetFontDefault(), $"Game Over", 70, 3);
            Raylib.DrawTextEx(Raylib.GetFontDefault(), $"Game Over", 
                new Vector2(screenWidth / 2 - gameoverTextSize.X / 2, screenHeight / 6), 70, 3, Color.Red);
        }

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

        if (Raylib.GetTime() > timer + 1.5f && Raylib.GetKeyPressed() != 0) { RestartGame(false); }

        Raylib.EndDrawing();
    }

    /// <summary>
    /// Easy way to set player
    /// </summary>
    void SetPlayer()
    {
        player = new Player(new Vector2(screenWidth / 2, screenHeight * 0.85f),
            new Vector2(30, 30), 150, playerImage, true, new Rectangle(26, 0, 24, 26),
            new Vector2(0, forwardSpeed));
    }

    /// <summary>
    /// Creates different types of enemies
    /// </summary>
    /// <param name="spot">Where to place</param>
    /// <param name="type">A number that gives the enemy type. Currently numbers go from 0 to 2</param>
    /// <returns>Return a new enemy</returns>
    Enemy SetEnemy(Vector2 spot, int type)
    {
        // Mover
        if (type == 0)
        {
            return new Enemy(new Rectangle(spot, 30, 30), 25, enemyImage, false,
            new Rectangle(27, 202, 15, 21), new Vector2(0, forwardSpeed), true, false, 0f);
        } 
        // Shooter
        else if (type == 1)
        {
            return new Enemy(new Rectangle(spot, 25, 25), 25, enemyImage, false,
            new Rectangle(27, 202, 15, 21), new Vector2(0, forwardSpeed), false, true, 2.5f);
        }
        // Advanced shooter
        else if (type == 2)
        {
            return new Enemy(new Rectangle(spot, 20, 20), 25, enemyImage, false,
            new Rectangle(27, 202, 15, 21), new Vector2(0, forwardSpeed), true, true, 1.5f);
        }
        // Debug tank
        else
        {
            return new Enemy(new Rectangle(spot, 45, 45), 20, enemyImage, false,
            new Rectangle(27, 202, 15, 21), new Vector2(0, forwardSpeed), false, false, 0f);
        }
    }
}
