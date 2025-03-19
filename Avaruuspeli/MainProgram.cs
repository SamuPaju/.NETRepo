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
    Player player2;
    bool twoPlayer = true;
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
    Boss boss;

    // Level
    List<Vector2> level1SpawnLocations = new List<Vector2>()
    {
        // Boss location
        new Vector2(275, -850),

        new Vector2(100, -200), new Vector2(300, -200),
        new Vector2(100, -100), new Vector2(200, -100),
        new Vector2(300, -300), new Vector2(100, -300),
        new Vector2(200, -500), new Vector2(600, -500),
        new Vector2(200, -600), new Vector2(300, -600)
    };
    List<Vector2> level2SpawnLocations = new List<Vector2>()
    {
        // Boss location
        new Vector2(275, -750),

        new Vector2(50, -100), new Vector2(100, -100),
        new Vector2(150, -100), new Vector2(200, -100),
        new Vector2(700, -250), new Vector2(100, -300),
        new Vector2(200, -400), new Vector2(600, -400),
        new Vector2(200, -500), new Vector2(300, -500)
    };
    List<Vector2>[] levelArray;
    int levelIndex = 0;

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

        // Level stuff
        levelArray = new List<Vector2>[] { level1SpawnLocations, level2SpawnLocations };

        // Enemy stuff        
        AddEnemies(levelArray[levelIndex]);

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
        //player.Movement(new Vector2(0, forwardSpeed));
        player.KeepInsideScreen(screenWidth, screenHeight, cameraPos);
        PlayerEnemyCollision(player, enemyList, boss);
        if (twoPlayer)
        {
            player2.KeepInsideScreen(screenWidth, screenHeight, cameraPos);
            PlayerEnemyCollision(player2, enemyList, boss);
        }

        camera.Target = cameraPos;
        cameraPos.Y += forwardSpeed * Raylib.GetFrameTime();

        foreach (Enemy enemy in enemyList) { enemy.Update(); }

        if (boss.active)
        {
            boss.Movement();
            BossShoot(boss);
            forwardSpeed = 0f;
        }

        // Shoot
        if (Raylib.IsKeyPressed(KeyboardKey.Space) && player.alive) { Shoot(player.transform, player.collision); }
        if (twoPlayer && Raylib.IsKeyPressed(KeyboardKey.M) && player2.alive) { Shoot(player2.transform, player2.collision); }
        EnemyShoot(enemyList);

        // Game over
        if (twoPlayer)
        {
            if (Raylib.IsKeyPressed(KeyboardKey.P) || !player.alive && !player2.alive)
            {
                roundTimer = Raylib.GetTime() - timer;
                timer = Raylib.GetTime();
                win = false;
                state = GameState.ScoreScreen;
            }
        }
        else
        {
            if (Raylib.IsKeyPressed(KeyboardKey.P) || !player.alive)
            {
                roundTimer = Raylib.GetTime() - timer;
                timer = Raylib.GetTime();
                win = false;
                state = GameState.ScoreScreen;
            }
        }
        // New round
        if (Raylib.IsKeyPressed(KeyboardKey.N) || boss.health <= 0)
        {
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
    void Draw()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.Black);

        Raylib.BeginMode2D(camera);
;
        player.Movement(new Vector2(0, forwardSpeed));       
        if (twoPlayer) { player2.Movement(new Vector2(0, forwardSpeed)); }

        // Activate and draw all the enemies on the screen
        foreach (Enemy enemy in enemyList)
        {
            Vector2 enemyScreenPos = Raylib.GetWorldToScreen2D(enemy.transform.position, camera);

            if (enemyScreenPos.Y + enemy.collision.size.Y > 0 && enemyScreenPos.Y < screenHeight
                && enemyScreenPos.X + enemy.collision.size.X > 0 && enemyScreenPos.X < screenWidth)
            {
                enemy.active = true;
                enemy.spriteRenderer.Draw();
            }
            else { enemy.active = false; }
        }

        // Draw the boss when it's partially on the screen and activate it when it's fully on the screen 
        Vector2 bossScreenPos = Raylib.GetWorldToScreen2D(boss.transform.position, camera);
        if (bossScreenPos.Y + boss.collision.size.Y > 0 && bossScreenPos.Y < screenHeight) { boss.spriteRenderer.Draw(); }
        if (bossScreenPos.Y > 0) { boss.active = true; }

        // Bullets are handled here because I have the bullets position changes and drawing
        // in the same method in bullet script
        HandleBullets(playerBullets, enemyList, boss, screenHeight, true);
        HandleBullets(enemyBullets, enemyList, boss, screenHeight, false);

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
                if (player.alive && Raylib.CheckCollisionRecs(bullet.spriterenderer.box, player.spriteRenderer.box))
                {
                    bulletList.Remove(bullet);
                    Raylib.PlaySound(explotionSound);
                    player.health--;
                    return;
                }
                if (twoPlayer && player2.alive && Raylib.CheckCollisionRecs(bullet.spriterenderer.box, player2.spriteRenderer.box))
                {
                    bulletList.Remove(bullet);
                    Raylib.PlaySound(explotionSound);
                    player2.health--;
                    return;
                }
            }
        }
    }

    /// <summary>
    /// Adds enemies and boss to the level
    /// </summary>
    void AddEnemies(List<Vector2> levelSpawnLocations)
    {
        foreach (Vector2 spot in levelSpawnLocations)
        {
            if (spot == levelSpawnLocations[0])
            {
                boss = new Boss(new Rectangle(spot, 250, 250), 20, playerImage, false, new Rectangle(4, 112, 137, 112), 15, 2.5f);
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
        // Reset all the lists and gamestate
        playerBullets = new List<Bullet>();
        enemyBullets = new List<Bullet>();
        enemyList = new List<Enemy>();
        state = GameState.Play;

        SetPlayer();

        // Set camera at the start
        cameraPos = new Vector2(0, 0);

        // If new level starts don't reset stats
        if (!isNewLevel)
        {
            score = 0;
            multiplier = 1;
            kills = 0;
            levelIndex = 0;
        }
        // Set next level
        else 
        { 
            levelIndex++; 
            if (levelIndex >= levelArray.Length) { levelIndex = 0; }
        }

        AddEnemies(levelArray[levelIndex]);
    }

    /// <summary>
    /// Increases score
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="multiplier"></param>
    void IncreaseScore(int amount, int multiplier)
    {
        // If multiplier is 0 set it to 1
        if (multiplier == 0) { multiplier = 1; }
        // Apply score
        score += amount * multiplier;
    }

    /// <summary>
    /// Show player stats from the last run
    /// </summary>
    void ScoreScreen(bool win)
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.Black);

        // Win text
        if (win)
        {
            Vector2 gameoverTextSize = Raylib.MeasureTextEx(Raylib.GetFontDefault(), $"Victory", 70, 3);
            Raylib.DrawTextEx(Raylib.GetFontDefault(), $"Victory",
                new Vector2(screenWidth / 2 - gameoverTextSize.X / 2, screenHeight / 6), 70, 3, Color.Blue);
        }
        // Game over text
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

        // Waits a little while until lets player go forward
        if (Raylib.GetTime() > timer + 1.5f && Raylib.GetKeyPressed() != 0) { RestartGame(win); }

        Raylib.EndDrawing();
    }

    /// <summary>
    /// Easy way to set player
    /// </summary>
    void SetPlayer()
    {      
        if (twoPlayer) 
        {
            // Set both players
            player = new Player(new Vector2(screenWidth / 2 - 80, screenHeight * 0.85f),
                new Vector2(30, 30), 150, playerImage, true, new Rectangle(26, 0, 24, 26), 1);
            player2 = new Player(new Vector2(screenWidth / 2 + 80, screenHeight * 0.85f),
                new Vector2(30, 30), 150, playerImage, true, new Rectangle(26, 0, 24, 26), 2);
        }
        else
        {
            // Set only one player
            player = new Player(new Vector2(screenWidth / 2, screenHeight * 0.85f),
                new Vector2(30, 30), 150, playerImage, true, new Rectangle(26, 0, 24, 26), 1);
        }
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

    /// <summary>
    /// Makes player take damage if collides with an enemy
    /// </summary>
    /// <param name="enemyList"></param>
    /// <param name="boss"></param>
    void PlayerEnemyCollision(Player player, List<Enemy> enemyList, Boss boss)
    {
        // Check if player is alive
        if (!player.alive) { return; }

        // Goes through all the enemies and check if they collide with a player
        foreach (Enemy enemy in enemyList)
        {
            if (Raylib.CheckCollisionRecs(player.spriteRenderer.box, enemy.spriteRenderer.box))
            {
                enemyList.Remove(enemy);
                Raylib.PlaySound(explotionSound);
                IncreaseScore(50, multiplier);
                kills++;
                player.health--;
                return;
            }
        }

        // Instantly kills player if player has less health than boss. Otherwise boss dies.
        if (Raylib.CheckCollisionRecs(player.spriteRenderer.box, boss.spriteRenderer.box))
        {
            player.health--;
            boss.health--;
            Raylib.PlaySound(explotionSound);
        }
    }
}
