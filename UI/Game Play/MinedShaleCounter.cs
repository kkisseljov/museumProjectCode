using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinedShaleCounter : MonoBehaviour {
    public Text minedShaleAmountText;
    public Text targetShaleAmountText;

    private void Start()
    {
        if (ScenarioProgressController.Singleton == null)
        {
            return;
        }

        if (ScenarioProgressController.Singleton.scenarioData == null || ScenarioProgressController.Singleton.progressData == null)
        {
            Debug.LogError("ScenarioProgressController is empty!");
            //gameObject.SetActive(false);
            return;
        }

        int targetShaleAmount = ScenarioProgressController.Singleton.scenarioData.goal.targetShaleAmount;
        if(targetShaleAmount > 0)
        {
            targetShaleAmountText.text = "Target shale amount: " + targetShaleAmount.ToString();
        } else
        {
            targetShaleAmountText.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (ScenarioProgressController.Singleton == null)
        {
            return;
        }

        minedShaleAmountText.text = "Mined shale: " + ScenarioProgressController.Singleton.progressData.totalShaleMined;
    } 
}
