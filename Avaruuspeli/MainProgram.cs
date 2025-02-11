using Raylib_cs;
using System;
using System.Numerics;

namespace Avaruuspeli;
public class MainProgram
{
    int screenWidth = 800;
    int screenHeight = 600;
    Player player = new Player(new Vector2(400, 500), 25, 100, Color.White);
    List<Bullet> playerBullets = new List<Bullet>();
    List<Enemy> enemies = new List<Enemy>();
    EnemyFormation enemyFormation = new EnemyFormation(new Vector2(0, 0), 0, 0);
    double shotTime = -1;

    public void Start()
    {
        Raylib.InitWindow(screenWidth, screenHeight, "Avruuspeli");

        AddEnemies();

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
        if (Raylib.GetTime() > shotTime + 2)
        {
            Vector2 playerPos = transform.position;
            playerPos.X += collision.size / 2;
            playerBullets.Add(new Bullet(playerPos, 10, 200, Color.Yellow));
            shotTime = Raylib.GetTime();
        }
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
    public void AddEnemies()
    {
        int spawnX = 0;
        int spawnY = 0;

        int rows = 6;
        int columns = 11;

        int enemySize = 25;
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

    public void ResizeEF(EnemyFormation ef, List<Enemy> enemies)
    {
        float left = enemies[0].transform.position.X;
        float right = enemies[0].transform.position.Y + enemies[0].collision.size;
        //float bottom = enemies[0].collision.size + enemies[0].transform.position.X;
        float top = enemies[0].transform.position.Y;

        foreach (Enemy enemy in enemies)
        {
            if (enemy.transform.position.X < left)
            {
                left = enemy.transform.position.X;
            }
            if (enemy.transform.position.Y + enemy.collision.size > right)
            {
                right = enemy.transform.position.Y;
            }
            if (enemy.transform.position.X < top)
            {
                top = enemy.collision.size;
            }
            /*if (enemy.collision.size + enemy.transform.position.Y > bottom)
            {
                bottom = enemy.collision.size;
            }*/
        }

        ef.transform.position = new Vector2(left, top);
        ef.collision.size = (int)(right - left);
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

    

