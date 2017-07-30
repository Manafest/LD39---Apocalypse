using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : MonoBehaviour, IUnit {

	public Level level;
	public Vector2 StartingPosition;
	private Vector2 xyPosition;
	public Vector2 GetPosition()
	{
		return xyPosition;
	}

	public bool isArcher = false;
	public int range = 1; // range of units attacks
	public int damage = 10;
	public int Power = 5;

	// Use this for initialization
	void Start () {
		if (level == null) {
			level = PlayerManager.currentLevel;
		}
		xyPosition = StartingPosition;
		this.transform.position = Coordinates.toVector3(StartingPosition);
		level.IUnits.Add (this);
	}

	public void TakeDamage (int damage)
	{
		level.IUnits.Remove (this);
		StopCoroutine("moveToPos");
		if (Spells.selectedSpell == Spell.CHAIN) {
			electrocution ();
		}

		GameObject flt = Instantiate (PlayerManager.currentLevel.player.floatyTextPrefab, 
			Coordinates.toVector3 (level.player.GetPosition ()), Quaternion.identity);
		flt.GetComponent<FloatyText> ().Initalise (Power.ToString (), 2f, 1);
		PlayerManager.currentLevel.player.ChangePower (Power);
		if (Spells.selectedSpell != Spell.CHAIN) {
			Destroy (this.gameObject);
		}
	}

	public void TakeTurn ()
	{
		if (CheckForPlayer ()) {
			Debug.Log ("Attacked by " + this.name + " doing " + this.damage + " damage!");
			PlayerManager.currentLevel.player.TakeDamage (damage);
			AttackAnimation ();
			return;
		}
		if (CheckSight ()) {
			moveTowardsPlayer ();
			return;
		}
	}

	private void AttackAnimation ()
	{
		if (isArcher) {
			Spells.ArrowCreation (this.xyPosition);
		} else {
			StopCoroutine("moveToPos");
			StopCoroutine ("attackAnimation");
			StartCoroutine ("attackAnimation", Coordinates.toVector3(level.player.GetPosition ()));
		}

		GameObject flt = Instantiate (PlayerManager.currentLevel.player.floatyTextPrefab, 
			Coordinates.toVector3(level.player.GetPosition ()), Quaternion.identity);
		flt.GetComponent<FloatyText> ().Initalise (damage.ToString(), 2f, 0);
	}

	IEnumerator attackAnimation (Vector3 pos)
	{
		while(Vector3.Distance(pos, this.transform.position) > 1f)
		{
			this.transform.position = Vector3.MoveTowards (this.transform.position, pos, 0.2f);
			yield return null;
		}
		moveToPosition (this.xyPosition);
	}

	private bool CheckForPlayer ()
	{
		// check if player is in "range" if so this unit would like to attack
		return ((float)range + 0.7f) > Vector2.Distance (level.player.GetPosition (), this.xyPosition);
	}

	private bool CheckSight ()
	{
		// check if player is in "range" if so this unit would like to move towards the player
		return 13f > Vector2.Distance (level.player.GetPosition (), this.xyPosition);
	}

	private void moveTowardsPlayer ()
	{
		/*Debug.Log (string.Format ("|{0} - {1}| > |{2} - {3}|",
			level.player.GetPosition ().x, this.xyPosition.x, level.player.GetPosition ().y, this.xyPosition.y));*/
		
		if (Mathf.Abs (level.player.GetPosition ().x - this.xyPosition.x) > 
			Mathf.Abs (level.player.GetPosition ().y - this.xyPosition.y)) {
			// difference is bigger in x
			Vector2 v = MovementRules.Horizontal(this.xyPosition, level, this.xyPosition.x < level.player.GetPosition ().x);
			if (v != new Vector2(-1,-1)) {
				moveToPosition(v);
			} else {
				v = MovementRules.Vertical(this.xyPosition, level, this.xyPosition.y < level.player.GetPosition ().y);
				if (v != new Vector2(-1,-1)) {
					moveToPosition(v);
				}
			}
		} else {
			// difference is bigger in y
			Vector2 v = MovementRules.Vertical(this.xyPosition, level, this.xyPosition.y < level.player.GetPosition ().y);
			if (v != new Vector2(-1,-1)) {
				moveToPosition(v);
			} else {
				v = MovementRules.Horizontal(this.xyPosition, level, this.xyPosition.x < level.player.GetPosition ().x);
				if (v != new Vector2(-1,-1)) {
					moveToPosition(v);
				}
			}
		}
	}

	private void electrocution ()
	{
		// Play electrocution sound
		spriteOne = this.GetComponentInChildren<SpriteRenderer> ().sprite;
		spriteTwo = PlayerManager.currentLevel.player.lightningSprite;
		flip = true;
	}

	private Sprite spriteOne;
	private Sprite spriteTwo;
	private bool flip = false;
	private bool eflip = true;
	private float eDuration = 1.0f;
	private float efrequancy = 0.1f;

	void Update ()
	{
		if (flip) {
			eDuration -= Time.deltaTime;
			efrequancy -= Time.deltaTime;
			if (efrequancy < 0f) {
				if (eflip) {
					this.GetComponentInChildren<SpriteRenderer> ().sprite = spriteOne;
				} else {
					this.GetComponentInChildren<SpriteRenderer> ().sprite = spriteTwo;
				}
				eflip = !eflip;
				efrequancy = 0.2f;
			}
			if (eDuration < 0f) {
				Destroy (this.gameObject);
			}
		}
	}

	private void moveToPosition (Vector2 pow) {
		Vector3 vec3 = Coordinates.toVector3(pow);
		this.xyPosition = pow;
		StopCoroutine("moveToPos");
		StopCoroutine ("attackAnimation");
		StartCoroutine("moveToPos", vec3);
	}

	IEnumerator moveToPos (Vector3 pos)
	{
		while(Vector3.Distance(pos, this.transform.position) > 0.1f)
		{
			this.transform.position = Vector3.Lerp (this.transform.position, pos, Time.deltaTime * MovementRules.moveSpeed);
			yield return null;
		}
	}
}
