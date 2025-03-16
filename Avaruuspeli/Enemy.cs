using Raylib_cs;
using System.Numerics;

namespace Avaruuspeli;

class Enemy
{
    public Transform transform;
    public Collision collision;
    public SpriteRenderer spriteRenderer;

    public bool active;
    public bool shooter;
    public bool mover = true;

    int screenWidth;

    Vector2 velocity;
    Vector2 acceleration;
    float maxSpeed = 100;
    Vector2 levelSpeed;

    public double lastShotTime;
    public float firerate = 1;

    public Enemy(Rectangle frame, float speed, Texture2D sprite, bool rotate, Rectangle spriteSpot, Vector2 levelSpeed)
    {
        transform = new Transform(frame.Position, speed);
        collision = new Collision(frame.Size);
        spriteRenderer = new SpriteRenderer(transform, collision, sprite, rotate, spriteSpot);

        this.screenWidth = Raylib.GetScreenWidth();
        this.levelSpeed = levelSpeed;

        if (mover) { transform.direction.X += 1; }
        transform.direction.Y += 1;
    }

    public void Movement()
    {
        float time = Raylib.GetFrameTime();

        if (mover)
        {
            if (transform.position.X < 50)
            {
                transform.direction.X += 1;
            }
            if (transform.position.X > screenWidth - 50)
            {
                transform.direction.X -= 1;
            }
        }

        //acceleration = transform.direction * transform.speed + levelSpeed;
        acceleration.X = transform.direction.X * transform.speed;
        acceleration.Y = transform.direction.Y * transform.speed + levelSpeed.Y;
        velocity += acceleration * time;

        // Check that the velocity doesn't go way too high or low
        if (velocity.X < -maxSpeed) { velocity.X = -maxSpeed; }
        if (velocity.X > maxSpeed) { velocity.X = maxSpeed; }
        if (velocity.Y < -maxSpeed) { velocity.Y = -maxSpeed; }
        if (velocity.Y > maxSpeed) { velocity.Y = maxSpeed; }

        //Console.WriteLine(velocity);
        transform.position += velocity * time;
    }

    
    
}
