using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//TODO refactor toolbars, too common functionality
public class UnitToolbar : MonoBehaviour {

    public static UnitToolbar Singleton { get; private set; }

    public List<Sprite> unitSprites = new List<Sprite>();
    public UnitToolbarItem buttonPrefab;
    public Transform listTransform;

    public UnitToolbarItem selectedUnit;

    public List<UnitToolbarItem> buttons = new List<UnitToolbarItem>();

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        //EventSystem.current.SetSelectedGameObject(buttons[buttons.Count - 1].gameObject, null);
    }

    private void Update()
    {
        buttons.ForEach(x =>
        {
            if(x.isKeyPressed && x.interactable)
            {
                EventSystem.current.SetSelectedGameObject(x.gameObject, null);
            }
        });
    }

    public void AddUnitButton(string name)
    {
        UnitToolbarItem button = Instantiate(buttonPrefab, listTransform);
        buttons.Add(button);
        button.unitName = name;
        Debug.Log("Setting Unit sprite -> buttons count: " + buttons.Count + "\t unit sprites count: " + unitSprites.Count + "\t result: " + buttons.Count % unitSprites.Count);
        button.unitImage.sprite = unitSprites[buttons.Count % unitSprites.Count];
        button.SetKeyCode(buttons.Count - 1);
        button.characterIndex = buttons.Count - 1;
    }

    public void RemoveUnitButton(string name)
    {
        UnitToolbarItem unitToDelete = buttons.Find(x => x.unitName == name);

        if(unitToDelete != null)
        {
            buttons.Remove(unitToDelete);
            Destroy(unitToDelete.gameObject);
        } else
        {
            Debug.LogError("Failed to find unit toolbar button with name: " + name);
        }
    }
}
