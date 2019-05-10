using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMapLoaderData: ILoaderData
{
    public int gridWidth;
    public int gridLength;
    public GridCoordinates startingPosition;

    public NewMapLoaderData(int gridWidth, int gridLength, GridCoordinates startingPosition)
    {
        this.gridWidth = gridWidth;
        this.gridLength = gridLength;
        this.startingPosition = startingPosition;
    }
}
