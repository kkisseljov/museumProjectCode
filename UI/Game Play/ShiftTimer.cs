using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShiftTimer : MonoBehaviour {

    public Text timerText;
    public Text shiftText;

    private void Start()
    {
        if(timerText == null || shiftText == null || GameController.Singleton == null)
        {
            Debug.LogError("ShiftTimer can not work due to missing GameController or text is not attached");
            gameObject.SetActive(false);
        } else
        {
            shiftText.text = "Shifts: "
                + (ScenarioProgressController.Singleton.progressData.shiftsPassed + 1).ToString()
                + "/"
                + ScenarioProgressController.Singleton.scenarioData.goal.numberOfShifts;
        }
    }

    void Update () {
        float currentTime = GameController.Singleton.currentShiftTime;
        int hours = Mathf.FloorToInt(currentTime / 3600);
        int minutes = Mathf.FloorToInt(currentTime / 60) % 60;

        timerText.text = hours.ToString() + ":" + (minutes < 10 ? "0" : "") + minutes.ToString();
	}
}
