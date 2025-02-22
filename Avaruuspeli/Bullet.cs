using System;
using Raylib_cs;
using System.Numerics;

namespace Avaruuspeli;
public class Bullet
{
	public Transform transfrom;
	public Collision collision;
	public SpriteRenderer spriterenderer;

	public Bullet(Vector2 startPosition, Vector2 size, float speed, Color color)
	{
		transfrom = new Transform(startPosition, speed);
		collision = new Collision(transfrom, size);
		spriterenderer = new SpriteRenderer(transfrom, collision, color);
	}

	/// <summary>
	/// Handles bullet's movement and drawing
	/// </summary>
	public void Handler()
	{
		transfrom.position.Y -= transfrom.speed * Raylib.GetFrameTime();
		spriterenderer.Draw();
	}
}
