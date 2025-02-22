using System.Numerics;

namespace Avaruuspeli
{
    public class EnemyFormation
    {
        public Transform transform;
        public Collision collision;

        public EnemyFormation(Vector2 position, Vector2 size, float speed)
        {
            transform = new Transform(position, speed);
            collision = new Collision(transform, size);
        }
    }
}
