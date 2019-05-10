using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//TODO refactor toolbars, too common functionality
public class ActionToolbarItem : MonoBehaviour, ISelectHandler {

    public string actionName = "None";
    public KeyCode keyCode;

    private Button _button;

    public bool interactable
    {
        get
        {
            return _button.interactable;
        }
    }

    public bool isKeyPressed
    {
        get
        {
            return Input.GetKey(keyCode);
        }
    }

    private void Start()
    {
        _button = GetComponent<Button>();
    }

    public void OnSelect(BaseEventData eventData)
    {
        ActionToolbar.Singleton.selectedAction = this;
    }
}
