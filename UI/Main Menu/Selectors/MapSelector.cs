using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//TODO refactor -> Selectors have plenty common things
public class MapSelector : MonoBehaviour, ISelectHandler
{
    public string mapName;
    public LoadMapMenu loadMapMenu;
    private Button _button;

    private void Start()
    {
        _button = GetComponent<Button>();
        Text text = _button.GetComponentInChildren<Text>();
        text.text = mapName;
    }

    public void OnSelect(BaseEventData eventData)
    {
        loadMapMenu.SelectMap(this);
    }
}
