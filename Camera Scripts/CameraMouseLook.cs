using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CameraMouseLook {

    public Vector3 initialRotation;
    public Vector3 actualRotation;
    public Vector3 initialMousePosition;

    public float mouseLookSpeed = 0.2f;

    private CameraController _cameraController;

    public void Init()
    {
        _cameraController = CameraController.Singleton;

        if (_cameraController.target != null)
        {
            initialRotation = _cameraController.target.localRotation.eulerAngles;
            actualRotation = initialRotation;
        }
    }

    public void SetTargetRotation()
    {
        if (Input.GetMouseButtonDown(1))
        {
            initialMousePosition = Input.mousePosition;
            return;
        }

        if (Input.GetMouseButton(1))
        {
            Vector2 offset = initialMousePosition - Input.mousePosition;
            actualRotation = initialRotation + new Vector3(offset.y, -offset.x, 0f) * mouseLookSpeed;
        }

        if (Input.GetMouseButtonUp(1))
        {
            initialMousePosition = Input.mousePosition;
            initialRotation = actualRotation;
            return;
        }
    }
}
