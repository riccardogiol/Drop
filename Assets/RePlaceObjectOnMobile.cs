using UnityEngine;

public class RePlaceObjectOnMobile : MonoBehaviour
{
    public GameObject canvasObject;

    void Awake()
    {
        if (!Application.isMobilePlatform)
            return;
        canvasObject.GetComponent<RectTransform>().parent = transform;
        canvasObject.GetComponent<RectTransform>().pivot = new Vector3(0.5f, 0.5f, 0);
        canvasObject.GetComponent<RectTransform>().anchorMin = new Vector3(0.5f, 0.5f, 0);
        canvasObject.GetComponent<RectTransform>().anchorMax = new Vector3(0.5f, 0.5f, 0);
        canvasObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
    }
}
