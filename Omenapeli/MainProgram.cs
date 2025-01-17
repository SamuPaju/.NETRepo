using Raylib_cs;
using System.Numerics;


namespace Omenapeli;
public class MainProgram
{
    Player player = new Player(new Vector2(400, 600), 50, 0.5f, Color.Green);
	Apple apple = new Apple(new Vector2(400, 200), 60, 0, Color.Red);
    public int screen_width = 800;
	public int screen_height = 800;
	public int score = 0;
	
	public void Start() 
	{
        Raylib.InitWindow(screen_width, screen_height, "Omenapeli");
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
		ApplyKeyboardInput(player.transform);		// Change player velocity
		player.transform.Update();				// Add player velocity to position
		KeepInsideScreen(player.transform, player.collision, 
			0, 0, screen_width, screen_height);     // Keep player inside screen area
		Draw();
		HitApple();
	}

	public void Draw()
	{
		player.spriteRenderer.Draw();		// Draw player
		apple.spriteRenderer.Draw();
		Raylib.DrawText(score.ToString(), 20, 20, 15, Color.White);        
    }

	public void ApplyKeyboardInput(Transform transform)
	{
		if (Raylib.IsKeyDown(KeyboardKey.Right))
		{
			transform.velocity.X += transform.speed;
		}
		else if (Raylib.IsKeyDown(KeyboardKey.Left))
		{
            transform.velocity.X -= transform.speed;
        }
		else if (Raylib.IsKeyDown(KeyboardKey.Up))
		{
			transform.velocity.Y -= transform.speed;
		}
		else if (Raylib.IsKeyDown(KeyboardKey.Down))
		{
			transform.velocity.Y += transform.speed;
		}
	}

	public void KeepInsideScreen(Transform transform, Collision collision, int left, int top, int width, int heigth)
	{
		float x = transform.position.X;
		transform.position.X = Math.Clamp(x, left, width - collision.size);
		float y = transform.position.Y;
		transform.position.Y = Math.Clamp(y, top, heigth - collision.size);
	}

	public void HitApple()
	{
        if (Raylib.CheckCollisionRecs(player.spriteRenderer.box, apple.spriteRenderer.box))
		{
			ChangeAppleLocation(apple.transform);
		}
	}

	public void ChangeAppleLocation(Transform transform)
	{
		Random random = new Random();
		Vector2 newPosition = new Vector2(random.Next(screen_width-50), random.Next(screen_height-50));

		transform.position = newPosition;
		score++;
	}
}
