using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[Obsolete]
public class LoadMapFromResourcesMenu : MonoBehaviour {

    public class MapSelector : MonoBehaviour, ISelectHandler
    {
        public string mapName;
        public LoadMapFromResourcesMenu loadMapMenu;
        private Button _button;

        private void Start()
        {
            _button = GetComponent<Button>();
            Text text = _button.GetComponentInChildren<Text>();
            text.text = mapName;
        }

        public void OnSelect(BaseEventData eventData)
        {
            //loadMapMenu.SelectMap(this);
        }

        public void OnDestroy()
        {
            Debug.Log("On MapSelector destroy " + name);
        }

        public void DestroyThis()
        {
            GameObject.Destroy(this.gameObject, 0f);
        }
    }

    public Transform grid;

    public Button buttonPrefab;

    private string[] maps;
    private List<MapSelector> mapSelectors = new List<MapSelector>();
    private MapSelector selectedMap;

    /*
    private void Start()
    {
        if (grid == null || buttonPrefab == null)
        {
            Debug.LogError("Some objects are not set for LoadGameMenu!");
            enabled = false;
            return;
        }

        FetchGrid();
    }

    public void Refresh()
    {
        ClearGrid();
        FetchGrid();
    }

    private void FetchGrid()
    {
        maps = MapData.GetMapsInResources();

        Debug.Log("maps count in directory -> " + maps.Length);

        for (int i = 0; i < maps.Length; i++)
        {
            Button button = Instantiate(buttonPrefab, grid);
            MapSelector mapSelector = button.gameObject.AddComponent<MapSelector>();
            mapSelector.mapName = maps[i];
            mapSelector.loadMapMenu = this;
        }
    }

    private void ClearGrid()
    {

        mapSelectors.Clear();
        MapSelector[] buttons = grid.GetComponentsInChildren<MapSelector>();

        for (int i = 0; i < buttons.Length; i++)
        {
            Destroy(buttons[i].gameObject);
        }

        Debug.Log("ClearGrid -> " + mapSelectors.Count + " / " + grid.childCount);
    }

    public void SelectMap(MapSelector mapSelector)
    {
        selectedMap = mapSelector;
    }

    public void OnLoadButtonClicked()
    {
        if (selectedMap != null)
        {
            Debug.Log("Load " + selectedMap.mapName);

            SavedMapData data = new SavedMapData(selectedMap.mapName, true);

            SceneSwitcher.Singleton.PrepareToLoadLevel(data);
        }
    }
    */
}
