using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour {
    public static SceneSwitcher Singleton { get; private set; }

    public string loadingScreenScene;

    private ILoaderData _sceneLoaderData;

    private void Awake()
    {
        Singleton = this;
    }

    public ILoaderData sceneLoaderData
    {
        get
        {
            return _sceneLoaderData;
        }
    }

    public void QuitToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ToResultsScene(ILoaderData sceneLoaderData)
    {
        _sceneLoaderData = sceneLoaderData;
        SceneManager.LoadScene("ResultsScene");
    }

    public void PrepareToLoadLevel()
    {
        SceneManager.LoadScene(loadingScreenScene);
    }

    public void PrepareToLoadLevel(ILoaderData sceneLoaderData)
    {
        _sceneLoaderData = sceneLoaderData;
        SceneManager.LoadScene(loadingScreenScene);
    }
}
