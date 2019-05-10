using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterAction {
    public bool isDone;
    public bool inProgress;
    public CharacterControl characterControl;

    protected CharacterAction(CharacterControl characterControl)
    {
        this.characterControl = characterControl;
    }

    public abstract void Start();
    public abstract void Perform();
    public abstract void Stop();
}
