using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum TileType
{
	GRASS,
	STONE
}

public enum TileObjectType
{
    NONE,
    OneSH1
}

[System.Serializable]
public class TileData {
	public Material mat;
	public TileType tileType;
}

public class GraphicsHandler : MonoBehaviour {

	public TileData[] tileData;
	public GameObject tilePrefab;
	public PlayerManager player;

	public int height;
	public int width;

	// Tile gap 8

	public void CreateLevel ()
	{
        GameObject tiles = new GameObject("Level #");
        tiles.transform.parent = this.transform;
		for (int x = 0; x < this.height; x++) {
			for (int y = 0; y < this.width; y++) {
                Vector3 pos = new Vector3(x * 8, y * 8, 0);
				GameObject o = Instantiate(tilePrefab, pos, Quaternion.identity, tiles.transform);
                o.name = x + ", " + y;
				// Get tile data
				TileData tile = tileData.First(i => i.tileType == TileType.GRASS);
				o.GetComponent<MeshRenderer> ().material = tile.mat;
				Coordinates xy = o.GetComponent<Coordinates> ();
				xy.x = x; xy.y = y;
				xy.passable = true;
			}
		}
        Debug.Log("Finished Load");
	}

}
