using System;
using Cinemachine;
using Unity.Mathematics;
using UnityEngine;

public class MapMoveCamera : MonoBehaviour
{
    public CinemachineVirtualCamera cinemachine;
    public Camera cam;
    public GameObject targetPrefab;
    
    Transform target;
    GameObject originalTarget;
    GameObject movingTarget;
    public static bool inMoveCameraMode;
    Vector3 camStartingPosition;
    Vector3 targetStartingPosition;
    public float maxX, maxY;

    void Start()
    {
        originalTarget = cinemachine.Follow.gameObject;
        movingTarget = Instantiate(targetPrefab, originalTarget.transform.position, quaternion.identity);
        target = originalTarget.transform;
        inMoveCameraMode = false;
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
                    targetStartingPosition = target.position;
                }
                if (Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    Vector3 difference = camStartingPosition - cam.ScreenToWorldPoint(Input.GetTouch(0).position);
                    if (inMoveCameraMode)
                    {
                        Vector3 newPosition = targetStartingPosition + difference;
                        float newX = Math.Max(Math.Min(newPosition.x, maxX), 0);
                        float newY = Math.Max(Math.Min(newPosition.y, maxY), 0);
                        target.position = new Vector3(newX, newY);
                    } else 
                    {
                        if (difference.magnitude > 0.5)
                            Enter();
                    }
                }
            }
        } else {
            if (Input.GetMouseButtonDown(0))
            {
                camStartingPosition = cam.ScreenToWorldPoint(Input.mousePosition);
                targetStartingPosition = target.position;
            }

            if (Input.GetMouseButton(0))
            {
                Vector3 difference = camStartingPosition - cam.ScreenToWorldPoint(Input.mousePosition);
                if (inMoveCameraMode)
                {
                    Vector3 newPosition = targetStartingPosition + difference;
                    float newX = Math.Max(Math.Min(newPosition.x, maxX), 0f);
                    float newY = Math.Max(Math.Min(newPosition.y, maxY), 0f);
                    target.position = new Vector3(newX, newY);
                } else 
                {
                    if (difference.magnitude > 0.5)
                        Enter();
                }
            }
        }
    }

    void Enter()
    {
        movingTarget.transform.position = target.position;
        target = movingTarget.transform;
        cinemachine.LookAt = target;
        cinemachine.Follow = target;
        inMoveCameraMode = true;
    }

    
    public void Exit()
    {
        target = originalTarget.transform;
        cinemachine.LookAt = target;
        cinemachine.Follow = target;
        inMoveCameraMode =false;
    }
}
