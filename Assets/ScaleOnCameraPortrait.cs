using UnityEngine;

public class ScaleOnCameraPortrait : MonoBehaviour
{
    Camera cam;
    public float scaleXP, scaleYP;
    float scaleXL, scaleYL;
    float currentRatio;

    void Awake()
    {
        cam = FindFirstObjectByType<Camera>();
        if (cam!= null)
        {
            currentRatio = cam.aspect;
            scaleXL = GetComponent<RectTransform>().localScale.x;
            scaleYL = GetComponent<RectTransform>().localScale.y;
            if (currentRatio < 1)
            {
                GetComponent<RectTransform>().localScale = new Vector3(scaleXP, scaleYP, 1); 
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
                GetComponent<RectTransform>().localScale = new Vector3(scaleXL, scaleYL, 1); 
                currentRatio = cam.aspect;
            }
        }
    }
}
