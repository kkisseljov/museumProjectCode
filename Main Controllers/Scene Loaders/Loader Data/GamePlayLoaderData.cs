using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayLoaderData : ILoaderData
{
    public SavedMapLoaderData savedMapLoaderData;
    public LevelScenarioData scenarioData;

    public GamePlayLoaderData(SavedMapLoaderData savedMapLoaderData, LevelScenarioData scenarioData)
    {
        this.savedMapLoaderData = savedMapLoaderData;
        this.scenarioData = scenarioData;
    }
}
