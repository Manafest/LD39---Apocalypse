using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bomb : MonoBehaviour, IUnit {

	public Level level;
	public Vector2 StartingPosition;

	public int damage = 100;
	public int timer = 2;

	public Vector2 GetPosition ()
	{
		return StartingPosition;
	}

	// Use this for initialization
	public void initalise (Vector2 startingPosition, Level lvl) {
		this.StartingPosition = startingPosition;
		this.transform.position = Coordinates.toVector3(StartingPosition);
		this.level = lvl;
		level.IUnits.Add (this);
	}

	public void TakeDamage (int damage)
	{
		Spells.BombAnimation ();
		level.IUnits.Remove (this);
		Destroy (this.gameObject);

	}

	private bool WithinRange (Vector2 vec)
	{
		return Mathf.Abs (vec.x - this.StartingPosition.x) < 3 && Mathf.Abs (vec.y - this.StartingPosition.y) < 3;
	}

	public void TakeTurn ()
	{
		if (timer == 0) {
			// explode
			var unitsHit = PlayerManager.currentLevel.IUnits.Where (i =>  WithinRange(i.GetPosition())).ToArray();
			for (int i = 0; i < unitsHit.Length; i++) {
				unitsHit[i].TakeDamage (50);
			}
			if (WithinRange(PlayerManager.currentLevel.player.GetPosition())) {
				PlayerManager.currentLevel.player.TakeDamage (50);
			}

		} else {
			timer--;
		}
		if (timer < 0) {
			Debug.LogError (this.name + " did not remove itself!");
		}
	}
}
