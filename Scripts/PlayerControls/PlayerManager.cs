using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour, IUnit {

	public static Level currentLevel;
	public Material highlightMaterial;
	public GameObject bombPrefab;
	public GameObject lightningPrefab;
	public Sprite lightningSprite;
	public GameObject flamePrefab;
	public GameObject fireballPrefab;
	public GameObject arrowPrefab;
	public GameObject floatyTextPrefab;

	public GameObject WarningText;
	public GameObject GameOverPanel;
	public GameObject GameWonPanel;
	public GameObject[] Levels;

	public static bool locked = false;

	public void NextLevel ()
	{
		currentLevel.gameObject.SetActive (false);
		level++;
		if (Levels.Length - 1 < level)  {
			// level list completed Do random of 2-4
			int l = Random.Range(2,5);
			Levels [l].SetActive(true);
			currentLevel = Levels [l].GetComponent<Level>();
			currentLevel.InitaliseUnits ();
		} else {
			Levels [level].SetActive(true);
			currentLevel = Levels [level].GetComponent<Level>();
		}
		currentLevel.player = this;
		initalise ();
	}

	private Vector2 xyPosition;
	public Vector2 GetPosition()
	{
		return xyPosition;
	}

	public int power = 10;
	public int level = 0;

	// Use this for initialization
	public void initalise () {
		GameOver = false;
		GameOverWaitTime = 1f;
		PowerNumber.text = power.ToString ();
		xyPosition = currentLevel.PlayerStartPosition;
		this.transform.position = Coordinates.toVector3(xyPosition);
	}

	float GameOverWaitTime = 1f;
	bool GameOver;
	
	// Update is called once per frame
	void Update () {
		if (GameOver) {
			if (GameOverWaitTime < 0f) {
				GameOverPanel.SetActive (true);
			} else {
				GameOverWaitTime -= Time.deltaTime;
			}
		}
		if (currentLevel.player != this) {
			currentLevel.player = this;
			initalise ();
		}
		if (flip) {
			eDuration -= Time.deltaTime;
			efrequancy -= Time.deltaTime;
			if (efrequancy < 0f) {
				if (eflip) {
					this.GetComponentInChildren<SpriteRenderer> ().sprite = spriteOne;
				} else {
					this.GetComponentInChildren<SpriteRenderer> ().sprite = lightningSprite;
				}
				eflip = !eflip;
				efrequancy = 0.2f;
			}
			if (eDuration < 0f) {
				flip = !flip;
				if (power < 1) {
					//Game Over
					Debug.Log("Game Over!");
				}
			}
		}
	}

	public void electrocution ()
	{
		// Play electrocution sound
		spriteOne = this.GetComponentInChildren<SpriteRenderer> ().sprite;
		flip = true;
	}

	private Sprite spriteOne;
	private Sprite spriteTwo;
	private bool flip = false;
	private bool eflip = true;
	private float eDuration = 1.0f;
	private float efrequancy = 0.1f;

	public Text PowerNumber;

	public void ChangePower (int change)
	{
		power = power + change;
		PowerNumber.text = power.ToString ();

	}

	public void CheckPower ()
	{
		if (power <= 0) {
			locked = true;
			GameOver = true;
		}
	}

	public void TakeDamage (int damage)
	{
		ChangePower (-damage);
	}

	public void TakeTurn ()
	{
		
	}

	public void SpellCost (Spell spell)
	{
		ChangePower (-getSpellCost (spell));
	}

	public bool SpellCheck (Spell spell)
	{
		return getSpellCost (spell) > power;
	}

	public int getSpellCost (Spell spell)
	{
		switch (spell) {
		case Spell.CONE:
			return 2;
		case Spell.BOMB:
			return 20;
		case Spell.METEOR:
			return 15;
		case Spell.CHAIN:
			return 10;
		case Spell.POOL:
			return 250;
		default:
			return 0;
		}
	}

	public void Horizontal (float raw)
	{
		Vector2 v = MovementRules.Horizontal (xyPosition, currentLevel, raw > 0);
		if (v != new Vector2(-1,-1)) {
			moveToPosition(v);
		}
	}

	public void Vertical (float raw)
	{
		Vector2 v = MovementRules.Vertical (xyPosition, currentLevel, raw > 0);
		if (v != new Vector2(-1,-1)) {
			moveToPosition(v);
		}
	}


	private void moveToPosition (Vector2 pow) {
		if (locked) {
			return;
		}
		Vector3 vec3 = Coordinates.toVector3(pow);
		this.xyPosition = pow;
		StopCoroutine("moveToPos");
		StartCoroutine("moveToPos", vec3);
		currentLevel.TurnMade ();
	}

	IEnumerator moveToPos (Vector3 pos)
	{
		while(Vector3.Distance(pos, this.transform.position) > 0.05f)
		{
			this.transform.position = Vector3.Lerp (this.transform.position, pos, Time.deltaTime * MovementRules.moveSpeed);
			yield return null;
		}
	}
}
