using Raylib_cs;
using System.Numerics;

namespace Avaruuspeli
{
    internal class Player
    {
        public Transform transform;
        public Collision collision;
        public SpriteRenderer spriteRenderer;

        public Player(Vector2 position, int size, float speed, Color color)
        {
            transform = new Transform(position, speed);
            collision = new Collision(transform, size);
            spriteRenderer = new SpriteRenderer(transform, collision, color);
        }
    }
}
