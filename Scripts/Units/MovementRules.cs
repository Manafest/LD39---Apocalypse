using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MovementRules {

	public static float moveSpeed = 7;

	public static Vector2 Horizontal (Vector2 myPos, Level level, bool left)
	{
		if (left) {
			// move left
			Vector2 v = myPos + new Vector2 (1, 0);
			if (v.x > level.GetMapWidth() - 1) {
				// trying to move out of bounds
				return new Vector2(-1, -1);
			} else {
				if (level.map.ContainsKey(v) && level.map[v].passable == false) {
					// trying to move out of bounds
					return new Vector2(-1, -1);
				}
			}
			if (checkForUnit (v, level) == false) {
				return v;
			}
			return new Vector2(-1, -1);
		} else {
			Vector2 v = myPos + new Vector2 (-1, 0);
			if (v.x < 0) {
				// trying to move out of bounds
				return new Vector2(-1, -1);
			} else {
				if (level.map.ContainsKey(v) && level.map[v].passable == false) {
					// trying to move out of bounds
					return new Vector2(-1, -1);
				}
			}
			if (checkForUnit (v, level) == false) {
				return v;
			}
			return new Vector2(-1, -1);
		}
	}

	public static Vector2 Vertical (Vector2 myPos, Level level, bool up)
	{
		if (up) {
			// move left
			Vector2 v  = myPos + new Vector2 (0, 1);
			if (v.y > level.GetMapHeight () - 1) {
				// trying to move out of bounds
				return new Vector2(-1, -1);
			} else {
				if (level.map.ContainsKey (v) && level.map [v].passable == false) {
					// trying to move out of bounds
					return new Vector2(-1, -1);
				}
			}
			if (checkForUnit (v, level) == false) {
				return v;
			}
			return new Vector2(-1, -1);
		} else {
			Vector2 v = myPos + new Vector2 (0, -1);
			if (v.y < 0) {
				// trying to move out of bounds
				return new Vector2(-1, -1);
			} else {
				if (level.map.ContainsKey (v) && level.map [v].passable == false) {
					// trying to move out of bounds
					return new Vector2(-1, -1);
				}
			}
			if (checkForUnit (v, level) == false) {
				return v;
			}
			return new Vector2(-1, -1);
		}
	}

	public static bool CheckOutOfBounds (Vector2 newPos, Level level)
	{
		return newPos.x < 0 || newPos.y < 0 || newPos.x > level.GetMapWidth () - 1 || newPos.y > level.GetMapHeight () - 1;
	}

	private static bool checkForUnit (Vector2 newPos, Level level)
	{
		return level.IUnits.Select (x => x.GetPosition ()).Contains (newPos);
	}
}
