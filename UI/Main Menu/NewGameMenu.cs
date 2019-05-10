using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NewGameMenu : MonoBehaviour {

    public class ScenarioSelector : MonoBehaviour, ISelectHandler
    {
        public string scenarioName;
        public NewGameMenu newGameMenu;
        public LevelScenarioData data = null;
        private Button _button;

        private void Start()
        {
            _button = GetComponent<Button>();
            Text text = _button.GetComponentInChildren<Text>();
            text.text = scenarioName;
        }

        public void OnSelect(BaseEventData eventData)
        {
            newGameMenu.SelectScenario(this);

            data = LevelScenarioData.Load(scenarioName);
            newGameMenu.SetScenarioOverview(data);

            Debug.Log("Loaded: " + data.description);
        }

        public void OnDestroy()
        {
            Debug.Log("On Scenario selector destroy " + name);
        }
    }

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
            scenarioSelector.newGameMenu = this;
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

    public void SetScenarioOverview(LevelScenarioData data)
    {
        contentText.text = data.ToString();
    }

    public void OnStartButtonClick()
    {
        if(selectedScenario != null)
        {
            SavedMapLoaderData savedMapLoaderData = new SavedMapLoaderData(selectedScenario.data.mapFileName);
            GamePlayLoaderData loaderData = new GamePlayLoaderData(savedMapLoaderData, selectedScenario.data);
            SceneSwitcher.Singleton.PrepareToLoadLevel(loaderData);
        }
    }
}
