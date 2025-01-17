using Raylib_cs;
using System.Numerics;

namespace Omenapeli;
public class Transform
{
	public Vector2 position;
	public float speed;
	public Vector2 velocity;

	public Transform(Vector2 position, float speed)
	{
		this.position = position;
		this.speed = speed;
	}

    public void Update()
    {
		position += velocity * Raylib.GetFrameTime();
    }
}
