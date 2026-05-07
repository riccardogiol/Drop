using UnityEngine;

public class ScaleOnCameraPortrait : MonoBehaviour
{
    Camera cam;
    public float scaleXP, scaleYP;
    float scaleXL, scaleYL;
    float currentRatio;

    public bool scaleOnLandscapeMobile = false;
    public float scaleXLM = 1, scaleYLM = 1;

    void Awake()
    {
        cam = Camera.main;
        if (cam == null)
            cam = FindFirstObjectByType<Camera>();
        if (cam!= null)
        {
            currentRatio = cam.aspect;
            scaleXL = GetComponent<RectTransform>().localScale.x;
            scaleYL = GetComponent<RectTransform>().localScale.y;
            if (currentRatio < 1)
            {
                GetComponent<RectTransform>().localScale = new Vector3(scaleXP, scaleYP, 1); 
            } else if (scaleOnLandscapeMobile && Application.isMobilePlatform)
            {
                GetComponent<RectTransform>().localScale = new Vector3(scaleXLM, scaleYLM, 1);
            }
        }
    }

    void Update()
    {
        if (cam.aspect != currentRatio)
        {
            if (cam.aspect < 1)
            {
                GetComponent<RectTransform>().localScale = new Vector3(scaleXP, scaleYP, 1); 
                currentRatio = cam.aspect;
            } else {
                if (scaleOnLandscapeMobile && Application.isMobilePlatform)
                    GetComponent<RectTransform>().localScale = new Vector3(scaleXLM, scaleYLM, 1);
                else
                    GetComponent<RectTransform>().localScale = new Vector3(scaleXL, scaleYL, 1); 
                currentRatio = cam.aspect;
            }
        }
    }
}
