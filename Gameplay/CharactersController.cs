using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityStandardAssets.SceneUtils;

//TODO rename this
public class CharactersController : MonoBehaviour {
    public static CharactersController Singleton { get; private set; }

    public GameObject characterPrefab;

    //TODO move this somewhere, shouldn't be here
    public GameObject targetPickerPrefab;

    public List<CharacterControl> activeCharacters = new List<CharacterControl>();
    public CharacterControl selectedCharacter = null;

    public bool spawnCharacter = false;
    public float spawnDelay = 1.5f;

    private void Awake()
    {
        Singleton = this;
    }

    private void Update()
    {
        
    }

    //TODO not able to spawn on start, needs to be reworked
    public IEnumerator SpawnPlayerCharacter()
    {
        yield return new WaitForSeconds(spawnDelay);

        if (BlockGridRenderer.Singleton != null && BlockGridRenderer.Singleton.entrance != null)
        {

            Vector3 characterSpawnPosition = BlockGridRenderer.Singleton.entrance.spawnPositions[activeCharacters.Count % 2].position;
            CharacterControl character = Instantiate(characterPrefab, characterSpawnPosition, Quaternion.Euler(Vector3.zero))
                .GetComponent<CharacterControl>();
            character.name = "Character" + (activeCharacters.Count + 1);

            activeCharacters.Add(character);

            GridCoordinates startPos = BlockGridController.Singleton.grid.startingPosition;
            GridCoordinates randomCoordinatesInsideRoom = new GridCoordinates(
                startPos.x + Random.Range(BlockGrid.startingRoomLeftRearCorner.x, BlockGrid.startingRoomFrontRightCorner.x),
                startPos.y + Random.Range(BlockGrid.startingRoomLeftRearCorner.y, BlockGrid.startingRoomFrontRightCorner.y)
            );

            UnitToolbar.Singleton.AddUnitButton(character.name);

            yield return new WaitForSeconds(0.1f);

            //TODO rework and refactor target picker script. Need some kind of Move() method for CharacterControl to move him to given Vector3
            PlaceTargetWithMouse targetPicker = character.targetPicker;
            targetPicker.transform.position = randomCoordinatesInsideRoom.GetPositionOffset(targetPicker.surfaceOffset);
            character.transform.SendMessage("SetTarget", targetPicker.transform);
        }

        yield return null;
    }

    public void UnspawnPlayerCharacter(CharacterControl characterControl)
    {
        UnitToolbar.Singleton.RemoveUnitButton(characterControl.name);
        activeCharacters.Remove(characterControl);
        Destroy(characterControl.gameObject);

        Debug.Log(characterControl.name + " should be destroyed");

        if(activeCharacters.Count == 0)
        {
            GameController.Singleton.EndShift();
        }
    }

    public void SelectCharacter(int index)
    {
        if(index < 0 || index >= activeCharacters.Count)
        {
            Debug.LogError("Wrong character index!");
            return;
        }

        activeCharacters.ForEach(x =>
        {
            x.targetPicker.enabled = false;
            x.highlightSprite.SetActive(false);
        });

        selectedCharacter = activeCharacters[index];
        selectedCharacter.targetPicker.enabled = true;
        selectedCharacter.AttachCamera();
        selectedCharacter.highlightSprite.SetActive(true);
    }

    //TODO temp
    /*
    private void OnGUI()
    {
        if(GameController.Singleton.paused)
        {
            return;
        }

        for (int i = 0; i < activeCharacters.Count; i++)
        {
            if (GUI.Button(new Rect(10, 50 + 50 * i, 100, 40), activeCharacters[i].name))
            {
                selectedCharacter = activeCharacters[i];
                selectedCharacter.targetPicker.enabled = true;
                selectedCharacter.AttachCamera();
            }

            if (activeCharacters[i] != selectedCharacter) {
                activeCharacters[i].targetPicker.enabled = false;
            }
        }
    }
    */
}
