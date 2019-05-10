using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewMapLoader : MonoBehaviour, ISceneLoader
{
    private NewMapLoaderData _data;

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

    List<LoadingProcces> ISceneLoader.processes
    {
        get
        {
            if(_processes.Count == 0)
            {
                _processes.Add(BlockGridController.Singleton.gridGenerationProcess);
                _processes.AddRange(BlockGridRenderer.Singleton.loadingSequenceProcesses);
                _processes.Add(sceneLoadingProcess);
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
            _data = (NewMapLoaderData) value;
        }
    }

    public IEnumerator StartLoadSequence()
    {
        yield return StartCoroutine(BlockGridController.Singleton.GenerateFlatGrid(_data.gridWidth, _data.gridLength /*, _data.startingPosition*/));
        //yield return StartCoroutine(BlockGridRenderer.Singleton.InstantiateBlockGridAsync());
        //yield return StartCoroutine(BlockGridRenderer.Singleton.SetNearbyBlockReferencesAsync());
        //yield return StartCoroutine(BlockGridRenderer.Singleton.RenderBlocks());
        yield return StartCoroutine(BlockGridRenderer.Singleton.StartLoadSequence());
        yield return StartCoroutine(LoadTargetScene());
        yield return StartCoroutine(MoveObjectsToTargetScene());
        isDone = true;
        yield return null;
    }

    private IEnumerator LoadTargetScene()
    {
        sceneLoadingOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        while(!sceneLoadingOperation.isDone)
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
