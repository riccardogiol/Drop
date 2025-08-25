using UnityEngine;
using UnityEngine.UI;

public class ScaleCanvasOnDesktop : MonoBehaviour
{
    CanvasScaler canvasScaler;
    public int desktopXReferenceResolution = 2500;
    void Awake()
    {
        if (Application.isMobilePlatform)
            return;
        canvasScaler = GetComponent<CanvasScaler>();
        canvasScaler.referenceResolution = new Vector2(desktopXReferenceResolution, canvasScaler.referenceResolution.y);
    }
}
