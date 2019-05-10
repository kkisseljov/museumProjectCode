using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour {

    public Grid grid;

    private TileController _tileController;

	// Use this for initialization
	void Start () {
        _tileController = this.GetComponent<TileController>();
        grid = new Grid(100,100);
        grid.GenerateTestGrid(3, 3);

        InstantiateTiles();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void InstantiateTiles()
    {
        _tileController.Clean();

        for (int x = 0; x < grid.width_x; x++)
        {
            for(int y = 0; y < grid.length_y; y++)
            {
                int tileCode = grid.GetTileCode(x, y);
                if (tileCode > 0)
                {
                    _tileController.SpawnTile(x, y, tileCode);
                }
                else
                {
                    Debug.LogError("NO TILE CODE FOR:" + x + ", " + y);
                }
                
            }
        }
    }
}
