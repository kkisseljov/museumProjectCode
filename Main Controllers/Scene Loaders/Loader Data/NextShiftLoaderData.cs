using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextShiftLoaderData : ILoaderData {

    public GameplayData gameplayData;

    public NextShiftLoaderData(GameplayData gameplayData)
    {
        this.gameplayData = gameplayData;
    }
}
