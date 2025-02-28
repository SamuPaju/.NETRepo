using System;
using Raylib_cs;
using System.Numerics;

namespace Avaruuspeli;
/// <summary>
/// Bullet class that holds all the components and methods that it needs
/// </summary>
public class Bullet
{
	public Transform transfrom;
	public Collision collision;
	public SpriteRenderer spriterenderer;

	public Bullet(Vector2 startPosition, Vector2 size, float speed, Color color, Texture2D sprite, bool rotate, Rectangle spriteSpot)
	{
		transfrom = new Transform(startPosition, speed);
		collision = new Collision(size);
		spriterenderer = new SpriteRenderer(transfrom, collision, color, sprite, rotate, spriteSpot);
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
