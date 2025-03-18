using Raylib_cs;
using System.Numerics;

namespace Avaruuspeli;

class Boss
{
    public Transform transform;
    public Collision collision;
    public SpriteRenderer spriteRenderer;

    public bool active = true;

    public int health;
    public float fireRate;

    public Boss(Rectangle frame, float speed, Texture2D sprite, bool rotate,
        Rectangle spriteSpot, int health, float fireRate)
    {
        transform = new Transform(frame.Position, speed);
        collision = new Collision(frame.Size);
        spriteRenderer = new SpriteRenderer(transform, collision, sprite, rotate, spriteSpot);

        this.health = health;
        this.fireRate = fireRate;
    }
}
