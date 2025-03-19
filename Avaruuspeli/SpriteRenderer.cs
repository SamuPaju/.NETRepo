using System.Numerics;
using Raylib_cs;

namespace Avaruuspeli;
/// <summary>
/// Takes care of drawing objects
/// </summary>
public class SpriteRenderer
{
	public Transform transform;
	public Collision collision;
	public Rectangle box;
	Texture2D sprite;
	bool rotate;
	public Rectangle spriteSpot;


    public SpriteRenderer(Transform transform, Collision collision, Texture2D sprite, bool rotate, Rectangle spriteSpot)
	{
		this.transform = transform;
		this.collision = collision;
		this.sprite = sprite;
		this.rotate = rotate;
		this.spriteSpot = spriteSpot;
	}

	/// <summary>
	/// Draws a part of an image
	/// </summary>
	public void Draw()
	{
		box = new Rectangle((int)transform.position.X, (int)transform.position.Y, collision.size.X, collision.size.Y);

		// Part of a picture
		// If rotate is true we will turn the image and set the origin to the down right corner
		if (rotate) { Raylib.DrawTexturePro(sprite, spriteSpot, box, new Vector2(box.Width, box.Height), 180, Color.White); }
		// Else we will draw the image normally
		else { Raylib.DrawTexturePro(sprite, spriteSpot, box, new Vector2(0, 0), 0, Color.White); }       
    }

    /// <summary>
    /// Draws given image
    /// </summary>
    /// <param name="spriteSpot"></param>
    public void DrawAnimated(Rectangle spriteSpot)
	{
        box = new Rectangle((int)transform.position.X, (int)transform.position.Y, collision.size.X, collision.size.Y);

        // If rotate is true we will turn the image and set the origin to the down right corner
        if (rotate) { Raylib.DrawTexturePro(sprite, spriteSpot, box, new Vector2(box.Width, box.Height), 180, Color.White); }
        // Else we will draw the image normally
        else { Raylib.DrawTexturePro(sprite, spriteSpot, box, new Vector2(0, 0), 0, Color.White); }
    }
}
