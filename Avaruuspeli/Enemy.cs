using Raylib_cs;
using System.Collections.Generic;
using System.Numerics;

namespace Avaruuspeli;

class Enemy
{
    public Transform transform;
    public Collision collision;
    public SpriteRenderer spriteRenderer;
    public bool active;

    Vector2 velocity;
    Vector2 acceleration;
    float maxSpeed = 100;

    public Enemy(Rectangle frame, float speed, Texture2D sprite, bool rotate, Rectangle spriteSpot)
    {
        transform = new Transform(frame.Position, speed);
        collision = new Collision(frame.Size);
        spriteRenderer = new SpriteRenderer(transform, collision, sprite, rotate, spriteSpot);
    }

    public void Movement()
    {
        float time = Raylib.GetFrameTime();

        acceleration = transform.direction * transform.speed;// + levelSpeed;
        velocity += acceleration * time;

        // Check that the velocity doesn't go way too high or low
        if (velocity.X < -maxSpeed) { velocity.X = -maxSpeed; }
        if (velocity.X > maxSpeed) { velocity.X = maxSpeed; }
        if (velocity.Y < -maxSpeed) { velocity.Y = -maxSpeed; }
        if (velocity.Y > maxSpeed) { velocity.Y = maxSpeed; }

        transform.position += velocity * time;
    }
    
}
