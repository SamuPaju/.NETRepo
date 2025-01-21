using Raylib_cs;
using System.Numerics;

namespace Avaruuspeli;
public class MainProgram
{
    int screenWidth = 800;
    int screenHeight = 600;
    Player player = new Player(new Vector2(400, 500), 50, 100, Color.White);
    Bullet bullet = new Bullet(new Vector2(400, 495), 20, 200, Color.Yellow);
    
    public void Start()
    {
        Raylib.InitWindow(screenWidth, screenHeight, "Avruuspeli");
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
            bullet.transfrom.position = new Vector2(400, 495);
        }
        bullet.Handler();
        
    }

    public void Shoot()
    {
        Vector2 playerPos = player.transform.position;
        
    }

    public void Draw()
    {
        player.spriteRenderer.Draw();
    }

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

    

