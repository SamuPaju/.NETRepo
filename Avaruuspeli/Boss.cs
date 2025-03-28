﻿using Raylib_cs;
using System;
using System.Numerics;

namespace Avaruuspeli;

/// <summary>
/// Boss type of enemy 
/// </summary>
class Boss
{
    public Transform transform;
    public Collision collision;
    public SpriteRenderer spriteRenderer;

    public bool active;

    public int health;
    public double lastShotTime = 0;
    public float fireRate;

    public Boss(Rectangle frame, float speed, Texture2D sprite, bool rotate,
        Rectangle spriteSpot, int health, float fireRate)
    {
        transform = new Transform(frame.Position, speed);
        collision = new Collision(frame.Size);
        spriteRenderer = new SpriteRenderer(transform, collision, sprite, rotate, spriteSpot);

        active = false;

        this.health = health;
        this.fireRate = fireRate;
    }

    /// <summary>
    /// Moves boss from side to side
    /// </summary>
    public void Movement()
    {
        transform.position.X += transform.speed * Raylib.GetFrameTime();

        if (transform.position.X + collision.size.X >= Raylib.GetScreenWidth() - 10)
        {
            transform.position.X -= 1;
            transform.speed *= -1;
        }
        if (transform.position.X <= 10)
        {
            transform.position.X += 1;
            transform.speed *= -1;            
        }
    }
}
