using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CameraZoom {

    public float maxDistance = 15f;     //TODO move to const
    public float minDistance = 5f;      //TODO move to const
    public float currentDistance = 10f;
   
    public float scrollSpeed = 5f;

    private CameraController _cameraController;

    public void Init()
    {
        _cameraController = CameraController.Singleton;
    }

    public void SetCameraDistance()
    {
        float scrolledDistance = Input.GetAxis("Mouse ScrollWheel") * -1;

        if (_cameraController.placeHolder != null)
        {
            float _currentDistance = _cameraController.placeHolder.localPosition.z * -1;
            float _newDistance = currentDistance + scrolledDistance * scrollSpeed;

            if (_newDistance < minDistance)
            {
                _newDistance = minDistance;
            }

            if (_newDistance > maxDistance)
            {
                _newDistance = maxDistance;
            }

            currentDistance = _newDistance;
        }
    }
}
