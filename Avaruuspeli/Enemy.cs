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
    public bool mover;
    bool spin;

    int screenWidth;

    Vector2 velocity;
    Vector2 acceleration;
    float maxSpeed = 100;
    Vector2 levelSpeed;

    public double lastShotTime;
    public float fireRate = 1.5f;

    public Enemy(Rectangle frame, float speed, Texture2D sprite, bool rotate, bool spin,
        Rectangle spriteSpot, Vector2 levelSpeed, bool mover, bool shooter, float fireRate)
    {
        transform = new Transform(frame.Position, speed);
        collision = new Collision(frame.Size);
        spriteRenderer = new SpriteRenderer(transform, collision, sprite, rotate, spriteSpot);

        screenWidth = Raylib.GetScreenWidth();
        this.levelSpeed = levelSpeed;

        this.mover = mover;
        this.shooter = shooter;
        this.fireRate = fireRate;
        this.spin = spin;

        if (mover) { transform.direction.X += 1; }
        transform.direction.Y += 1;
    }

    /// <summary>
    /// Calls needed methods
    /// </summary>
    public void Update()
    {
        if (active)
        {
            Movement();
            // If enemy is set to spin call SpriteRenderers spin method
            if (spin) { spriteRenderer.Spin(25); }
        }
    }

    /// <summary>
    /// Moves enemies
    /// </summary>
    void Movement()
    {
        float time = Raylib.GetFrameTime();

        if (mover)
        {
            if (transform.position.X < collision.size.X / 2)
            {
                transform.direction.X += 1;
            }
            if (transform.position.X > screenWidth - collision.size.X)
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
