using Raylib_cs;
using System.Numerics;

namespace Avaruuspeli
{
    internal class Player
    {
        public Transform transform;
        public Collision collision;
        public SpriteRenderer spriteRenderer;
        public int health;

        public Player(Vector2 position, Vector2 size, float speed, Color color, Texture2D sprite, bool rotate, Rectangle spriteSpot)
        {
            transform = new Transform(position, speed);
            collision = new Collision(transform, size);
            spriteRenderer = new SpriteRenderer(transform, collision, color, sprite, rotate, spriteSpot);
            health = 3;
        }
    }
}
