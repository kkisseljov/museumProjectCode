using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour {

    public List<GameObject> tilePrefabs = new List<GameObject>();

    public List<Tile> tiles = new List<Tile>();
    public Transform gridTransform;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SpawnTile(int gridPositionX, int gridPositionY, int tileCode)
    {
        GameObject targetPrefab = tilePrefabs.Find((GameObject _tileGo) =>
        {
            Tile _tileComponent = _tileGo.GetComponent<Tile>();
            return tileCode == _tileComponent.code;
        });

        GameObject tile = Instantiate(targetPrefab, gridTransform, false);
        tile.transform.localPosition = new Vector3(gridPositionX, targetPrefab.transform.localPosition.y, gridPositionY);
        tile.gameObject.name += "_" + gridPositionX + ":" + gridPositionY;
        Tile tileComponent = tile.GetComponent<Tile>();
        tileComponent.gridCoordinateX = gridPositionX;
        tileComponent.gridCoordinateY = gridPositionY;

        tiles.Add(tileComponent);
    }

    public void Clean()
    {
        foreach(Tile tile in tiles)
        {
            Destroy(tile.gameObject);
        }
    }
}
