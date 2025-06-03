using UnityEngine;

public class ScaleOnCanvasResize : MonoBehaviour
{
    public RectTransform canvasRT;
    public int HPadding = 0;
    public int VPadding = 0;

    float originalWidth;
    float originalHeight;

    float currentRatio = 0;

    void Awake()
    {
        originalWidth = GetComponent<RectTransform>().rect.width;
        originalHeight = GetComponent<RectTransform>().rect.height;
        currentRatio = canvasRT.rect.width / canvasRT.rect.height;
        if (currentRatio > 1)
        {
            float scalingFactor = canvasRT.rect.height/(originalHeight + VPadding*2);
            GetComponent<RectTransform>().localScale = new Vector3(scalingFactor, scalingFactor, 1); 
        } else {
            float scalingFactor = canvasRT.rect.width/(originalWidth + HPadding*2);
            GetComponent<RectTransform>().localScale = new Vector3(scalingFactor, scalingFactor, 1); 
        }
    }

    void Update()
    {
        float newRatio = canvasRT.rect.width / canvasRT.rect.height;
        if (newRatio != currentRatio)
        {
            currentRatio = newRatio;
            if (currentRatio > 1)
            {
                float scalingFactor = canvasRT.rect.height/(originalHeight + VPadding*2);
                GetComponent<RectTransform>().localScale = new Vector3(scalingFactor, scalingFactor, 1); 
            } else {
                float scalingFactor = canvasRT.rect.width/(originalWidth + HPadding*2);
                GetComponent<RectTransform>().localScale = new Vector3(scalingFactor, scalingFactor, 1); 
            }
        }
        
    }
}
