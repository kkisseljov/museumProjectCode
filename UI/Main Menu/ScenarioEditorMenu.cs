using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//TODO refactor this
public class ScenarioEditorMenu : MonoBehaviour {
   
    public EditScenarioMenu editScenarioMenu;
    public Transform grid;

    public Button buttonPrefab;

    public Text contentText;

    private string[] scenarios;
    private List<ScenarioSelector> scenarioSelectors = new List<ScenarioSelector>();
    private ScenarioSelector selectedScenario;

    private void Start()
    {
        if (grid == null || buttonPrefab == null)
        {
            Debug.LogError("Some objects are not set for LoadGameMenu!");
            enabled = false;
            return;
        }

        Refresh();
    }

    private void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        ClearGrid();
        FetchGrid();
        contentText.text = "";
    }

    private void FetchGrid()
    {
        scenarios = LevelScenarioData.GetScenariosInDirectory();

        Debug.Log("scenarios count in directory -> " + scenarios.Length);

        for (int i = 0; i < scenarios.Length; i++)
        {
            Button button = Instantiate(buttonPrefab, grid);
            ScenarioSelector scenarioSelector = button.gameObject.AddComponent<ScenarioSelector>();
            scenarioSelector.scenarioName = scenarios[i];
            scenarioSelector.scenarioEditorMenu = this;
        }
    }

    private void ClearGrid()
    {

        scenarioSelectors.Clear();
        ScenarioSelector[] buttons = grid.GetComponentsInChildren<ScenarioSelector>();

        for (int i = 0; i < buttons.Length; i++)
        {
            Destroy(buttons[i].gameObject);
        }

        Debug.Log("ClearGrid -> " + scenarioSelectors.Count + " / " + grid.childCount);
    }

    public void SelectScenario(ScenarioSelector scenarioSelector)
    {
        selectedScenario = scenarioSelector;
    }

    public void OnNewButtonClicked()
    {
        editScenarioMenu.SetCreateMode();
        editScenarioMenu.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OnEditButtonClicked()
    {
        if (selectedScenario != null)
        {
            editScenarioMenu.SetEditMode(selectedScenario.data);
            editScenarioMenu.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    public void OnDeleteButtonClicked()
    {
        if (selectedScenario != null)
        {
            LevelScenarioData.Delete(selectedScenario.scenarioName);
            Refresh();
        }
    }

    public void SetScenarioOverview(LevelScenarioData data)
    {
        contentText.text = data.ToString();
    }
}
