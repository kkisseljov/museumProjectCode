using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {

    public void OpenMapEditor()
    {
        //SceneSwitcher.Singleton.PrepareToLoadLevel("MapEditor");
    }

    public void OpenNewGameScreen()
    {
        //TODO implement this

        //TODO temporary
        //SceneSwitcher.Singleton.PrepareToLoadLevel("GRID_TEST");
    }

    public void OpenLoadGameScreen()
    {
        //TODO implement this
    }

    public void OpenSettingsScreen()
    {
        //TODO implement this
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
