using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// TEMPORARY -> Shitty code can be ignored.
/// TODO Rework UI to Canvas
/// </summary>
public class MapEditorController : MonoBehaviour
{
    public static MapEditorController Singleton { get; private set; }

    public BlockGridPrefabs editorBlockPrefabs;

    bool mainMenu = true;
    bool saveMenu = false;
   
    bool editMode = true;

    bool saveProccessModal = false;
    bool saveInProccess = false;
    bool saved = false;

    bool scenarioModal = false;

    int gridWidth = 100;
    int gridLength = 100;
    int startingPosX = 50;
    int startingPosY = 30;
    string gridWidthStr = "100";
    string gridLengthStr = "100";
    string startingPosXStr = "50";
    string startingPosYStr = "30";

    string mapName = "NewMap " + DateTime.Now.ToString("MM-dd-yyyy--HH-mm-ss");
    string mapToLoad = "";
    string[] maps;

    Regex integerOnly = new Regex(@"^\d+$");

    private int toolbarInt = 0;
    private string[] toolbarStrings;

    public BlockType selectedBlockType
    {
        get
        {
            return editorBlockPrefabs.blockPrefabs[toolbarInt].type;
        }
    }

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    { 
        toolbarStrings = editorBlockPrefabs.blockPrefabs.Select(x => x.prefab.name).ToArray();

        GridCoordinates startingPosition = BlockGridController.Singleton.grid.startingPosition;
        CameraControllerOld.Singleton.freeRoamMovingPoint.position = startingPosition.GetPositionOffset();
    }

    private void OnGUI()
    {
        int modalWidth = (int)(Screen.width * 0.5f);
        int modalHeight = (int)(Screen.height * 0.7f);
        Rect modalWindowRect = new Rect(
            Screen.width / 2 - modalWidth / 2,
            Screen.height / 2 - modalHeight / 2,
            modalWidth,
            modalHeight
        );

        int alertModalWidth = 300;
        int alertModalHeight = 150;
        Rect alertModalWindowRect = new Rect(
            Screen.width / 2 - alertModalWidth / 2,
            Screen.height / 2 - alertModalHeight / 2,
            alertModalWidth,
            alertModalHeight
        );

        MapEditorBlockSelector.Singleton.enabled = !saveMenu && !saveProccessModal;

        if (saveProccessModal && !saveMenu)
        {
            GUI.Window(1, alertModalWindowRect, SaveProcessModal, mapName);
        }

        if (mainMenu)
        {
            MainMenu();
        }

        if (editMode)
        {
            toolbarInt = GUI.Toolbar(new Rect(Screen.width / 2 - 150, Screen.height - 100, 250, 30), toolbarInt, toolbarStrings);
        }

        if (saveMenu && !saveProccessModal)
        {
            GUI.Window(0, modalWindowRect, SaveModal, "Save map");
        }
    }

    void MainMenu()
    {
        if (editMode && GUI.Button(new Rect(10, 140, 100, 30), "Save"))
        {
            saveMenu = true;

            maps = MapData.GetMapsInDirectory();

            Debug.Log((int)(Screen.width * 0.5f) + " " + (int)(Screen.height * 0.7f));
        }
        
        if (GUI.Button(new Rect(10, 220, 100, 30), "Quit"))
        {
            if (SceneSwitcher.Singleton)
            {
                SceneSwitcher.Singleton.QuitToMainMenu();
            }
            else
            {
                SceneManager.LoadScene("MainMenu");
            }
        }
    }

    void SaveModal(int id)
    {
        int modalWidth = (int)(Screen.width * 0.5f);
        int modalHeight = (int)(Screen.height * 0.7f);

        GUI.Label(new Rect(10, 30, 100, 30), "Map name: ");
        mapName = GUI.TextField(new Rect(90, 30, 200, 30), mapName, 35);

        for(int i = 0; i < maps.Length; i++)
        {
            if(GUI.Button(new Rect(10, 70 + 40 * i, 560, 30), maps[i]))
            {
                mapName = maps[i];
            }
        }

        if (GUI.Button(new Rect(10, modalHeight - 40, 100, 30), "Save"))
        {
            saveMenu = false;
            saveProccessModal = true;
            saveInProccess = true;

            MapData newMap = new MapData(mapName, BlockGridController.Singleton.grid);
            if(newMap.Save())
            {
                saved = true;
                saveInProccess = false;
            } else
            {
                saved = false;
                saveInProccess = false;
            }
        }

        if (GUI.Button(new Rect(modalWidth - 110, modalHeight - 40, 100, 30), "Cancel"))
        {
            saveMenu = false;
        }
    }

    /*
    void LoadModal(int id)
    {
        int modalWidth = (int)(Screen.width * 0.5f);
        int modalHeight = (int)(Screen.height * 0.7f);

        GUI.Label(new Rect(10, 30, 100, 30), "Map name: ");
        GUI.TextField(new Rect(90, 30, 200, 30), mapToLoad, 35);

        for (int i = 0; i < maps.Length; i++)
        {
            if (GUI.Button(new Rect(10, 70 + 40 * i, 560, 30), maps[i]))
            {
                mapToLoad = maps[i];
            }
        }

        if (mapToLoad != "" && GUI.Button(new Rect(10, modalHeight - 40, 100, 30), "Load"))
        {
            loadMenu = false;
            loadProccessModal = true;
            loadInProcess = true;

            MapData loadedMapData = MapData.Load(mapToLoad);
            BlockGridController.Singleton.LoadGrid(loadedMapData);

            loadInProcess = false;
        }

        if (GUI.Button(new Rect(modalWidth - 110, modalHeight - 40, 100, 30), "Cancel"))
        {
            loadMenu = false;
        }
    }
    */

    void SaveProcessModal(int id)
    {
        if(saveInProccess)
        {
            GUI.Label(new Rect(10, 30, 280, 30), "Saving...");
        }
        

        if(!saveInProccess)
        {
            GUI.Label(new Rect(10, 30, 280, 30), saved ? "Saved" : "Failed to save - see console for details");

            if (GUI.Button(new Rect(150, 110, 100, 30), "OK"))
            {
                saveMenu = false;
                saveProccessModal = false;

                Debug.Log("Clicked OK on Save Proccess modal");
            }
        }
    }
}
