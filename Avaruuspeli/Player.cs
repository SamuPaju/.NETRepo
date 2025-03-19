using Raylib_cs;
using System.Numerics;

namespace Avaruuspeli
{
    /// <summary>
    /// Player class that holds all the component that player needs
    /// </summary>
    internal class Player
    {
        public Transform transform;
        public Collision collision;
        public SpriteRenderer spriteRenderer;
        public int health;

        Vector2 velocity;
        Vector2 acceleration;
        float maxSpeed = 120;

        Rectangle movingLeftImage = new Rectangle(52, 0, 20, 26);
        Rectangle movingRightImage = new Rectangle(2, 0, 24, 26);
        Rectangle movingForwardImage = new Rectangle(26, 0, 24, 26);

        public Player(Vector2 position, Vector2 size, float speed, Texture2D sprite, bool rotate, Rectangle spriteSpot)
        {
            transform = new Transform(position, speed);
            collision = new Collision(size);
            spriteRenderer = new SpriteRenderer(transform, collision, sprite, rotate, spriteSpot);
            health = 3;
        }

        /// <summary>
        /// Takes movement input and moves the player
        /// </summary>
        /// <param name="levelSpeed">Speed of the level</param>
        public void Movement(Vector2 levelSpeed)
        {
            float time = Raylib.GetFrameTime();

            // Acceleration movement
            transform.direction = new Vector2(0, 0);

            // Change direction when a key is pressed
            if (Raylib.IsKeyDown(KeyboardKey.Left) || Raylib.IsKeyDown(KeyboardKey.A)) { transform.direction.X -= 1; }
            if (Raylib.IsKeyDown(KeyboardKey.Right) || Raylib.IsKeyDown(KeyboardKey.D)) { transform.direction.X += 1; }
            if (Raylib.IsKeyDown(KeyboardKey.Up) || Raylib.IsKeyDown(KeyboardKey.W)) { transform.direction.Y -= 1; }
            if (Raylib.IsKeyDown(KeyboardKey.Down) || Raylib.IsKeyDown(KeyboardKey.S)) { transform.direction.Y += 1; }

            // Slow movement if none of the keys are pressed
            if (transform.direction.X == 0) { velocity.X *= 0.9996f; }
            if (transform.direction.Y == 0) { velocity.Y *= 0.9996f; }

            // Calculate velocity
            acceleration = transform.direction * transform.speed + levelSpeed;
            velocity += acceleration * time;

            // Check that the velocity doesn't go way too high or low
            if (velocity.X < -maxSpeed) { velocity.X = -maxSpeed; }
            if (velocity.X > maxSpeed) { velocity.X = maxSpeed; }
            if (velocity.Y < -maxSpeed) { velocity.Y = -maxSpeed; }
            if (velocity.Y > maxSpeed) { velocity.Y = maxSpeed; }

            // Animate movement based on velocity amount
            if (velocity.X <= -20) { spriteRenderer.DrawAnimated(movingLeftImage); }
            else if (velocity.X >= 20) { spriteRenderer.DrawAnimated(movingRightImage); }
            else { spriteRenderer.DrawAnimated(movingForwardImage); }

            // Apply velocity to position
            transform.position += velocity * time;
        }

        /// <summary>
        /// Keeps player in the game area
        /// </summary>
        /// <param name="width">Screen width</param>
        /// <param name="height">Screen height</param>
        public void KeepInsideScreen(int width, int height, Vector2 cameraPos)
        {
            float x = transform.position.X;
            transform.position.X = Math.Clamp(x, 0, width - collision.size.X);
            
            float y = transform.position.Y;
            transform.position.Y = Math.Clamp(y, cameraPos.Y, height + cameraPos.Y - collision.size.Y);            
        }
    }
}
