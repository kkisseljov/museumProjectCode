using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//TODO refactor toolbars, too common functionality
public class ActionToolbar : MonoBehaviour
{
    public static ActionToolbar Singleton { get; private set; }

    public ActionToolbarItem selectedAction;

    public List<ActionToolbarItem> buttons = new List<ActionToolbarItem>();

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(buttons[buttons.Count - 1].gameObject, null);
    }

    private void Update()
    {
        if (buttons[0].isKeyPressed && buttons[0].interactable)
        {
            EventSystem.current.SetSelectedGameObject(buttons[0].gameObject, null);
        }

        if (buttons[buttons.Count - 1].isKeyPressed && buttons[buttons.Count - 1].interactable)
        {
            EventSystem.current.SetSelectedGameObject(buttons[buttons.Count - 1].gameObject, null);
        }
    }
}
