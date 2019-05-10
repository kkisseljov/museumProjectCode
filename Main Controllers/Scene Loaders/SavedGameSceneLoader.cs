using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SavedGameSceneLoader : MonoBehaviour, ISceneLoader
{
    private NextShiftLoaderData _data;

    public bool isDone = false;

    private List<LoadingProcces> _processes = new List<LoadingProcces>();
    private AsyncOperation _sceneLoadingOperation;
    private LoadingProcces _sceneLoadingProcess = new LoadingProcces("Loading gameplay scene");

    private ScenarioProgressController gameProgressController;

    public string sceneName
    {
        get
        {
            return "GamePlayScene";
        }
    }

    List<LoadingProcces> ISceneLoader.processes
    {
        get
        {
            if (_processes.Count == 0)
            {
                _processes.AddRange(BlockGridRenderer.Singleton.loadingSequenceProcesses);
                _processes.Add(_sceneLoadingProcess);
            }

            return _processes;
        }
    }

    ILoaderData ISceneLoader.data
    {
        get
        {
            return _data;
        }

        set
        {
            _data = (NextShiftLoaderData)value;
        }
    }

    public IEnumerator StartLoadSequence()
    {
        MapData mapData = _data.gameplayData.mapData;

        if (mapData == null)
        {
            SceneManager.LoadScene("MainMenu");
            yield return null;
        }
        else
        {
            BlockGridController.Singleton.grid = mapData.blockGrid;
            yield return StartCoroutine(BlockGridRenderer.Singleton.StartLoadSequence());

            //Needs to be done before loading target scene as many things depends on it
            gameProgressController = new GameObject("Scenario Progress").AddComponent<ScenarioProgressController>();
            gameProgressController.scenarioData = _data.gameplayData.scenarioData;
            gameProgressController.progressData = _data.gameplayData.progressData;

            yield return StartCoroutine(LoadTargetScene());
            yield return StartCoroutine(MoveObjectsToTargetScene());
            isDone = true;
            yield return null;
        }
    }

    private IEnumerator LoadTargetScene()
    {
        _sceneLoadingOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        while (!_sceneLoadingOperation.isDone)
        {
            _sceneLoadingProcess.progress = _sceneLoadingOperation.progress;
            yield return null;
        }

        yield return null;
    }

    private IEnumerator MoveObjectsToTargetScene()
    {
        SceneManager.MoveGameObjectToScene(gameProgressController.gameObject, SceneManager.GetSceneByName(sceneName));
        SceneManager.MoveGameObjectToScene(BlockGridController.Singleton.gameObject, SceneManager.GetSceneByName(sceneName));
        SceneManager.MoveGameObjectToScene(BlockGridRenderer.Singleton.gameObject, SceneManager.GetSceneByName(sceneName));

        yield return null;
    }
}
