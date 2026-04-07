using UnityEngine;

public class ChangeBorderOnCameraPortrait : MonoBehaviour
{
    Camera cam;
    RectTransform rectTransform;
    public float Left, Right, Top, Bottom;
    float currentRatio = -1;

    Vector2 offsetMinLandscape, offsetMaxLandscape, offsetMinPortrait, offsetMaxPortrait;

    void Awake()
    {
        cam = Camera.main;
        if (cam == null)
            cam = FindFirstObjectByType<Camera>();
        rectTransform = GetComponent<RectTransform>();
        if (cam!= null)
        {
            offsetMinLandscape = rectTransform.offsetMin;
            offsetMaxLandscape = rectTransform.offsetMax;
            offsetMinPortrait = new Vector2(Left, Bottom);
            offsetMaxPortrait = new Vector2(-Right, -Top);
            if (cam.aspect != currentRatio)
            {
                if (cam.aspect < 1)
                {
                    rectTransform.offsetMin = offsetMinPortrait; 
                    rectTransform.offsetMax = offsetMaxPortrait; 
                    currentRatio = cam.aspect;
                } else {
                    rectTransform.offsetMin = offsetMinLandscape; 
                    rectTransform.offsetMax = offsetMaxLandscape; 
                    currentRatio = cam.aspect;
                }
            }
        }
    }

    void Update()
    {
        if (cam.aspect != currentRatio)
        {
            if (cam.aspect < 1)
            {
                rectTransform.offsetMin = offsetMinPortrait; 
                rectTransform.offsetMax = offsetMaxPortrait; 
                currentRatio = cam.aspect;
            } else {
                rectTransform.offsetMin = offsetMinLandscape; 
                rectTransform.offsetMax = offsetMaxLandscape; 
                currentRatio = cam.aspect;
            }
        }
    }
}
