using System;

namespace Omenapeli;
public class Collision
{
	public Transform transfrom;
	public int size;
	public Collision(Transform transform, int size)
	{
		this.transfrom = transform;
		this.size = size;
	}
}
