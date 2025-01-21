using System;
using Raylib_cs;

namespace Avaruuspeli;
public class SpriteRenderer
{
	public Transform transform;
	public Collision collision;
	public Color color;
	public Rectangle box;

	public SpriteRenderer(Transform transform, Collision collision, Color color)
	{
		this.transform = transform;
		this.collision = collision;
		this.color = color;
	}

	public void Draw()
	{
		box = new Rectangle((int)transform.position.X, (int)transform.position.Y, collision.size / 2, collision.size / 2);
        Raylib.DrawRectangleRec(box, color);
	}
}
