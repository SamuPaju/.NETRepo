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

        public Player(Vector2 position, Vector2 size, float speed, Color color, Texture2D sprite, bool rotate, Rectangle spriteSpot)
        {
            transform = new Transform(position, speed);
            collision = new Collision(size);
            spriteRenderer = new SpriteRenderer(transform, collision, color, sprite, rotate, spriteSpot);
            health = 3;
        }
    }
}
