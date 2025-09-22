using System.Collections;
using Cinemachine;
using UnityEngine;

public class EagleEyeMode : MonoBehaviour
{
    public Transform targetSpot;
    public float extraZoomOut = 0.0f;
    public GameObject targetPrefab;
    GameObject originalTarget;
    GameObject targetRef;

    public static bool inEagleMode;

    PlaygroundManager playgroundManager;
    public CinemachineVirtualCamera cinemachine;
    public CameraAnimationManager cameraAnimationManager;

    float rechargeTimer = 10.0f, rechargeCountdown = 0;
    ButtonFiller buttonFiller;

    float timer = 4.0f, countdown = 0;
    float maxSlowDownFactor = 0.3f;

    void Awake()
    {
        playgroundManager = FindFirstObjectByType<PlaygroundManager>();

        originalTarget = cinemachine.Follow.gameObject;

        if (targetSpot == null)
        {
            GameObject goRef = Instantiate(targetPrefab, new Vector3(playgroundManager.maxX / 2.0f, playgroundManager.maxY / 2.0f, 0f), Quaternion.identity);
            targetSpot = goRef.transform;
        }

    }

    void Start()
    {
        playgroundManager = FindFirstObjectByType<PlaygroundManager>();
        ButtonFiller[] buttonFillers = FindObjectsOfType<ButtonFiller>();
        foreach (var bf in buttonFillers)
        {
            if (bf.gameObject.name == "EagleEyeButton")
                buttonFiller = bf;
        }
        if (buttonFiller == null)
            return;

        if (PlayerPrefs.GetInt("EasyMode", 0) == 1)
            rechargeTimer -= 4.0f;

        buttonFiller.SetMaxValue(rechargeTimer);
        buttonFiller.SetValue(0);
    }

    void Update()
    {
        if (inEagleMode)
        {
            countdown -= Time.deltaTime;
            if (countdown <= timer / 2f)
                Time.timeScale = maxSlowDownFactor + (1 - (countdown * 2) / timer) * (1 - maxSlowDownFactor);
            if (countdown <= 0)
                Exit();
        }

        if (rechargeCountdown > 0)
        {
            rechargeCountdown -= Time.deltaTime;
            buttonFiller.SetValue(rechargeCountdown);
        }
    }

    public void Enter()
    {
        if (buttonFiller == null)
            return;
        if (rechargeCountdown > 0)
            return;
        inEagleMode = true;
        playgroundManager.ShowEnergy();
        Time.timeScale = maxSlowDownFactor;
        countdown = timer;
        rechargeCountdown = rechargeTimer;

        Vector3 halfWay = new Vector3((originalTarget.transform.position.x + targetSpot.position.x) / 2.0f, (originalTarget.transform.position.y + targetSpot.position.y) / 2.0f, 0);
        targetRef = Instantiate(targetPrefab, originalTarget.transform.position, Quaternion.identity);
        targetRef.GetComponent<LinearMovement>().MoveTo(halfWay, 0.5f);
        targetRef.GetComponent<LinearMovement>().enabled = true;
        cinemachine.LookAt = targetRef.transform;
        cinemachine.Follow = targetRef.transform;
        cameraAnimationManager.EnterEagleZoomAnimation(extraZoomOut);
    }

    public void Exit()
    {
        if (!inEagleMode)
            return;
        inEagleMode = false;
        playgroundManager.HideEnergy();
        Time.timeScale = 1f;
        countdown = 0;

        StartCoroutine(DelayedExit());
        cameraAnimationManager.ExitEagleZoomAnimation();
    }

    IEnumerator DelayedExit()
    {
        targetRef.GetComponent<LinearMovement>().MoveTo(originalTarget.transform.position, 1.5f);
        targetRef.GetComponent<LinearMovement>().enabled = true;
        yield return new WaitForSeconds(1.4f);
        cinemachine.LookAt = originalTarget.transform;
        cinemachine.Follow = originalTarget.transform;
    }
}

// code to detect swipe and change target to moving target for cinemachine if we want to introduce it
/*
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
                    float newX = Math.Max(Math.Min(newPosition.x, maxX - 3), 3);
                    float newY = Math.Max(Math.Min(newPosition.y, maxY - 3), 3);
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
                float newX = Math.Max(Math.Min(newPosition.x, maxX - 3), 3);
                float newY = Math.Max(Math.Min(newPosition.y, maxY - 3), 3);
                target.transform.position = new Vector3(newX, newY);
            }
        }

*/
