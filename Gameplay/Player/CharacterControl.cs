using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityStandardAssets.SceneUtils;

public class CharacterControl : MonoBehaviour {

    [Serializable]
    public class CharacterTools
    {
        public GameObject pickaxe;
    }

    public enum Status { Idle, PerformingAction, GoingHome, Inactive }

    public Transform cameraTarget;
    public Status status = Status.Idle;

    public Character character = new Character(new Character.Inventory(100, 0));

    public List<CharacterAction> currentActions = new List<CharacterAction>();

    public GameObject highlightSprite;

    public CharacterTools tools = new CharacterTools();

    private AICharacterControl _control;
    private PlaceTargetWithMouse _targetPicker;
    private Animator _animator;

    public ActionToolbarItem selectedActionInToolbar
    {
        get
        {
            return ActionToolbar.Singleton.selectedAction;
        }
    }

    public PlaceTargetWithMouse targetPicker
    {
        get
        {
            if (_targetPicker == null)
            {
                Debug.Log("Spawn target picker for: " + name);
                SpawnTargetPicker();
            }

            return _targetPicker;
        }
    }

    public Animator animator
    {
        get
        {
            return _animator;
        }
    }

    public bool ReachedDestination
    {
        get
        {
            return _control.agent.remainingDistance <= _control.agent.stoppingDistance;
        }
    }

    public void Awake()
    {
        _control = GetComponent<AICharacterControl>();
        _animator = GetComponent<Animator>();
    }

    public void Start()
    {
        Debug.Log("Character " + name + " enabled");
        SpawnTargetPicker();
    }

    public void Update()
    {
        if (_targetPicker != null)
        {
            if (_control.agent.remainingDistance <= _control.agent.stoppingDistance)
            {
                _control.target = null;
                _targetPicker.transform.GetChild(0).gameObject.SetActive(false);
            }
            else
            {
                _targetPicker.transform.GetChild(0).gameObject.SetActive(true);
            }
        }

        ProcessActions();
    }

    public void AttachCamera()
    {
        CameraControllerOld.Singleton.AttachToTarget(cameraTarget);
    }

    public void MoveTo(Transform transform)
    {
        if(status == Status.GoingHome || status == Status.Inactive)
        {
            return;
        }

        StopAllActions();

        SendMessage("SetTarget", transform);
    }

    public void MineBlock(BlockRenderer renderer)
    {
        Debug.Log("CharacterControl -> MineBlock");

        StopAllActions();

        if (!character.inventory.IsFull && GameController.Singleton.isShiftActive)
        {
            currentActions.Add(new MineAction(renderer, this));
        }
    }

    void GoHome()
    {
        Debug.Log("CharacterControl -> GoHome");

        StopAllActions();

        status = Status.GoingHome;
        GoHomeAction action = new GoHomeAction(this);
        currentActions.Add(action);
        action.Start();
    }

    public void OnBlockClicked(BlockRenderer renderer)
    {
        Debug.Log("CharacterControl -> OnBlockClicked");

        switch(selectedActionInToolbar.actionName)
        {
            case "Mine":
                MineBlock(renderer);
                break;
            default:
                break;
        }
    }

    /*      Perhaps will not be needed
    *   
   public void MoveTo(Vector3 position)
   {
       MoveAction moveAction = new MoveAction(position, null);

       currentActions.Clear();     //TODO temporary, TBD if moving should cancel all actions

       currentActions.Add(moveAction);
   }
   */

    private void ProcessActions()
    {
        if(status == Status.Inactive)
        {
            return;
        }

        if(!GameController.Singleton.isShiftActive)
        {
            if(status != Status.GoingHome)
            {
                GoHome();
            } else
            {
                currentActions[0].Perform();
            }
        }

        CharacterAction completedAction = currentActions.Find(x => x.isDone);

        if (completedAction != null)
        {
            currentActions.Remove(completedAction);
            status = Status.Idle;
        }

        if (status == Status.Idle)
        {
            CharacterAction actionToStart = currentActions.Find(x => !x.isDone && !x.inProgress);

            if (actionToStart != null)
            {
                actionToStart.Start();

                status = Status.PerformingAction;
            }
        }
        else
        {
            CharacterAction actionInProgress = currentActions.Find(x => !x.isDone && x.inProgress);

            if (actionInProgress != null)
            {
                actionInProgress.Perform();
            }
        }
    }

    private void StopAllActions()
    {
        currentActions.FindAll(x => x.inProgress).ForEach(x => x.Stop());
        currentActions.Clear();
        status = Status.Idle;
    }

    private void SpawnTargetPicker()
    {
        _targetPicker = Instantiate(
                    CharactersController.Singleton.targetPickerPrefab,
                    BlockGridController.Singleton.grid.startingPosition.GetPositionOffset(),
                    Quaternion.Euler(Vector3.zero)
                ).GetComponent<PlaceTargetWithMouse>();

        _targetPicker.name = name + "'s target picker";
        _targetPicker.setTargetOn = gameObject;

        //Will be activated by buttons on UnitToolbar
        _targetPicker.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(name + " OnTriggerEnter() -> " + other.name);

        if(other.name == "UnloadSpot")
        {
            UnloadShale();
        } else if(other.name == "Unspawn" && status == Status.GoingHome)    //Workaround, checking if agent has reached destination doesn't seem to be stable
        {
            status = Status.Inactive;
            StopAllActions();
            CharactersController.Singleton.UnspawnPlayerCharacter(this);
        }
    }

    //TODO can be an action in future
    void UnloadShale()
    {
        if(ScenarioProgressController.Singleton != null)
        {
            ScenarioProgressController.Singleton.progressData.totalShaleMined += character.inventory.shaleAmount;
        }
        
        character.inventory.shaleAmount = 0;
    }
}
