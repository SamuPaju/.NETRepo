using Raylib_cs;
using System;
using System.Numerics;

namespace Omenapeli;
public class Apple
{
	public Transform transform;
    public Collision collision;
    public SpriteRenderer spriteRenderer;

	public Apple(Vector2 position, int size, float speed, Color color)
	{
		transform = new Transform(position, speed);
		collision = new Collision(transform, size);
		spriteRenderer = new SpriteRenderer(transform, collision, color);
	}
}
