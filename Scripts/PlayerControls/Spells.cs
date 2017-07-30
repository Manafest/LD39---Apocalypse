using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Spell
{
	NONE = -1,
	CONE,
	CHAIN,
	BOMB,
	METEOR,
	POOL
}

public class Spells : MonoBehaviour {

	public static void initalise ()
	{
		oldMaterial = new Dictionary<Vector2, Material> ();
		unitsHit = new List<IUnit> ();
	}




	public static Spell selectedSpell = Spell.NONE;
	public void SelectSpell (int spell)
	{
		selectedSpell = (Spell)spell;
	}

	public static void ActivateSpell () {
		if (PlayerManager.locked) {
			return;
		}
		if (PlayerManager.currentLevel.player.SpellCheck(selectedSpell)) {
			PlayerManager.currentLevel.player.WarningText.SetActive (true);
			PlayerManager.currentLevel.player.WarningText.GetComponent<WarningText> ().Initalise ();
			return;
		}
		if (selectedSpell == Spell.BOMB) {
			PlantBomb ();
		}
		if (selectedSpell == Spell.POOL) {
			PlayerManager.locked = true;
			PlayerManager.currentLevel.player.SpellCost (selectedSpell);
			ResetHighlights ();
			var all = PlayerManager.currentLevel.map.Keys.ToArray ();
			for (int i = 0; i < all.Length; i++) {
				FlameCreation (all[i]);
			}
			PlayerManager.currentLevel.player.GameWonPanel.SetActive (true);
			return;
		}
		if (unitsHit.Count == 0 && selectedSpell != Spell.BOMB) {
			Debug.Log ("no units to hit");
			return;
		}
		PlayerManager.locked = true;

		GameObject flt = Instantiate (PlayerManager.currentLevel.player.floatyTextPrefab, 
			Coordinates.toVector3(PlayerManager.currentLevel.player.GetPosition ()), Quaternion.identity);
		flt.GetComponent<FloatyText> ().Initalise (PlayerManager.currentLevel.player.getSpellCost(selectedSpell).ToString(), 2f, 2);

		PlayerManager.currentLevel.player.SpellCost (selectedSpell);
		IUnit unitSource = null;
		if (selectedSpell == Spell.METEOR) {
			FireBallCreation ();
			return;
		}
		if (selectedSpell == Spell.CONE) {
			ConeAnimation ();
			return;
		}
		foreach (var unit in unitsHit) {
			if (selectedSpell == Spell.CHAIN) {
				if (unitSource == null) {
					ChainLightningAnimation (unit, PlayerManager.currentLevel.player);
				} else {
					ChainLightningAnimation (unit, unitSource);
				}
				unitSource = unit;
				if (unit is PlayerManager) {
					PlayerManager.currentLevel.player.electrocution ();
				}
			}
			unit.TakeDamage (10);
		}
		unitsHit = new List<IUnit> ();
		PlayerManager.currentLevel.TurnMade ();
	}

	private static void ChainLightningAnimation (IUnit unitDest, IUnit unitSource) {
		GameObject l = Instantiate (PlayerManager.currentLevel.player.lightningPrefab, PlayerManager.currentLevel.transform);
		l.transform.position = Coordinates.toVector3(unitSource.GetPosition ());
		l.transform.localScale = new Vector3 (Vector2.Distance (unitSource.GetPosition (), unitDest.GetPosition ()), 1, 1);

		Vector2 C = unitSource.GetPosition () - unitDest.GetPosition ();
		Debug.Log ((Mathf.Rad2Deg * Mathf.Atan2 (C.y, C.x)) - 180);
		l.transform.Rotate (0, 0, (Mathf.Rad2Deg * Mathf.Atan2 (C.y, C.x)) - 180);
	}

	public static void FlameHitCallBack ()
	{
		foreach (var unit in unitsHit) {
			FlameCreation (unit.GetPosition ());
			unit.TakeDamage (50);
		}
		unitsHit = new List<IUnit> ();
		PlayerManager.currentLevel.TurnMade ();
	}

	public static void ArrowCreation (Vector2 source)
	{
		GameObject l = Instantiate (PlayerManager.currentLevel.player.arrowPrefab, PlayerManager.currentLevel.transform);
		l.transform.position = Coordinates.toVector3(source);

		Vector2 C = source - PlayerManager.currentLevel.player.GetPosition ();
		l.transform.Rotate (0, 0, (Mathf.Rad2Deg * Mathf.Atan2 (C.y, C.x)) - 60);
		l.GetComponent<Arrow> ().initalise (Coordinates.toVector3 (PlayerManager.currentLevel.player.GetPosition ()));

	}

	private static void FireBallCreation ()
	{
		GameObject l = Instantiate (PlayerManager.currentLevel.player.fireballPrefab, PlayerManager.currentLevel.transform);
		l.transform.position = Coordinates.toVector3(PlayerManager.currentLevel.player.GetPosition());

		Vector2 C = PlayerManager.currentLevel.player.GetPosition () - TargetPos;
		l.transform.Rotate (0, 0, (Mathf.Rad2Deg * Mathf.Atan2 (C.y, C.x)));
		l.GetComponent<Meteor> ().initalise (Coordinates.toVector3 (TargetPos));

	}

	private static void FlameCreation (Vector2 vec)
	{
		GameObject f = Instantiate (PlayerManager.currentLevel.player.flamePrefab, PlayerManager.currentLevel.transform);
		f.transform.position = Coordinates.toVector3(vec);
	}

	private static Vector2 TargetPos;
	private static void PlantBomb ()
	{
		GameObject o = Instantiate (PlayerManager.currentLevel.player.bombPrefab, PlayerManager.currentLevel.transform);
		o.GetComponentInChildren<Bomb> ().initalise (TargetPos, PlayerManager.currentLevel);
	}


	public static void BombAnimation ()
	{
		for (int x = -2; x < 3; x++) {
			for (int y = -2; y < 3; y++) {
				Vector2 pos = TargetPos + new Vector2(x, y);
				if (!MovementRules.CheckOutOfBounds (pos, PlayerManager.currentLevel)) {
					FlameCreation (pos);
				}
			}
		}
	}

	private static void ConeAnimation () 
	{
		FlameHitCallBack ();
	}

	private static int chainBounces = 5;
	private static float bounceDistance = 6;
	private static List<IUnit> unitsHit;

	public static void ChainLighting (IUnit unit) {
		unitsHit = new List<IUnit> ();
		HighlightTarget (unit.GetPosition ());
		unitsHit.Add (unit);

		var closeUnits = PlayerManager.currentLevel.IUnits
			.Where (x => Vector2.Distance (x.GetPosition (), unit.GetPosition ()) < bounceDistance && x != unit);
		
		for (int i = 0; i < chainBounces; i++) {
			if (closeUnits.Any()) {
				IUnit nextUnit = closeUnits.First ();
				HighlightTarget (nextUnit.GetPosition ());
				unitsHit.Add (nextUnit);
				closeUnits = PlayerManager.currentLevel.IUnits
					.Where (x => Vector2.Distance (x.GetPosition (), nextUnit.GetPosition ()) < bounceDistance && !unitsHit.Contains(x));
			} else {
				HighlightTarget (PlayerManager.currentLevel.player.GetPosition ());
				unitsHit.Add (PlayerManager.currentLevel.player);
				break;
			}
		}
	}

	private static bool WithinRangeCone (Vector2 vec, Vector2 characterPos)
	{
		return (Mathf.Abs (vec.x - characterPos.x) + Mathf.Abs (vec.y - characterPos.y)) < 2;
	}

	public static void ConeOfFlame (Vector2 vec)
	{
		ResetHighlights ();
		if (vec.Equals(PlayerManager.currentLevel.player.GetPosition ())
			|| !WithinRangeMeteor (PlayerManager.currentLevel.player.GetPosition (), vec)) {
			return;
		}
		unitsHit = PlayerManager.currentLevel.IUnits
			.Where (x => WithinRangeCone(vec, x.GetPosition())
				&& WithinRangeCone(PlayerManager.currentLevel.player.GetPosition(), x.GetPosition())).ToList();
		for (int x = -2; x < 3; x++) {
			for (int y = -2; y < 3; y++) {
				Vector2 pos = vec + new Vector2 (x, y);
				if (WithinRangeCone (vec, pos) && WithinRangeCone (PlayerManager.currentLevel.player.GetPosition (), pos)
					&& !MovementRules.CheckOutOfBounds (pos, PlayerManager.currentLevel)) {
					HighlightTarget (pos);
				} else {
					if (PlayerManager.currentLevel.player.GetPosition () == pos) {
						HighlightTarget (pos);
					}
				}
			}
		}
	}

	public static void InsanityBomb (Vector2 vec)
	{
		ResetHighlights ();
		unitsHit = new List<IUnit> ();
		TargetPos = vec;
		for (int x = -2; x < 3; x++) {
			for (int y = -2; y < 3; y++) {
				Vector2 pos = vec + new Vector2(x, y);
				if (!MovementRules.CheckOutOfBounds (pos, PlayerManager.currentLevel)) {
					HighlightTarget (pos);
				}
			}
		}
	}

	public static void Meteor (Vector2 vec)
	{
		ResetHighlights ();
		unitsHit = PlayerManager.currentLevel.IUnits.Where (x => WithinRangeMeteor(vec, x.GetPosition())).ToList();
		for (int x = -2; x < 3; x++) {
			for (int y = -2; y < 3; y++) {
				Vector2 pos = vec + new Vector2(x, y);
				if (WithinRangeMeteor(vec, pos) & !MovementRules.CheckOutOfBounds (pos, PlayerManager.currentLevel)) {
					HighlightTarget (pos);
				}
			}
		}
		if (WithinRangeMeteor(vec,PlayerManager.currentLevel.player.GetPosition())) {
			unitsHit.Add (PlayerManager.currentLevel.player);
		}
		TargetPos = vec;
	}

	private static bool WithinRangeMeteor (Vector2 vec, Vector2 characterPos)
	{
		return (Mathf.Abs (vec.x - characterPos.x) + Mathf.Abs (vec.y - characterPos.y)) < 3;
	}

	public static void PoolsOfDeath (Vector2 vec)
	{
		ResetHighlights ();
		var all = PlayerManager.currentLevel.map.Keys.ToArray ();
		for (int i = 0; i < all.Length; i++) {
			HighlightTarget (all[i]);
		}

	}

	public static void ResetHighlights ()
	{
		foreach (KeyValuePair<Vector2, Material> kv in oldMaterial) {
			PlayerManager.currentLevel.map [kv.Key].GetComponent<MeshRenderer> ().material = kv.Value;
		}
		oldMaterial = new Dictionary<Vector2, Material> ();
	}

	private static Dictionary<Vector2, Material> oldMaterial;

	private static void HighlightTarget (Vector2 vec) {
		if (oldMaterial.ContainsKey(vec)) {
			return;
		}
		MeshRenderer r = PlayerManager.currentLevel.map [vec].GetComponent<MeshRenderer> ();
		oldMaterial.Add (vec, r.material);
		r.material = PlayerManager.currentLevel.player.highlightMaterial;
	}

}
