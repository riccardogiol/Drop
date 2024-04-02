using UnityEngine;

public class ResizeOnCameraPortrait : MonoBehaviour
{
    Camera cam;
    public float widthP, heightP;
    float widthL, heightL;
    float currentRatio;

    void Awake()
    {
        cam = FindFirstObjectByType<Camera>();
        if (cam!= null)
        {
            currentRatio = cam.aspect;
            widthL = GetComponent<RectTransform>().rect.width;
            heightL = GetComponent<RectTransform>().rect.height;
            if (currentRatio < 1)
            {
                GetComponent<RectTransform>().sizeDelta = new Vector2(widthP, heightP); 
            }
        }
    }

    void Update()
    {
        if (cam.aspect != currentRatio)
        {
            if (cam.aspect < 1)
            {
                GetComponent<RectTransform>().sizeDelta = new Vector2(widthP, heightP); 
                currentRatio = cam.aspect;
            } else {
                GetComponent<RectTransform>().sizeDelta = new Vector2(widthL, heightL); 
                currentRatio = cam.aspect;
            }
        }
    }
}
