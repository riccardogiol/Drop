using System;
using Cinemachine;
using Unity.Mathematics;
using UnityEngine;

public class MapMoveCamera : MonoBehaviour
{
    public CinemachineVirtualCamera cinemachine;
    public Camera cam;
    public GameObject targetPrefab;
    public float minX, minY;
    public float maxX, maxY;
    
    GameObject originalTarget;
    GameObject movingTarget;

    public static bool inMoveCameraMode;
    Vector3 camStartingPosition;
    Vector3 targetStartingPosition;

    void Start()
    {
        originalTarget = cinemachine.Follow.gameObject;
        movingTarget = Instantiate(targetPrefab, originalTarget.transform.position, quaternion.identity);
        BoundMovingTargetPosition(originalTarget.transform.position);
        cinemachine.LookAt = movingTarget.transform;
        cinemachine.Follow = movingTarget.transform;
        inMoveCameraMode = false;
        MoveCameraToPosition(originalTarget.transform.position);
    }

    void Update()
    {
        if (Application.isMobilePlatform)
        {
            if (Input.touchCount > 0)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    camStartingPosition = cam.ScreenToWorldPoint(Input.GetTouch(0).position);
                    targetStartingPosition = movingTarget.transform.position;
                }
                if (Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    Vector3 difference = camStartingPosition - cam.ScreenToWorldPoint(Input.GetTouch(0).position);
                    if (inMoveCameraMode)
                        BoundMovingTargetPosition(targetStartingPosition + difference);
                    else 
                        if (difference.magnitude > 0.5)
                            inMoveCameraMode = true;
                }
            }
        } else {
            if (Input.GetMouseButtonDown(0))
            {
                camStartingPosition = cam.ScreenToWorldPoint(Input.mousePosition);
                targetStartingPosition = movingTarget.transform.position;
            }
            if (Input.GetMouseButton(0))
            {
                Vector3 difference = camStartingPosition - cam.ScreenToWorldPoint(Input.mousePosition);
                if (inMoveCameraMode)
                    BoundMovingTargetPosition(targetStartingPosition + difference);
                else 
                    if (difference.magnitude > 0.5)
                        inMoveCameraMode = true;
            }
        }
    }

    void FixedUpdate()
    {
        if (!inMoveCameraMode)
            BoundMovingTargetPosition(originalTarget.transform.position);
    }

    void BoundMovingTargetPosition(Vector3 newPosition)
    {
        movingTarget.transform.position = new Vector3(
            Math.Max(Math.Min(newPosition.x, maxX), minX), 
            Math.Max(Math.Min(newPosition.y, maxY), minY));
    }

    public void Exit()
    {
        BoundMovingTargetPosition(originalTarget.transform.position);
        inMoveCameraMode =false;
    }

    public void MoveCameraToPosition(Vector3 position)
    {
        GameObject target = cinemachine.Follow.gameObject;
        cinemachine.Follow = null;
        cinemachine.LookAt = null;
        cinemachine.transform.position = position;
        cinemachine.Follow = target.transform;
        cinemachine.LookAt = target.transform;
    }
}
