using UnityEngine;

public class MoveOnCameraPortrait : MonoBehaviour
{
    Camera cam;
    public float Yvalue, Xvalue;
    float currentRatio;

    Vector2 landscapePosition, portraitPosition;

    public bool evaluateSafeAreaTop = false;
    float topDifference = -999;
    public float extraTopDiffWithSafeArea = 0;

    void Awake()
    {
        cam = Camera.main;
        if (cam == null)
            cam = FindFirstObjectByType<Camera>();
        if (cam!= null)
        {
            currentRatio = cam.aspect;
            landscapePosition = GetComponent<RectTransform>().anchoredPosition;
            portraitPosition = landscapePosition + new Vector2(Xvalue, Yvalue); 
            if (currentRatio < 1)
            {
                if (evaluateSafeAreaTop && topDifference == -999 && Application.isMobilePlatform)
                {
                    topDifference = Screen.height - (Screen.safeArea.y + Screen.safeArea.height);
                    if (topDifference > 0)
                        portraitPosition = portraitPosition - new Vector2(0, topDifference - extraTopDiffWithSafeArea);
                }
                GetComponent<RectTransform>().anchoredPosition = portraitPosition; 
            }
        }
    }

    void Update()
    {
        if (cam.aspect != currentRatio)
        {
            if (cam.aspect < 1)
            {
                if (evaluateSafeAreaTop && topDifference == -999 && Application.isMobilePlatform)
                {
                    topDifference = Screen.height - (Screen.safeArea.y + Screen.safeArea.height);
                    if (topDifference > 0)
                        portraitPosition = portraitPosition - new Vector2(0, topDifference  - extraTopDiffWithSafeArea);
                }
                GetComponent<RectTransform>().anchoredPosition = portraitPosition; 
                currentRatio = cam.aspect;
            } else {
                GetComponent<RectTransform>().anchoredPosition = landscapePosition; 
                currentRatio = cam.aspect;
            }
        }
    }
}
