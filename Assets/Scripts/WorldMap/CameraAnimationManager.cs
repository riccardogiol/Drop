using Cinemachine;
using UnityEngine;

public class CameraAnimationManager : MonoBehaviour
{
    public CinemachineVirtualCamera cinemachineVirtualCamera;
    public float mobileZoom = 3;
    public float desktopZoom = 4;
    float eagleZoom;
    float exitZoom = 2;
    public float timer = 2.0f;
    float countdown = 0;

    float inGameZoom;
    bool stableZoom = true;
    float startZoom;
    float finishZoom;

    PlayerMapTargeting playerMapTargeting;

    void Awake()
    {
        playerMapTargeting = GetComponent<PlayerMapTargeting>();
        if (Application.isMobilePlatform)
            inGameZoom = mobileZoom;
        else
            inGameZoom = desktopZoom;
        eagleZoom = inGameZoom + 1;
    }

    void Start()
    {
        startZoom = exitZoom;
        finishZoom = inGameZoom;
        countdown = 0;
        stableZoom = false;
    }

    void FixedUpdate()
    {
        if (stableZoom)
            return;
        if (countdown < timer)
        {
            SetCameraZoomLerp();
            if (Time.timeScale > 0.01)
                countdown += Time.deltaTime / Time.timeScale;
            if (countdown >= timer)
            {
                cinemachineVirtualCamera.m_Lens.OrthographicSize = finishZoom;
                stableZoom = true;
            }
        }
    }

    void SetCameraZoomLerp()
    {
        cinemachineVirtualCamera.m_Lens.OrthographicSize = startZoom + (finishZoom - startZoom) * (countdown/timer);
    }

    public void StartEndingAnimation()
    {
        playerMapTargeting.FromTargetToPlayer();
        startZoom = inGameZoom;
        finishZoom = exitZoom;
        countdown = 0;
        stableZoom = false;
    }

    public void EnterEagleZoomAnimation()
    {
        StartZoomAnimation(eagleZoom);
    }

    public void ExitEagleZoomAnimation()
    {
        StartZoomAnimation(inGameZoom);
    }

    void StartZoomAnimation(float targetZoom)
    {
        startZoom = cinemachineVirtualCamera.m_Lens.OrthographicSize;
        finishZoom = targetZoom;
        countdown = 0;
        stableZoom = false;
    }
}
