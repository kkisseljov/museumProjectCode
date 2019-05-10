using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    public void OnResumeClick()
    {
        GameController.Singleton.paused = false;
        gameObject.SetActive(false);
    }

	public void OnQuitWithoutSavingClick()
	{
        GameController.Singleton.paused = false;
        SceneManager.LoadScene("MainMenu");
	}

	public void OnSaveAndQuitClick()
	{
        GameController.Singleton.paused = false;
        SceneManager.LoadScene("MainMenu");
    }
}
