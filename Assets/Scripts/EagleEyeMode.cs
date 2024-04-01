using UnityEngine;

public class EagleEyeMode : MonoBehaviour
{
    public GameObject targetPrefab;
    
    public static bool inEagleMode;
    PlaygroundManager playgroundManager;

    float rechargeTimer = 10.0f, rechargeCountdown = 0;
    ButtonFiller buttonFiller;

    float timer = 2.0f, countdown = 0;
    float maxSlowDownFactor = 0.3f;

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
        buttonFiller.SetMaxValue(rechargeTimer);
        buttonFiller.SetValue(0);
    }

    void Update()
    {
        if(inEagleMode)
        {
            countdown -= Time.deltaTime;
            if (countdown <= timer/2f)
                Time.timeScale = maxSlowDownFactor + (1-(countdown*2)/timer)*(1-maxSlowDownFactor);
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
        if (rechargeCountdown > 0)
            return;
        inEagleMode = true;
        playgroundManager.ShowEnergy();
        Time.timeScale = maxSlowDownFactor;
        countdown = timer;
        rechargeCountdown = rechargeTimer;
    }
    
    public void Exit()
    {
        inEagleMode =false;
        playgroundManager.HideEnergy();
        Time.timeScale = 1f;
        countdown = 0;
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
