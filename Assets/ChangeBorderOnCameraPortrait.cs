using UnityEngine;

public class ChangeBorderOnCameraPortrait : MonoBehaviour
{
    Camera cam;
    public float Left, Right, Top, Bottom;
    float currentRatio = -1;

    Vector2 offsetMinLandscape, offsetMaxLandscape, offsetMinPortrait, offsetMaxPortrait;

    void Awake()
    {
        cam = FindFirstObjectByType<Camera>();
        if (cam!= null)
        {
            offsetMinLandscape = GetComponent<RectTransform>().offsetMin;
            offsetMaxLandscape = GetComponent<RectTransform>().offsetMax;
            offsetMinPortrait = new Vector2(Left, Bottom);
            offsetMaxPortrait = new Vector2(-Right, -Top);
        }
    }

    void Update()
    {
        if (cam.aspect != currentRatio)
        {
            if (cam.aspect < 1)
            {
                GetComponent<RectTransform>().offsetMin = offsetMinPortrait; 
                GetComponent<RectTransform>().offsetMax = offsetMaxPortrait; 
                currentRatio = cam.aspect;
            } else {
                GetComponent<RectTransform>().offsetMin = offsetMinLandscape; 
                GetComponent<RectTransform>().offsetMax = offsetMaxLandscape; 
                currentRatio = cam.aspect;
            }
        }
    }
}
