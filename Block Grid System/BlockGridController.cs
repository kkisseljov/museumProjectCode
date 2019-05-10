using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGridController : MonoBehaviour {
    public static BlockGridController Singleton { get; private set; }

    public enum Mode { MapEditor, GameLevel };

    public Mode mode;

    public BlockGridPrefabs prefabs;

    [NonSerialized]
    public LoadingProcces gridGenerationProcess = new LoadingProcces("Generating grid");

    [NonSerialized]
    private BlockGrid _grid;

    public bool hasGrid
    {
        get
        {
            return _grid != null;
        }
    }

    public BlockGrid grid
    {
        get
        {
            return _grid;
        }
        set
        {
            _grid = value;
        }
    }

    private void Awake()
    {
        Singleton = this;

        new GameObject("Block Grid").AddComponent<BlockGridRenderer>();
    }

    public IEnumerator GenerateFlatGrid(int width, int length /*, GridCoordinates startingPosition*/)
    {
        if (BlockGridRenderer.Singleton.isRendered)
        {
            BlockGridRenderer.Singleton.Clear();
        }

        yield return null;

        _grid = BlockGrid.CreateFlatGrid(width, length, BlockType.RockShale);
        gridGenerationProcess.progress = 1f;

        yield return null;

        if (_grid != null)
        {
            //_grid.array[startingPosition.x, startingPosition.y].type = BlockType.Ground;
            _grid.startingPosition = new GridCoordinates(Mathf.RoundToInt(width / 2), 0);
            _grid.GetBlock(_grid.startingPosition).type = BlockType.Ground;

            //TODO move to MapEditor scripts
            if (CameraControllerOld.Singleton != null)
            {
                CameraControllerOld.Singleton.freeRoamMovingPoint.position = _grid.startingPosition.GetPositionOffset();
            }
        }

        yield return null;
    }

    /*
    public void LoadGrid(MapData mapData)
    {
        if(BlockGridRenderer.Singleton.isRendered)
        {
            BlockGridRenderer.Singleton.Clear();
        }

        _grid = mapData.blockGrid;
        GridCoordinates startingPosition = _grid.startingPosition;

        if (_grid != null)
        {   
            BlockGridRenderer.Singleton.InstantiateBlockGrid();
      
            CameraControllerOld.Singleton.freeRoamMovingPoint.position = startingPosition.GetPositionOffset();
        }
    }
    */
}
