using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavedMapLoaderData : ILoaderData
{
    public string fileName;
    [Obsolete]
    public bool loadAsAsset = false;

    public SavedMapLoaderData(string fileName)
    {
        this.fileName = fileName;
    }

    [Obsolete]
    public SavedMapLoaderData(string fileName, bool loadAsAsset) : this(fileName)
    {
        this.loadAsAsset = loadAsAsset;
    }
}
