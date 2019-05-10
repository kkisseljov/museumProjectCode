using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionContextMenu : MonoBehaviour
{
    //TODO move this to other file so it can be attached to button from editor
    public class ActionSelector : MonoBehaviour
    {
        public PlayerAction action;
        public ActionContextMenu menu;
        private Button _button;

        private void Start()
        {
            _button = GetComponent<Button>();
            Text text = _button.GetComponentInChildren<Text>();
            //text.text = action.name;      //TODO replace by icon
        }

        public void OnClick()
        {
            menu.OnActionClick(this);
        }
    }

    public List<Button> actionSelectorButtons = new List<Button>(); //TODO temp
    private List<ActionSelector> _actionSelectors = new List<ActionSelector>();

    private void Update()
    {
        transform.LookAt(Camera.main.transform);
    }

    //TODO temp
    private void Awake()
    {
        actionSelectorButtons.ForEach(x => _actionSelectors.Add(x.gameObject.AddComponent<ActionSelector>()));
        _actionSelectors.ForEach(x => x.menu = this);
    }

    public void Show(List<PlayerAction> actions)
    {
        _actionSelectors.ForEach(x => x.gameObject.SetActive(false));

        for (int i = 0; i < actions.Count; i++)
        {
            if(i >= _actionSelectors.Count)
            {
                Debug.LogError("Not enough buttons to set all actions");
            } else
            {
                _actionSelectors[i].action = actions[i];
                _actionSelectors[i].gameObject.SetActive(true);
            }
        }
    }

    public void OnActionClick(ActionSelector actionSelector)
    {
        Debug.Log(actionSelector.action.name + " clicked");
        gameObject.SetActive(false);
    }
}
