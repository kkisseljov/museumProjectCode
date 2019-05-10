using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditScenarioMenu : MonoBehaviour
{
    public enum Mode { Create, Edit };

    public Text label;
    public InputField nameInput;
    public InputField mapNameInput;
    public InputField descriptionInput;
    public InputField numberOfShiftsInput;
    public InputField targetShaleAmountInput;

    public Toggle numberOfShiftsToggle;
    public Toggle targetShaleAmountToggle;

    private Mode _mode = Mode.Create;
    private LevelScenarioData _data = null;

    public void OnSaveButtonClick()
    {
        string name = nameInput.text;
        string mapName = mapNameInput.text;
        string description = descriptionInput.text;

        int numberOfShifts = Int32.Parse(numberOfShiftsInput.text);
        int targetShaleAmount = Int32.Parse(targetShaleAmountInput.text);

        if (_mode == Mode.Create)
        {
            LevelScenarioData.Goal goal = new LevelScenarioData.Goal(numberOfShifts, targetShaleAmount);
            LevelScenarioData scenario = new LevelScenarioData(mapName, name, description, 0, goal);
            scenario.Save();
        }
        else if (_mode == Mode.Edit)
        {
            _data.goal = new LevelScenarioData.Goal(numberOfShifts, targetShaleAmount);
            _data.description = description;
            _data.mapFileName = mapName;
            _data.index = 0;

            _data.Save();
        }
    }

    private void OnEnable()
    {
        if (_mode == Mode.Edit)
        {
            label.text = "Edit Scenario";
            nameInput.text = _data.name;
            nameInput.readOnly = true;
            mapNameInput.text = _data.mapFileName;
            descriptionInput.text = _data.description;

            numberOfShiftsInput.text = _data.goal.numberOfShifts.ToString();
            targetShaleAmountInput.text = _data.goal.numberOfShifts.ToString();
        } else if (_mode == Mode.Create)
        {
            _data = null;

            nameInput.text = "Scenario " + Mathf.RoundToInt(UnityEngine.Random.value * 100000).ToString();
            nameInput.readOnly = false;
            mapNameInput.text = "";
            descriptionInput.text = "";

            numberOfShiftsInput.text = "0";
            targetShaleAmountInput.text = "0";

            numberOfShiftsToggle.isOn = true;
            targetShaleAmountToggle.isOn = true;
        }
    }

    public void SetEditMode(LevelScenarioData data)
    {
        _mode = Mode.Edit;
        _data = data;
    }

    public void SetCreateMode()
    {
        _mode = Mode.Create;
        label.text = "New Scenario";
    }
}
