using System;
using System.Numerics;
using Raylib_cs;

namespace Avaruuspeli;
public class SpriteRenderer
{
	public Transform transform;
	public Collision collision;
	public Color color;
	public Rectangle box;
	Texture2D sprite;

	public SpriteRenderer(Transform transform, Collision collision, Color color, Texture2D sprite)
	{
		this.transform = transform;
		this.collision = collision;
		this.color = color;
		this.sprite = sprite;
	}

	public void Draw()
	{
		box = new Rectangle((int)transform.position.X, (int)transform.position.Y, collision.size.X, collision.size.Y);
        Raylib.DrawRectangleRec(box, color);

		// Hole picture
		//Raylib.DrawTextureV(sprite, transform.position, Color.White);

		// Part of a picture
		//Rectangle sourceRec = new Rectangle(new Vector2(100, 57), new Vector2(20, 25));
		//Raylib.DrawTextureRec(sprite, sourceRec, transform.position, Color.White);

		// Part of the picture better
		NPatchInfo nPatchInfo = new NPatchInfo()
		{
			// What part of the picture
			Source = new Rectangle(26, 0, 24, 26),

			// How much scaling (propably???)
			Left = 0,
			Top = 0,
			Right = 0,
			Bottom = 0
		};
		Raylib.DrawTextureNPatch(sprite, nPatchInfo, box, new Vector2(box.Width, box.Height), 180, Color.White);
	}
}
