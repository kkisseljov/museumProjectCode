using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//TODO refactor toolbars, too common functionality
public class UnitToolbarItem : MonoBehaviour, ISelectHandler
{
    public string unitName = "None";
    public int characterIndex = 0;

    public Image unitImage;
    public Text unitNameLabel;
    public Text keyCodeLabel;
    public Text statusLabel;
    public Text fullInventoryNotification;

    private KeyCode _keyCode = KeyCode.F1;
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
            return Input.GetKey(_keyCode);
        }
    }

    public void SetKeyCode(int index)
    {
        switch(index)
        {
            case 0:
                _keyCode = KeyCode.F1;
                break;
            case 1:
                _keyCode = KeyCode.F2;
                break;
            case 2:
                _keyCode = KeyCode.F3;
                break;
            case 3:
                _keyCode = KeyCode.F4;
                break;
            case 4:
                _keyCode = KeyCode.F5;
                break;
            case 5:
                _keyCode = KeyCode.F6;
                break;
            case 6:
                _keyCode = KeyCode.F7;
                break;
            default:
                _keyCode = KeyCode.F12;
                break;
        }
    }

    private void Start()
    {
        _button = GetComponent<Button>();
        unitNameLabel.text = unitName;
        keyCodeLabel.text = _keyCode.ToString();
    }

    //TODO temp, rework when more character actions are implemented
    private void Update()
    {
        CharacterControl control = CharactersController.Singleton.activeCharacters[characterIndex];
        if(control != null)
        {
            //statusLabel.text = control.currentActions.Count.ToString();

            if(control.status == CharacterControl.Status.PerformingAction && control.currentActions.Count > 0)
            {
                CharacterAction currentAction = control.currentActions.Find(x => x.inProgress);

                if(currentAction != null && currentAction.GetType() == typeof(MineAction))
                {
                     statusLabel.text = "Mining";
                }

            } else if (!control.ReachedDestination)
            {
                statusLabel.text = "Moving";
            } else
            {
                statusLabel.text = "Idle";
            }

            fullInventoryNotification.gameObject.SetActive(control.character.inventory.IsFull);
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        UnitToolbar.Singleton.selectedUnit = this;
        CharactersController.Singleton.SelectCharacter(characterIndex);
    }
}
