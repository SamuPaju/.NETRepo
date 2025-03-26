using System.Numerics;
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
        box = new Rectangle(transform.position, collision.size);
        if (rotate) 
		{
            // Rotation is needed if the image is upside down
            angle = 180;
            origin = new Vector2(box.Width / 2, box.Height / 2);
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
		// Update box location and size
        box = new Rectangle(transform.position, collision.size);

        // Draw given part of an image
        Raylib.DrawTexturePro(sprite, spriteSpot, new Rectangle(box.Position + origin, box.Size), origin, angle, Color.White);

        Raylib.DrawRectangleLines((int)box.Position.X, (int)box.Position.Y, (int)box.Size.X, (int)box.Size.Y, Color.Red);
    }

    /// <summary>
    /// Draws given image
    /// </summary>
    /// <param name="spriteSpot"></param>
    public void DrawAnimated(Rectangle spriteSpot)
	{
        // Update box location and size
        box = new Rectangle(transform.position, collision.size);

        // Draw given part of given image
        Raylib.DrawTexturePro(sprite, spriteSpot, new Rectangle(box.Position + origin, box.Size), origin, angle, Color.White);

        Raylib.DrawRectangleLines((int)box.Position.X, (int)box.Position.Y, (int)box.Size.X, (int)box.Size.Y, Color.Red);
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
