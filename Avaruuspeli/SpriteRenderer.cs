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

	public void Draw()
	{
		box = new Rectangle((int)transform.position.X, (int)transform.position.Y, collision.size.X, collision.size.Y);
        //Raylib.DrawRectangleRec(box, color);

		// Hole picture
		//Raylib.DrawTextureV(sprite, transform.position, Color.White);

		// Part of a picture
		//Rectangle sourceRec = new Rectangle(new Vector2(100, 57), new Vector2(20, 25));
		//Raylib.DrawTextureRec(sprite, sourceRec, transform.position, Color.White);

		// If rotate is true we will turn the image and set the origin to the down right corner
		if (rotate)
		{
            Raylib.DrawTexturePro(sprite, spriteSpot, box, new Vector2(box.Width, box.Height), 180, Color.White);
        }
		// Else we will draw the image normally
		else { Raylib.DrawTexturePro(sprite, spriteSpot, box, new Vector2(0, 0), 0, Color.White); }       
    }
}
