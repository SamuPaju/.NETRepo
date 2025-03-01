using Raylib_cs;
using System.Data;
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

        public Player(Vector2 position, Vector2 size, float speed, Color color, Texture2D sprite, bool rotate, Rectangle spriteSpot)
        {
            transform = new Transform(position, speed);
            collision = new Collision(size);
            spriteRenderer = new SpriteRenderer(transform, collision, color, sprite, rotate, spriteSpot);
            health = 3;
        }

        /// <summary>
        /// Takes movement input and moves the player
        /// </summary>
        public void Movement()
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
        /// <param name="width">Screen width</param>
        /// <param name="height">Screen height</param>
        public void KeepInsideScreen(int width, int height)
        {
            float x = transform.position.X;
            transform.position.X = Math.Clamp(x, 0, width - collision.size.X);
            
            float y = transform.position.Y;
            transform.position.Y = Math.Clamp(y, 0, height - collision.size.Y);            
        }
    }
}
