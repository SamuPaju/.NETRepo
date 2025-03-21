﻿using System.Numerics;
using Raylib_cs;

namespace Avaruuspeli;
/// <summary>
/// Takes care of drawing objects
/// </summary>
public class SpriteRenderer
{
	Transform transform;
	Collision collision;
	public Rectangle box;
	Texture2D sprite;
	public bool rotate;
	public Rectangle spriteSpot;
	float angle;
	Vector2 origin;


    public SpriteRenderer(Transform transform, Collision collision, Texture2D sprite, bool rotate, Rectangle spriteSpot)
	{
		this.transform = transform;
		this.collision = collision;
		this.sprite = sprite;
		this.rotate = rotate;
		this.spriteSpot = spriteSpot;

        // Rotate objects if necessary
        box = new Rectangle((int)transform.position.X, (int)transform.position.Y, collision.size.X, collision.size.Y);
        if (rotate) 
		{
            // Rotation is needed if the image is upside down
            angle = 180;
            origin = new Vector2(box.Width, box.Height);
        }
		else 
		{ 
			angle = 0;
            origin = new Vector2(0, 0);
        }
	}

	/// <summary>
	/// Draws a part of an image
	/// </summary>
	public void Draw()
	{
		// Draw given part of an image
		box = new Rectangle((int)transform.position.X, (int)transform.position.Y, collision.size.X, collision.size.Y);
        Raylib.DrawTexturePro(sprite, spriteSpot, box, origin, angle, Color.White);    
    }

    /// <summary>
    /// Draws given image
    /// </summary>
    /// <param name="spriteSpot"></param>
    public void DrawAnimated(Rectangle spriteSpot)
	{
		// Draw given part of given image
        box = new Rectangle((int)transform.position.X, (int)transform.position.Y, collision.size.X, collision.size.Y);
        Raylib.DrawTexturePro(sprite, spriteSpot, box, origin, angle, Color.White);
    }

	/// <summary>
	/// Spins object
	/// </summary>
	/// <param name="speed">Rotation speed</param>
	public void Spin(int speed)
	{
		angle += speed * Raylib.GetFrameTime();
	}
}
