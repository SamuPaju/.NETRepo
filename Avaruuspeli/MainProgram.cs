using Raylib_cs;
using System;
using System.Numerics;

namespace Avaruuspeli;
public class MainProgram
{
    int screenWidth = 800;
    int screenHeight = 600;
    Player player = new Player(new Vector2(400, 500), 50, 100, Color.White);
    List<Bullet> playerBullets = new List<Bullet>();
    List<Enemy> enemies = new List<Enemy>();
    
    public void Start()
    {
        Raylib.InitWindow(screenWidth, screenHeight, "Avruuspeli");

        AddEnemys();

        while (Raylib.WindowShouldClose() == false)
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);

            
            Update();

            Raylib.EndDrawing();
        }
        Raylib.CloseWindow();
    }

    public void Update()
    {
        Movement(player.transform);
        KeepInsideScreen(player.transform, player.collision, screenWidth, screenHeight);
        Draw();


        if (Raylib.IsKeyPressed(KeyboardKey.Space))
        {
            Shoot(player.transform, player.collision);
        }
        HandleBullets();
        EnemyHandle();
    }

    /// <summary>
    /// Shoots a bullet from given objects location
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="collision"></param>
    public void Shoot(Transform transform, Collision collision)
    {
        Vector2 playerPos = transform.position;
        playerPos.X += collision.size / 4;
        playerBullets.Add(new Bullet(playerPos, 20, 200, Color.Yellow));
    }

    /// <summary>
    /// Handles playerBullets
    /// </summary>
    public void HandleBullets()
    {
        foreach (Bullet bullet in playerBullets)
        {
            bullet.Handler();

            foreach (Enemy enemy in enemies)
            {
                if (Raylib.CheckCollisionRecs(bullet.spriterenderer.box, enemy.spriteRenderer.box))
                {
                    playerBullets.Remove(bullet);
                    enemies.Remove(enemy);
                    return;
                }
            }

            if (bullet.transfrom.position.X <= -10)
            {
                playerBullets.Remove(bullet);
                return;
            }
        }        
    }

    /// <summary>
    /// Adds enemies in the start
    /// </summary>
    /// <param name="sw"></param>
    /// <param name="sh"></param>
    public void AddEnemys()
    {
        int spawnX = 0;
        int spawnY = 0;

        int rows = 2;
        int columns = 4;

        int enemySize = 40;
        int spaceBetween = 20;

        for (int row = 0; row < rows; row++)
        {
            spawnX = 0;
            for (int column = 0; column < columns; column++)
            {
                Vector2 spawnPos = new Vector2(spawnX, spawnY);
                enemies.Add(new Enemy(spawnPos, enemySize, 5, Color.Red));
                spawnX += enemySize + spaceBetween;
            }
            spawnY += enemySize + spaceBetween;
        }
    }

    public void EnemyHandle()
    {
        // Coming later
    }

    public void Draw()
    {
        player.spriteRenderer.Draw();

        for (int i = 0;i < enemies.Count; i++)
        {
            enemies[i].spriteRenderer.Draw();
        }
    }

    /// <summary>
    /// Makes the movement for player
    /// </summary>
    /// <param name="transform"></param>
    public void Movement(Transform transform)
    {
        if (Raylib.IsKeyDown(KeyboardKey.Left))
        {
            transform.position.X -= transform.speed * Raylib.GetFrameTime();
        }
        if (Raylib.IsKeyDown(KeyboardKey.Right))
        {
            transform.position.X += transform.speed * Raylib.GetFrameTime();
        }
    }

    /// <summary>
    /// Keeps player in the game area
    /// </summary>
    /// <param name="transfrom"></param>
    /// <param name="collision"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public void KeepInsideScreen(Transform transfrom, Collision collision, int width, int height)
    {
        float x = transfrom.position.X;
        transfrom.position.X = Math.Clamp(x, 0, width - collision.size / 2);
        /*
        float y = transfrom.position.Y;
        transfrom.position.Y = Math.Clamp(y, 0, height - collision.size / 2);
        */
    }
}

    

