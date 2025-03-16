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
            collision = new Collision(size);
            enemyFormationRec = new Rectangle((int)transform.position.X, (int)transform.position.Y,
            (int)collision.size.X, (int)collision.size.Y);
        }
    }


    /*
     
     /// <summary>
    /// Resizes EnemyFormation to include all the enemies
    /// </summary>
    /// <param name="ef">EnemyFormation</param>
    /// <param name="enemies"></param>
    public void ResizeEF(EnemyFormation ef, List<EnemyT> enemies)
    {
        // Check if there are any enemies left
        if (enemies.Count <= 0) { return; }

        // Set the variables to the first enemy on the list so that the left and top variables work
        float left = enemies[0].transform.position.X;
        float right = enemies[0].transform.position.X;
        float top = enemies[0].transform.position.Y;
        float bottom = enemies[0].transform.position.Y;

        foreach (EnemyT enemy in enemies)
        {
            // Looks for all the side positions
            if (enemy.transform.position.X < left)
            {
                left = enemy.transform.position.X;
            }
            if (enemy.transform.position.X + enemy.collision.size.X > right)
            {
                right = enemy.transform.position.X + enemy.collision.size.X;
            }
            if (enemy.transform.position.Y < top)
            {
                top = enemy.transform.position.Y;
            }
            if (enemy.transform.position.Y + enemy.collision.size.Y > bottom)
            {
                bottom = enemy.transform.position.Y + enemy.collision.size.Y;
            }
        }
        // Sets the new position and size
        ef.transform.position = new Vector2(left, top);
        ef.collision.size = new Vector2(right - left, bottom - top);
    }
     
     */
}
