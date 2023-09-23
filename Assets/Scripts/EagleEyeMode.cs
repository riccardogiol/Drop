using System;
using Cinemachine;
using Unity.Mathematics;
using UnityEngine;

public class EagleEyeMode : MonoBehaviour
{
    public CinemachineVirtualCamera cinemachine;
    public Animator camAnimator;
    public Camera cam;
    public GameObject targetPrefab;
    
    GameObject target;
    Transform originalTarget;
    public static bool inEagleMode;
    Vector3 camStartingPosition;
    Vector3 targetStartingPosition;
    PlaygroundManager playgroundManager;
    int maxX, maxY;

    void Start()
    {
        originalTarget = cinemachine.Follow;
        playgroundManager = FindFirstObjectByType<PlaygroundManager>();
        maxX = playgroundManager.maxX;
        maxY = playgroundManager.maxY;
    }

    void Update()
    {
        if(!inEagleMode)
            return;
        if (Application.isMobilePlatform)
        {
            if (Input.touchCount > 0)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    camStartingPosition = cam.ScreenToWorldPoint(Input.GetTouch(0).position);
                    targetStartingPosition = target.transform.position;
                }
                if (Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    Vector3 difference = camStartingPosition - cam.ScreenToWorldPoint(Input.GetTouch(0).position);
                    Vector3 newPosition = targetStartingPosition + difference;
                    float newX = Math.Max(Math.Min(newPosition.x, maxX), 0);
                    float newY = Math.Max(Math.Min(newPosition.y, maxY), 0);
                    target.transform.position = new Vector3(newX, newY);
                }
            }
        } else {
            if (Input.GetMouseButtonDown(0))
            {
                camStartingPosition = cam.ScreenToWorldPoint(Input.mousePosition);
                targetStartingPosition = target.transform.position;
            }

            if (Input.GetMouseButton(0))
            {
                Vector3 difference = camStartingPosition - cam.ScreenToWorldPoint(Input.mousePosition);
                Vector3 newPosition = targetStartingPosition + difference;
                float newX = Math.Max(Math.Min(newPosition.x, maxX), 0);
                float newY = Math.Max(Math.Min(newPosition.y, maxY), 0);
                target.transform.position = new Vector3(newX, newY);
            }
        }
    }

    public void Enter()
    {
        target = Instantiate(targetPrefab, originalTarget.position, quaternion.identity);
        cinemachine.LookAt = target.transform;
        cinemachine.Follow = target.transform;
        inEagleMode = true;
        playgroundManager.ShowEnergy();
        camAnimator.SetTrigger("EnterEagleEye");
    }

    
    public void Exit()
    {
        Destroy(target);
        cinemachine.LookAt = originalTarget;
        cinemachine.Follow = originalTarget;
        inEagleMode =false;
        playgroundManager.HideEnergy();
        camAnimator.SetTrigger("ExitEagleEye");
    }
}
