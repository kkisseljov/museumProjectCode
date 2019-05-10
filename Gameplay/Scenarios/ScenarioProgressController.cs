using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioProgressController : MonoBehaviour {
    public static ScenarioProgressController Singleton { get; private set; }

    public LevelScenarioData scenarioData;
    public LevelProgressData progressData;

    private void Awake()
    {
        Singleton = this;
    }
}
