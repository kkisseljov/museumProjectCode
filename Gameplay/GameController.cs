using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    public static GameController Singleton { get; private set; }

    public PauseMenu pauseMenu;

    public int shiftDuration = 28800;
    public float currentShiftTime = 0f;
    public float shiftTimeScale = 20f;
    
    public bool paused
    {
        get
        {
            return Time.timeScale == 0f;
        }
        set
        {
            Time.timeScale = value ? 0f : 1f;
        }
    }

    public bool isShiftActive
    {
        get
        {
            return currentShiftTime <= shiftDuration;
        }
    }

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        //Check for case if scene is opened in editor
        if (BlockGridController.Singleton != null)
        {
            CameraControllerOld.Singleton.freeRoamMovingPoint.transform.position = BlockGridController.Singleton.grid.startingPosition.GetPositionOffset();

            StartCoroutine(SpawnCharacters(3));
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            paused = !paused;

            pauseMenu.gameObject.SetActive(paused);
        }

        if(!paused)
        {
            UpdateShiftTime();
        }
    }

    private void UpdateShiftTime()
    {
        currentShiftTime += Time.unscaledDeltaTime * shiftTimeScale;
    }

    public void EndShift()
    {
        LevelScenarioData scenarioData = ScenarioProgressController.Singleton.scenarioData;
        LevelProgressData progressData = ScenarioProgressController.Singleton.progressData;

        progressData.shiftsPassed++;

        MapData mapData = new MapData(scenarioData.name, BlockGridController.Singleton.grid);

        GameplayData gameplayData = new GameplayData(
            "Saved Game - " + scenarioData.name + " - Shift #" + (progressData.shiftsPassed + 1) + " - " + DateTime.Now.ToString("MM-dd-yyyy--HH-mm-ss"),
            mapData,
            scenarioData,
            progressData
        );

        SceneSwitcher.Singleton.ToResultsScene(new NextShiftLoaderData(gameplayData));
    }

    //TODO temprorary, just for testing
    private IEnumerator SpawnCharacters(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            yield return StartCoroutine(CharactersController.Singleton.SpawnPlayerCharacter());
        }
    }
}
