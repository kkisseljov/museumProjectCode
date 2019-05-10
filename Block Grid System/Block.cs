using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Block {

    public GridCoordinates coordinates = new GridCoordinates(0,0);
    public BlockType type = BlockType.Rock;
    public int durability = 100;
    public int shaleAmount = 0;

    private int initialDurability;

    [NonSerialized]
    private BlockRenderer _renderer_instance;

    public Block(GridCoordinates coordinates, BlockType type)
    {
        this.coordinates = coordinates;
        this.type = type;

        //TODO add durability to constructor
        initialDurability = durability;
    }

    public BlockRenderer renderer
    {
        get
        {
            return _renderer_instance;
        }
        set
        {
            _renderer_instance = value;
        }
    }

    public bool isRendered
    {
        get
        {
            return _renderer_instance != null && _renderer_instance.isRendered;
        }
    }

    public int MineBlock(int damage)
    {
        int minedShaleAmount = 0;

        if (shaleAmount > 0)
        {
            durability -= damage;

            if(durability <= 0)
            {
                minedShaleAmount = shaleAmount;
                shaleAmount = 0;
            } else
            {
                minedShaleAmount = Mathf.RoundToInt(shaleAmount * ((float)damage / (float)initialDurability));
                Debug.Log("% damage: " + ((float)damage / (float)initialDurability) + " initialDurability: " + initialDurability);
                Debug.Log("shale as float: " + shaleAmount * ((float)damage / (float)initialDurability));
                shaleAmount -= minedShaleAmount;
            }
        }

        Debug.Log("Mine Block(" + coordinates + ") -> \nshale: " + shaleAmount + "\tdmg: " + damage + "\tduraility: " + durability + "\tmined: " + minedShaleAmount);

        return minedShaleAmount;
    }

    public override string ToString()
    {
        return type.ToString() + " - " + coordinates.ToString();
    }
}
