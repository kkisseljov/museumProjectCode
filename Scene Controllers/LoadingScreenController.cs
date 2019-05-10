using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreenController : MonoBehaviour
{
    public List<LoadingProcces> processes = new List<LoadingProcces>();

    public Slider progressBar;
    public Text processText;
    public Text overallProgress;

    private ISceneLoader sceneLoader;

    private Scene scene;
    private AsyncOperation asyncSceneLoading = null;
    private LoadingProcces sceneLoadingProcess;

    public bool showLoadingProcesses = false;

    //TODO refactor this
    //Perhaps instantiation of loaders can be made inside ILoaderData scripts
    private void Start()
    {
        scene = SceneManager.GetActiveScene();

        ILoaderData data = SceneSwitcher.Singleton.sceneLoaderData;
  
        if (data.GetType() == typeof(NewMapLoaderData))
        {
            sceneLoader = new GameObject("New Map Loader").AddComponent<NewMapLoader>();
        }
        else if (data.GetType() == typeof(SavedMapLoaderData))
        {
            sceneLoader = new GameObject("Saved Map Loader").AddComponent<SavedMapLoader>();
        }
        else if (data.GetType() == typeof(GamePlayLoaderData))
        {
            sceneLoader = new GameObject("Gameplay scene Loader").AddComponent<GamePlaySceneLoader>(); 
        }
        else if (data.GetType() == typeof(NextShiftLoaderData))
        {
            sceneLoader = new GameObject("Saved game scene Loader").AddComponent<SavedGameSceneLoader>();
        }

        if(sceneLoader != null)
        {
            sceneLoader.data = SceneSwitcher.Singleton.sceneLoaderData;

            sceneLoader.processes.ForEach(x =>
            {
                processes.Add(x);
            });

            StartCoroutine(StartLoadSequence());
        } else
        {
            Debug.LogError("Failed to Instantiate scene loader from given loader data: " + data.GetType().ToString());
            SceneManager.LoadScene("MainMenu");
        }
    }

    private IEnumerator StartLoadSequence()
    {
        yield return StartCoroutine(sceneLoader.StartLoadSequence());
        SceneManager.UnloadSceneAsync(scene);
    }

    void Update()
    {
        SetProgressBarValue();
    }

    void SetProgressBarValue()
    {
        int processesAmount = processes.Count;
        float totalProgress = 0f;
        LoadingProcces currentProcess = null;

        processes.ForEach(x =>
        {
            totalProgress += x.progress;

            if(currentProcess == null && x.progress < 1f)
            {
                currentProcess = x;
            }
        });

        if(currentProcess != null)
        {
            processText.text = currentProcess.processName + " " + Mathf.RoundToInt(currentProcess.progress * 100) + "%";
        }

        progressBar.value = totalProgress / processesAmount;

        overallProgress.text = Mathf.RoundToInt(progressBar.value * 100) + "%";
    }
}
