using System;
using System.Numerics;

namespace Avaruuspeli;
/// <summary>
/// Holds the size of the object
/// </summary>
public class Collision
{
	public Vector2 size;

	public Collision(Vector2 size)
	{
		this.size = size;
	}
}
