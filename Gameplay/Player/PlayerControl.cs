using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

//TODO implement this
[RequireComponent(typeof(UnityStandardAssets.Characters.ThirdPerson.AICharacterControl))]
[RequireComponent(typeof(UnityStandardAssets.Characters.ThirdPerson.ThirdPersonUserControl))]
public class PlayerControl : MonoBehaviour {
    
    private UnityStandardAssets.Characters.ThirdPerson.AICharacterControl _AICharacterControl;

	// Use this for initialization
	void Start () {
        _AICharacterControl = this.GetComponent<UnityStandardAssets.Characters.ThirdPerson.AICharacterControl>();
    }
	
	// Update is called once per frame
	void Update () {
        float horizontalInput = CrossPlatformInputManager.GetAxis("Horizontal");
        float verticalInput = CrossPlatformInputManager.GetAxis("Vertical");

        if(horizontalInput != 0 || verticalInput != 0)
        {
            _AICharacterControl.target = null;
            _AICharacterControl.agent.isStopped = true;
        }
    }
}
