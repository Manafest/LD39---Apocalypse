using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnit {

	Vector2 GetPosition ();
	void TakeTurn ();
	void TakeDamage (int damage);
}
