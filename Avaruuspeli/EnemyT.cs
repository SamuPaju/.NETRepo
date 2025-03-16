using System;
using System.Numerics;
using Raylib_cs;

namespace Avaruuspeli;

/// <summary>
/// Enemy class that holds all the component that enemy needs
/// </summary>
public class EnemyT
{
    public Transform transform;
    public Collision collision;
    public SpriteRenderer spriteRenderer;

    public EnemyT(Vector2 position, Vector2 size, float speed, Texture2D sprite, bool rotate, Rectangle spriteSpot)
	{
        transform = new Transform(position, speed);
        collision = new Collision(size);
        spriteRenderer = new SpriteRenderer(transform, collision, sprite, rotate, spriteSpot);
    }

    /* Useless (Just do it in the main program)
    public void ChangeDirection()
    {
        transform.speed *= -1;
    }*/

    /*
     
    /// <summary>
    /// Makes enemies shoot
    /// </summary>
    /// <param name="enemyList"></param>
    /// <param name="EF">EnemyFormation</param>
    public void EnemyShoot(List<EnemyT> enemyList, EnemyFormation EF)
    {
        if (Raylib.GetTime() > enemyShotTime + 2)
        {
            int randomEnemy = new Random().Next(0, enemyList.Count());
            Vector2 bulletPos = new Vector2(enemyList[randomEnemy].transform.position.X, EF.transform.position.Y + EF.collision.size.Y);
            bulletPos.X += enemyList[randomEnemy].collision.size.X / 2;
            enemyBullets.Add(new Bullet(bulletPos, new Vector2(12, 12), -200, bulletImage, true, new Rectangle(2, 42, 8, 11)));
            Raylib.PlaySound(shootSound);
            enemyShotTime = Raylib.GetTime();
        }
    }




    /// <summary>
    /// Adds enemies to the window
    /// </summary>
    /// <param name="enemyRows"></param>
    /// <param name="enemyColumns"></param>
    public void AddEnemies(int enemyRows, int enemyColumns)
    {
        int spawnX = 1;
        int spawnY = 0;

        int rows = enemyRows;
        int columns = enemyColumns;

        int enemySize = 25;
        int spaceBetween = 30;

        for (int row = 0; row < rows; row++)
        {
            spawnX = 1;
            for (int column = 0; column < columns; column++)
            {
                Vector2 spawnPos = new Vector2(spawnX, spawnY);
                enemies.Add(new EnemyT(spawnPos, new Vector2(enemySize, enemySize), 
                    enemySpeed, enemyImage, false, new Rectangle(27,202,15,21)));
                spawnX += enemySize + spaceBetween;
            }
            spawnY += enemySize + spaceBetween;
        }
    }
     





    /// <summary>
    /// Handles enemies and the EnemyFormation
    /// </summary>
    /// <param name="enemyList"></param>
    /// <param name="EF">EnemyFormation</param>
    /// <param name="screenWidth"></param>
    public void EnemyHandler(List<EnemyT> enemyList, EnemyFormation EF, int screenWidth)
    {
        int downMovement = 50;

        EF.transform.position.X += EF.transform.speed * Raylib.GetFrameTime();
        foreach (EnemyT enemy in enemyList)
        {
            enemy.transform.position.X += enemy.transform.speed * Raylib.GetFrameTime();
        }

        if (EF.transform.position.X + EF.collision.size.X >= screenWidth)
        {
            EF.transform.position.X -= 1;
            EF.transform.speed *= -1;
            foreach (EnemyT enemy in enemyList)
            {
                enemy.transform.position.X -= 1;
                enemy.transform.speed *= -1;
                enemy.transform.position.Y += downMovement;
            }
            ResizeEF(EF, enemyList);
        }
        if (EF.transform.position.X <= 0)
        {
            EF.transform.position.X += 1;
            EF.transform.speed *= -1;
            foreach (EnemyT enemy in enemyList)
            {
                enemy.transform.position.X += 1;
                enemy.transform.speed *= -1;
                enemy.transform.position.Y += downMovement;
            }
            ResizeEF(EF, enemyList);
        }
    }

     */
}
