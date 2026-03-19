using UnityEngine;
using UnityEngine.UI;

public class SwapImageOnCameraPortrait : MonoBehaviour
{
    Camera cam;
    float currentRatio;

    Image image;
    public Sprite verticalSprite;
    Sprite horizontalSprite;

    void Awake()
    {
        cam = FindFirstObjectByType<Camera>();
        image = GetComponent<Image>();
        if (image == null)
        {
            enabled = false;
            return;
        }
        horizontalSprite = image.sprite;
        if (cam!= null)
        {
            currentRatio = cam.aspect;
            if (currentRatio < 1)
            {
                image.sprite = verticalSprite;
            }
        }
    }
    // Start is called before the first frame update
    void Update()
    {
        if (cam.aspect != currentRatio)
        {
            if (cam.aspect < 1)
            {
                image.sprite = verticalSprite; 
                currentRatio = cam.aspect;
            } else {
                image.sprite = horizontalSprite; 
                currentRatio = cam.aspect;
            }
        }
        
    }
}
