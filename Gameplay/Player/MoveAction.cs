using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO implement this if needed
public class MoveAction {

    public Vector3 moveToPosition;
    public CharacterControl characterControl;

    public MoveAction(Vector3 moveToPosition, CharacterControl characterControl)
    {
        this.moveToPosition = moveToPosition;
        this.characterControl = characterControl;
    }

    public MoveAction(Transform transform, CharacterControl characterControl)
    {
        moveToPosition = transform.position;
        this.characterControl = characterControl;
    }
}
