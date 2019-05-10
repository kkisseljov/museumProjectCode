using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class CameraControllerOld : MonoBehaviour {
    public static CameraControllerOld Singleton { get; private set; }

    public Transform target;
    public Transform placeHolder;

    public Transform freeRoamMovingPoint;
    public Transform freeRoamTarget;
    public Transform freeRoamPlaceholder;

    public Vector3 initialRotation;
    public Vector3 actualRotation;

    public Vector3 initialMousePosition;

    public float maxDistance = 15f;
    public float minDistance = 5f;
    public float currentDistance = 10f;

    public float mouseLookSpeed = 0.1f;
    public float scrollSpeed = 5f;

    public bool freeRoam = false;
    public float freeRoamSpeed = 1f;
    private Vector3 movement = Vector3.zero;

    private void Awake()
    {
        Singleton = this;
    }

    private void Start() {
        if (!freeRoam && target != null)
        {
            initialRotation = target.localRotation.eulerAngles;
            actualRotation = initialRotation;
        } else if( freeRoam && freeRoamTarget != null && freeRoamMovingPoint != null)
        {
            initialRotation = new Vector3(freeRoamTarget.localRotation.eulerAngles.x, freeRoamMovingPoint.eulerAngles.y);
            actualRotation = initialRotation;
        }
    }

    private void Update()
    {
        SetTargetRotation();
        SetCameraDistance();
        FreeRoam();
    }

    private void LateUpdate()
    {
        if (target != null && placeHolder != null && !freeRoam)
        {
            target.rotation = Quaternion.Euler(actualRotation);

            Vector3 currentPos = placeHolder.localPosition;
            placeHolder.localPosition = new Vector3(currentPos.x, currentPos.y, -currentDistance);
            
            Camera.main.transform.position = placeHolder.position;
            Camera.main.transform.LookAt(target);
        }

        if(freeRoamTarget != null && freeRoamPlaceholder != null && freeRoamMovingPoint != null && freeRoam)
        {
            Quaternion movingPointRotation = Quaternion.Euler(new Vector3(0f, actualRotation.y, 0f));
            Quaternion targetRotation = Quaternion.Euler(new Vector3(actualRotation.x, 0f, 0f));

            freeRoamMovingPoint.rotation = movingPointRotation;
            freeRoamTarget.localRotation = targetRotation;
            freeRoamMovingPoint.Translate(movement);
            freeRoamMovingPoint.position = new Vector3(freeRoamTarget.position.x, 0f, freeRoamTarget.position.z);
            movement = Vector3.zero;

            Vector3 currentPos = freeRoamPlaceholder.localPosition;
            freeRoamPlaceholder.localPosition = new Vector3(currentPos.x, currentPos.y, -currentDistance);

            Camera.main.transform.position = freeRoamPlaceholder.position;
            Camera.main.transform.LookAt(freeRoamTarget);
        }
    }

    private void SetTargetRotation()
    {
        if (Input.GetMouseButtonDown(2))
        {
            initialMousePosition = Input.mousePosition;
            return;
        }

        if (Input.GetMouseButton(2))
        {
            Vector2 offset = initialMousePosition - Input.mousePosition;
            actualRotation = initialRotation + new Vector3(offset.y, -offset.x, 0f) * mouseLookSpeed;
        }

        if (Input.GetMouseButtonUp(2))
        {
            initialMousePosition = Input.mousePosition;
            initialRotation = actualRotation;
            return;
        }
    }

    private void SetCameraDistance()
    {
        float scrolledDistance = Input.GetAxis("Mouse ScrollWheel") * -1;

        float _currentDistance;
        float _newDistance;

        if (placeHolder != null && !freeRoam)
        {
            _currentDistance = placeHolder.localPosition.z * -1;
            _newDistance = currentDistance + scrolledDistance * scrollSpeed;
        }
        else if (freeRoamPlaceholder != null && freeRoam)
        {
            _currentDistance = freeRoamPlaceholder.localPosition.z * -1;
            _newDistance = currentDistance + scrolledDistance * scrollSpeed;
        }
        else
        {
            return;
        }

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

    private void FreeRoam()
    {
        if(freeRoamTarget == null || freeRoamPlaceholder == null)
        {
            return;
        }

        if(!freeRoam)
        {
            if(CrossPlatformInputManager.GetAxis("Horizontal") != 0 || CrossPlatformInputManager.GetAxis("Vertical") != 0)
            {
                DetachFromTarget();
            }
        } else
        {
            float speed = Input.GetKey(KeyCode.LeftShift) ? freeRoamSpeed * 2 : freeRoamSpeed;
            movement += new Vector3(CrossPlatformInputManager.GetAxis("Horizontal") * speed * Time.deltaTime, 0f, 0f);
            movement += new Vector3(0f, 0f, CrossPlatformInputManager.GetAxis("Vertical") * speed * Time.deltaTime);
        }
    }

    private void DetachFromTarget()
    {
        if(freeRoamTarget == null || freeRoamPlaceholder == null || freeRoamMovingPoint == null || target == null || placeHolder == null)
        {
            return;
        }

        freeRoamMovingPoint.position = target.position;
        freeRoamTarget.position = target.position;
        freeRoamTarget.rotation = target.rotation;
        freeRoamPlaceholder.position = placeHolder.position;
        freeRoamPlaceholder.rotation = placeHolder.rotation;

        freeRoam = true;
    }

    public void AttachToTarget()
    {
        if(target != null && placeHolder != null)
        {
            freeRoam = false;
        }
    }

    public void AttachToTarget(Transform transform)
    {
        target = transform;
        placeHolder = transform.GetChild(0);
        AttachToTarget();
    }
}
