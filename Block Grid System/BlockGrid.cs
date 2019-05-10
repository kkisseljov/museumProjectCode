using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BlockGrid {
    //Room corners, consider startingPosition as a pivot for coordinates
    public static GridCoordinates startingRoomLeftRearCorner = new GridCoordinates(-1, 0);
    public static GridCoordinates startingRoomFrontRightCorner = new GridCoordinates(2, 4);

    public Block[,] array;

    public GridCoordinates startingPosition;

    public BlockGrid(Block[,] array)
    {
        this.array = array;
    }

    public int Width
    {
        get
        {
            return array != null ? array.GetLength(0) : 0;
        }
    }

    public int Length
    {
        get
        {
            return array != null ? array.GetLength(1) : 0;
        }
    }

    //TODO make const for max values
    public static BlockGrid CreateFlatGrid(int width, int length, BlockType type)
    {
        if(width <= 0 || length <= 0 || width > 200 || length > 200)
        {
            Debug.LogError("Can't create grid - wrong width and length values provided");
            return null;
        }

        Block[,] newGrid = new Block[width, length];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < length; y++)
            {
                newGrid[x, y] = new Block(new GridCoordinates(x, y), type);
            }
        }

        BlockGrid grid = new BlockGrid(newGrid);

        //Setup starting room
        GridCoordinates startingPos = new GridCoordinates(Mathf.RoundToInt(width / 2), 0);
        grid.startingPosition = startingPos;
        grid.array[startingPos.x, startingPos.y].type = BlockType.Ground;

        for (int y = startingRoomLeftRearCorner.y; y < startingRoomFrontRightCorner.y + 1; y++)
        {
            for (int x = startingRoomLeftRearCorner.x; x < startingRoomFrontRightCorner.x + 1; x++)
            {
                grid.array[startingPos.x + x, startingPos.y + y].type = BlockType.Ground;
            }
        }

        //Add shale to rocks
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < length; y++)
            {
                Block block = grid.array[x, y];
                if(block.type == BlockType.RockShale)
                {
                    block.shaleAmount = UnityEngine.Random.Range(100, 500);
                }
            }
        }
       
        return grid;
    }

    public Block GetBlock(GridCoordinates coordinates)
    {
        if(coordinates == null)
        {
            return null;
        }

        if(coordinates.OutOfBounds(this))
        {
            Debug.LogError("Tried to get block that is out of grid bounds:\n Width: " + Width + "\t Length: " + Length + "\n\t coords: " + coordinates.ToString());
            return null;
        }

        return array[coordinates.x, coordinates.y];
    }

    public List<Block> GetNearbyBlocks(GridCoordinates coordinates)
    {
        List<Block> results = new List<Block>();
        List<GridCoordinates> coords = coordinates.GetNearestCoords(this);
        coords.ForEach(x =>
        {
            results.Add(GetBlock(x));
        });
        return results;
    }

    /*
    public List<Block> GetPrefferedNearbyGroundBlocks(GridCoordinates coordinates)
    {
        List<Block> results = new List<Block>();
        coordinates.GetAdjacentCoords(this).ForEach(x =>
        {
            Block block = array[coordinates.x, coordinates.y];

            Debug.Log("Preffered ground block for " + coordinates + " -> " + x + " ( " + block.type + " )");
            if (block.type == BlockType.Ground)
            {
                results.Add(block);
            }
        });

        Debug.Log("GetPrefferedNearbyGroundBlocks found -> " + results.Count);

        return results;
    }
    */

    public List<Block> GetGroundBlocks()
    {
        List<Block> results = new List<Block>();

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Length; y++)
            {
                Block block = array[x, y];
                if(block.type == BlockType.Ground)
                {
                    results.Add(block);
                }
            }
        }

        return results;
    }
}
