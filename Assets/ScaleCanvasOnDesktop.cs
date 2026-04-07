using UnityEngine;
using UnityEngine.UI;

public class ScaleCanvasOnDesktop : MonoBehaviour
{
    CanvasScaler canvasScaler;
    public int desktopXReferenceResolution = 2500;
    public int mobileXReferenceResolution = 1900;
    void Awake()
    {
        canvasScaler = GetComponent<CanvasScaler>();
        if (Application.isMobilePlatform)
            canvasScaler.referenceResolution = new Vector2(mobileXReferenceResolution, canvasScaler.referenceResolution.y);
        else
            canvasScaler.referenceResolution = new Vector2(desktopXReferenceResolution, canvasScaler.referenceResolution.y);
    }
}
