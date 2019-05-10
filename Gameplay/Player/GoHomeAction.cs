using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoHomeAction : CharacterAction
{
    public GoHomeAction(CharacterControl characterControl) : base(characterControl)
    {
    }

    public override void Perform()
    {
        /*
        if (characterControl.ReachedDestination)
        {
            Stop();
            CharactersController.Singleton.UnspawnPlayerCharacter(characterControl);
        }
        */
    }

    public override void Start()
    {
        characterControl.targetPicker.transform.position = BlockGridRenderer.Singleton.entrance.spawnPositions[0].position;
        characterControl.transform.SendMessage("SetTarget", characterControl.targetPicker.transform);

        Debug.Log(characterControl.name + " is starting going home");

        inProgress = true;
    }

    //TODO seems to be a common logic, move to super ?
    public override void Stop()
    {
        Debug.Log(characterControl.name + " is finished going home");

        inProgress = false;
        isDone = true;

        characterControl.status = CharacterControl.Status.Inactive;
    }
}
