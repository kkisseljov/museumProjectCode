using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScenarioSelector : MonoBehaviour, ISelectHandler
{
    public string scenarioName;
    public ScenarioEditorMenu scenarioEditorMenu;
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
        scenarioEditorMenu.SelectScenario(this);

        data = LevelScenarioData.Load(scenarioName);
        scenarioEditorMenu.SetScenarioOverview(data);

        Debug.Log("Loaded: " + data.description);
    }

    public void OnDestroy()
    {
        Debug.Log("On Scenario selector destroy " + name);
    }
}
