using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Obsolete("Has some unexpected stutter. Use CameraControllerOld for now.")]
public class CameraController : MonoBehaviour
{
    public static CameraController Singleton { get; private set; }

    public Transform target;
    public Transform placeHolder;

    public bool mouseLook;
    public bool zoom;

    private CameraMouseLook _mouseLook;
    private CameraZoom _zoom;

    private Transform _cameraTransofrm;

    private void Awake()
    {
        Singleton = this;

        _cameraTransofrm = Camera.main.transform;

        if (target == null || placeHolder == null)
        {
            Debug.LogError("Camera Controller:  placeholder and target are not set !");
            enabled = false;
            return;
        }

        if (mouseLook)
        {
            EnableMouseLook();
        }

        if (zoom)
        {
            EnableZoom();
        }
    }

    private void EnableMouseLook()
    {
        _mouseLook = new CameraMouseLook();
        _mouseLook.Init();
    }

    private void EnableZoom()
    {
        _zoom = new CameraZoom();
        _zoom.Init();
    }

    private void Update()
    {
        if (_mouseLook != null)
        {
            _mouseLook.SetTargetRotation();
        }

        if (_zoom != null)
        {
            _zoom.SetCameraDistance();
        }
    }

    private void LateUpdate()
    {
        if (_mouseLook != null)
        {
            target.rotation = Quaternion.Euler(_mouseLook.actualRotation);
        }

        if (_zoom != null)
        {
            Vector3 currentPos = placeHolder.localPosition;
            placeHolder.localPosition = new Vector3(currentPos.x, currentPos.y, -_zoom.currentDistance);
        }

        _cameraTransofrm.LookAt(target);
        _cameraTransofrm.position = placeHolder.position;
    }
}
