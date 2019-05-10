using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SavedGameSelector : MonoBehaviour, ISelectHandler
{

    public string saveName;
    public LoadGameMenu loadGameMenu;
    private Button _button;

    private void Start()
    {
        _button = GetComponent<Button>();
        Text text = _button.GetComponentInChildren<Text>();
        text.text = saveName;
    }

    public void OnSelect(BaseEventData eventData)
    {
        loadGameMenu.SelectSavedGame(this);
    }
}
