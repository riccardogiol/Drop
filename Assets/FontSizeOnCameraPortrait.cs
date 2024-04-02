using UnityEngine;
using UnityEngine.UI;

public class FontSizeOnCameraPortrait : MonoBehaviour
{
    Camera cam;
    public int fontSizeP;
    int fontSizeL;
    float currentRatio;

    void Awake()
    {
        cam = FindFirstObjectByType<Camera>();
        if (cam!= null)
        {
            currentRatio = cam.aspect;
            fontSizeL = GetComponent<Text>().fontSize;
            if (currentRatio < 1)
            {
                GetComponent<Text>().fontSize = fontSizeP; 
            }
        }
    }

    void Update()
    {
        if (cam.aspect != currentRatio)
        {
            if (cam.aspect < 1)
            {
                GetComponent<Text>().fontSize = fontSizeP; 
                currentRatio = cam.aspect;
            } else {
                GetComponent<Text>().fontSize = fontSizeL; 
                currentRatio = cam.aspect;
            }
        }
    }
}
