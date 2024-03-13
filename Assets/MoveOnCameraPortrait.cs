using UnityEngine;

public class MoveOnCameraPortrait : MonoBehaviour
{
    Camera cam;
    public float Yvalue, Xvalue;
    float currentRatio;

    Vector2 landscapePosition, portraitPosition;

    void Awake()
    {
        cam = FindFirstObjectByType<Camera>();
        if (cam!= null)
        {
            currentRatio = cam.aspect;
            landscapePosition = GetComponent<RectTransform>().anchoredPosition;
            portraitPosition = landscapePosition + new Vector2(Xvalue, Yvalue); 
            if (currentRatio < 1)
            {
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
                GetComponent<RectTransform>().anchoredPosition = portraitPosition; 
                currentRatio = cam.aspect;
            } else {
                GetComponent<RectTransform>().anchoredPosition = landscapePosition; 
                currentRatio = cam.aspect;
            }
        }
    }
}
