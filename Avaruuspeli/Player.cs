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

        public Player(Vector2 position, Vector2 size, float speed, Color color)
        {
            transform = new Transform(position, speed);
            collision = new Collision(transform, size);
            spriteRenderer = new SpriteRenderer(transform, collision, color);
            health = 3;
        }
    }
}
