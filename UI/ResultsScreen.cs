using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultsScreen : MonoBehaviour {

    public Text title;
    public Text minedShale;
    public Text targetShaleAmount;
    public Text shiftsRemaining;
    public Text score;

    private bool scenarioDone = false;
    private GameplayData _data;

    private void Start()
    {
        if(SceneSwitcher.Singleton.sceneLoaderData != null && SceneSwitcher.Singleton.sceneLoaderData.GetType() == typeof(NextShiftLoaderData))
        {
            NextShiftLoaderData data = SceneSwitcher.Singleton.sceneLoaderData as NextShiftLoaderData;
            title.text += data.gameplayData.progressData.shiftsPassed;
            minedShale.text = data.gameplayData.progressData.totalShaleMined.ToString();
            targetShaleAmount.text = data.gameplayData.scenarioData.goal.targetShaleAmount.ToString();
            shiftsRemaining.text = (data.gameplayData.scenarioData.goal.numberOfShifts - data.gameplayData.progressData.shiftsPassed).ToString()
                + " out of "
                + data.gameplayData.scenarioData.goal.numberOfShifts;

            scenarioDone = data.gameplayData.progressData.shiftsPassed == data.gameplayData.scenarioData.goal.numberOfShifts;

            _data = data.gameplayData;
        }
    }

    public void ToMainMenu()
    {
        SceneSwitcher.Singleton.QuitToMainMenu();
    }

    public void Continue()
    {
        if(!scenarioDone)
        {
            if(_data.Save())
            {
                Debug.Log("Saved");
            }

            SceneSwitcher.Singleton.PrepareToLoadLevel();
        } else
        {
            SceneSwitcher.Singleton.QuitToMainMenu();
        }
    }
}
