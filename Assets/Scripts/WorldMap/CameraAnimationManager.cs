using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class CameraAnimationManager : MonoBehaviour
{
    public CinemachineVirtualCamera cinemachineVirtualCamera;
    Camera cam;
    public float mobileZoom = 3;
    public float desktopZoom = 4;
    public float portraitZoom = 7;
    float eagleZoom;
    public float exitZoom;
    public float timer = 1.4f;
    float countdown = 0;

    float inGameZoom;
    bool stableZoom = true;
    float startZoom;
    float finishZoom;

    float currentZoom = 0.0f;
    float maxZoom = 0;
    float minZoom = 0;

    public Button zoomOutButton, zoomInButton;

    float currentRatio;

    public Animator animator;

    PlayerMapTargeting playerMapTargeting;

    void Awake()
    {
        playerMapTargeting = GetComponent<PlayerMapTargeting>();
        cam = FindFirstObjectByType<Camera>();
        currentRatio = cam.aspect;
        if (currentRatio < 1)
        {
            inGameZoom = portraitZoom;
        }
        else
        {
            if (Application.isMobilePlatform)
                inGameZoom = mobileZoom;
            else
                inGameZoom = desktopZoom;
        }
        eagleZoom = inGameZoom + 1;
        if (exitZoom == 0.0f)
            exitZoom = inGameZoom - 1;
        else
            exitZoom = inGameZoom + exitZoom;
        maxZoom = inGameZoom + 4;
        minZoom = inGameZoom;
        if (zoomInButton != null)
        {
            zoomInButton.interactable = false;
            zoomOutButton.interactable = true;
        }

        startZoom = exitZoom;
        finishZoom = inGameZoom;
        cinemachineVirtualCamera.m_Lens.OrthographicSize = startZoom;
    }

    void Start()
    {
        countdown = 0;
        stableZoom = false;
    }

    void Update()
    {
        if (currentRatio != cam.aspect)
        {
            currentRatio = cam.aspect;
            if (currentRatio < 1)
            {
                inGameZoom = portraitZoom;
            }
            else
            {
                if (Application.isMobilePlatform)
                    inGameZoom = mobileZoom;
                else
                    inGameZoom = desktopZoom;
            }
            eagleZoom = inGameZoom + 1;
            exitZoom = inGameZoom - 1;
            maxZoom = inGameZoom + 4;
            minZoom = inGameZoom;
            finishZoom = inGameZoom;
            if (zoomInButton != null)
            {
                zoomInButton.interactable = false;
                zoomOutButton.interactable = true;
            }
            cinemachineVirtualCamera.m_Lens.OrthographicSize = inGameZoom;
        }
        if (stableZoom)
            return;
        if (countdown < timer)
        {
            SetCameraZoomLerp();
            //if (!MenusManager.isPaused) CONDITION FOR EAGLE EY INTERFERENCE?
            countdown += Time.unscaledDeltaTime;
            if (countdown >= timer)
            {
                cinemachineVirtualCamera.m_Lens.OrthographicSize = finishZoom;
                if (zoomInButton != null)
                    inGameZoom = finishZoom;
                stableZoom = true;
                timer = 1.4f;
            }
        }
    }

    void SetCameraZoomLerp()
    {
        cinemachineVirtualCamera.m_Lens.OrthographicSize = startZoom + (finishZoom - startZoom) * (countdown / timer);
    }

    public void StartEndingAnimation()
    {
        playerMapTargeting.FromTargetToPlayer();
        startZoom = inGameZoom;
        finishZoom = exitZoom;
        countdown = 0;
        stableZoom = false;
    }

    public void RevertEndingAnimation()
    {
        startZoom = exitZoom;
        finishZoom = inGameZoom;
        countdown = 0;
        stableZoom = false;
    }

    public void EnterEagleZoomAnimation(float extraZoomOutValue = 0.0f)
    {
        StartZoomAnimation(eagleZoom + extraZoomOutValue);
        timer = 0.8f;
    }

    public void ZoomValueAnimation(float value)
    {
        if (!stableZoom)
            return;
        float targetZoom = Mathf.Clamp(inGameZoom + value, minZoom, maxZoom);
        if (zoomInButton != null)
        {
            if (targetZoom >= maxZoom)
                zoomOutButton.interactable = false;
            else if (targetZoom <= minZoom)
                zoomInButton.interactable = false;
            else
            {
                zoomInButton.interactable = true;
                zoomOutButton.interactable = true;
            }
        }
        StartZoomAnimation(targetZoom);
    }

    public void ExitEagleZoomAnimation()
    {
        StartZoomAnimation(inGameZoom);
        timer = 0.8f;
    }

    void StartZoomAnimation(float targetZoom)
    {
        startZoom = cinemachineVirtualCamera.m_Lens.OrthographicSize;
        finishZoom = targetZoom;
        countdown = 0;
        stableZoom = false;
    }

    public void StartTilting()
    {
        animator.SetTrigger("StartTilting");
    }
}
