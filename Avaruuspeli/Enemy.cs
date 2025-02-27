﻿using System;
using System.Numerics;
using Raylib_cs;

namespace Avaruuspeli;

public class Enemy
{
    public Transform transform;
    public Collision collision;
    public SpriteRenderer spriteRenderer;

    public Enemy(Vector2 position, Vector2 size, float speed, Color color, Texture2D sprite, bool rotate, Rectangle spriteSpot)
	{
        transform = new Transform(position, speed);
        collision = new Collision(transform, size);
        spriteRenderer = new SpriteRenderer(transform, collision, color, sprite, rotate, spriteSpot);
    }

    /* Useless (Just do it in the main program)
    public void ChangeDirection()
    {
        transform.speed *= -1;
    }*/
}
