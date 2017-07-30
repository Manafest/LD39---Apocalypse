using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coordinates : MonoBehaviour {

	public bool passable = true;

	public int x;
	public int y;
	public Vector2 xy{
		get{
			return new Vector2 (x, y);
		}
	}

	public static Vector3 toVector3 (Vector2 vect2)
	{
		return new Vector3 (vect2.x * 8, vect2.y * 8, 0);
	}

	public override bool Equals (object other)
	{
		return xy.Equals (other);
	}
	public override int GetHashCode ()
	{
		return xy.GetHashCode ();
	}
}
