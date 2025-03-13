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
}
