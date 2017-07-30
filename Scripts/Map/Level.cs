using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Level : MonoBehaviour {

	[SerializeField]
	public int level;
	[SerializeField]
	public Dictionary<Vector2, Coordinates> map;
	public List<IUnit> IUnits;
	public PlayerManager player;
	public Vector2 PlayerStartPosition;
	public GameObject VictoryPanel;
	public GameObject unitsprefab;

	//public List<IUnit> IUnitsToRemove;

	// Use this for initialization
	void OnEnable () {
		//IUnitsToRemove = new List<IUnit> ();
		this.IUnits = new List<IUnit> ();
		map = new Dictionary<Vector2, Coordinates> ();
		Coordinates[] coords = gameObject.GetComponentsInChildren<Coordinates> ();
		for (int i = 0; i < coords.Length; i++) {
			map [coords [i].xy] = coords [i];
		}
		PlayerManager.currentLevel = this;
		Spells.initalise ();
		Debug.Log (string.Format("GetMap {0}, {1}. Count {2}, Last {3}",
			GetMapHeight (), GetMapHeight (), map.Count, map.Last().Key.ToString()));
	}

	public void InitaliseUnits()
	{
		Instantiate (unitsprefab, this.transform);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void TurnMade ()
	{
		var bombs = IUnits.Where (x => x is Bomb).ToArray ();
		for (int i = 0; i < bombs.Length; i++) {
			bombs[i].TakeTurn ();
		}

		foreach (IUnit unit in IUnits) {
			unit.TakeTurn ();
		}

		if (checkLevel()) {
			nextLevel ();
		}
		player.CheckPower ();
		PlayerManager.locked = false;
	}

	private void nextLevel ()
	{
		Debug.Log (this.name + " complete!");
		VictoryPanel.SetActive (true);
	}

	private bool checkLevel ()
	{
		return !this.IUnits.Any ();
	}

	public int GetMapWidth () {
		return map.Values.Last().x + 1;
	}

	public int GetMapHeight () {
		return map.Values.Last().y + 1;
	}
}
