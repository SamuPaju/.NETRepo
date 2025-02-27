using System;
using System.Numerics;

namespace Avaruuspeli;
public class Collision
{
	public Transform transfrom;
	public Vector2 size;

	public Collision(Transform transform, Vector2 size)
	{
		this.transfrom = transform;
		this.size = size;
	}
}
