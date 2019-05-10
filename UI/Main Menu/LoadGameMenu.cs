using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadGameMenu : MonoBehaviour {

    public Transform grid;

    public GameObject buttonPrefab;

    private string[] savedGames;
    private List<SavedGameSelector> savedGameSelectors = new List<SavedGameSelector>();
    private SavedGameSelector selectedSavedGame;

    private void Start()
    {
        if(grid == null || buttonPrefab == null)
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
        savedGames = GameplayData.GetMapsInDirectory();

        Debug.Log("maps count in directory -> " + savedGames.Length);

        for (int i = 0; i < savedGames.Length; i++)
        {
            Button button = Instantiate(buttonPrefab, grid).GetComponent<Button>();
            SavedGameSelector gameSelector = button.gameObject.AddComponent<SavedGameSelector>();
            gameSelector.saveName = savedGames[i];
            gameSelector.loadGameMenu = this;
        }
    }

    private void ClearGrid()
    {

        savedGameSelectors.Clear();
        SavedGameSelector[] buttons = grid.GetComponentsInChildren<SavedGameSelector>();

        for (int i = 0; i < buttons.Length; i++)
        {
            Destroy(buttons[i].gameObject);
        }

        Debug.Log("ClearGrid -> " + savedGameSelectors.Count + " / " + grid.childCount);
    }

    public void SelectSavedGame(SavedGameSelector savedGame)
    {
        selectedSavedGame = savedGame;
    }

    public void DeleteMap()
    {
        if (selectedSavedGame != null)
        {
            GameplayData.Delete(selectedSavedGame.saveName);
            ClearGrid();
            FetchGrid();
            selectedSavedGame = null;
        }
    }

    public void OnLoadButtonClicked()
    {
        if (selectedSavedGame != null)
        {
            Debug.Log("Load " + selectedSavedGame.saveName);

            NextShiftLoaderData data = new NextShiftLoaderData(GameplayData.Load(selectedSavedGame.saveName));

            SceneSwitcher.Singleton.PrepareToLoadLevel(data);
        }
    }
}
