using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SavedMapLoader : MonoBehaviour, ISceneLoader
{
    private SavedMapLoaderData _data;

    public bool isDone = false;

    private List<LoadingProcces> _processes = new List<LoadingProcces>();

    private AsyncOperation sceneLoadingOperation;

    private LoadingProcces sceneLoadingProcess = new LoadingProcces("Loading Map Editor scene");

    public string sceneName
    {
        get
        {
            return "MapEditor";
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
            _data = (SavedMapLoaderData) value;
        }
    }

    List<LoadingProcces> ISceneLoader.processes
    {
        get
        {
            if (_processes.Count == 0)
            {
                _processes.AddRange(BlockGridRenderer.Singleton.loadingSequenceProcesses);
                _processes.Add(sceneLoadingProcess);
            }

            return _processes;
        }
    }

    public IEnumerator StartLoadSequence()
    {
        MapData mapData = MapData.Load(_data.fileName);

        if(mapData == null)
        {
            SceneManager.LoadScene("MainMenu");
            yield return null;
        } else
        {
            BlockGridController.Singleton.grid = mapData.blockGrid;
            yield return StartCoroutine(BlockGridRenderer.Singleton.StartLoadSequence());
            yield return StartCoroutine(LoadTargetScene());
            yield return StartCoroutine(MoveObjectsToTargetScene());
            isDone = true;
            yield return null;
        }
    }

    private IEnumerator LoadTargetScene()
    {
        sceneLoadingOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        while (!sceneLoadingOperation.isDone)
        {
            sceneLoadingProcess.progress = sceneLoadingOperation.progress;
            yield return null;
        }

        yield return null;
    }

    private IEnumerator MoveObjectsToTargetScene()
    {
        SceneManager.MoveGameObjectToScene(BlockGridController.Singleton.gameObject, SceneManager.GetSceneByName(sceneName));
        SceneManager.MoveGameObjectToScene(BlockGridRenderer.Singleton.gameObject, SceneManager.GetSceneByName(sceneName));

        yield return null;
    }
}
