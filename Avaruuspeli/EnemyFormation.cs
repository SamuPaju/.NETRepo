using Raylib_cs;
using System.Numerics;

namespace Avaruuspeli
{
    public class EnemyFormation
    {
        public Transform transform;
        public Collision collision;
        public Rectangle enemyFormationRec;

        public EnemyFormation(Vector2 position, Vector2 size, float speed)
        {
            transform = new Transform(position, speed);
            collision = new Collision(transform, size);
            enemyFormationRec = new Rectangle((int)transform.position.X, (int)transform.position.Y,
            (int)collision.size.X, (int)collision.size.Y);
        }
    }
}
