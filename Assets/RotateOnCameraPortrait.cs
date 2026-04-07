using UnityEngine;

public class RotateOnCameraPortrait : MonoBehaviour
{
    Camera cam;
    public float RotationOnZ;
    float currentRatio;

    Quaternion landscapeRotation, portraitRotation;

    void Awake()
    {
        cam = Camera.main;
        if (cam == null)
            cam = FindFirstObjectByType<Camera>();
        if (cam!= null)
        {
            currentRatio = cam.aspect;
            landscapeRotation = GetComponent<RectTransform>().rotation;
            portraitRotation = Quaternion.Euler(0, 0, RotationOnZ);
            if (currentRatio < 1)
            {
                GetComponent<RectTransform>().rotation = portraitRotation; 
            }
        }
    }

    void Update()
    {
        if (cam.aspect != currentRatio)
        {
            if (cam.aspect < 1)
            {
                GetComponent<RectTransform>().rotation = portraitRotation; 
                currentRatio = cam.aspect;
            } else {
                GetComponent<RectTransform>().rotation = landscapeRotation; 
                currentRatio = cam.aspect;
            }
        }
    }
}
