using UnityEngine;

public class MoveOnMobile : MonoBehaviour
{
    public float Yvalue, Xvalue;

    Vector2 mobilePosition;

    void Start()
    {
        if (Application.isMobilePlatform)
        {
            mobilePosition = GetComponent<RectTransform>().anchoredPosition + new Vector2(Xvalue, Yvalue);
            GetComponent<RectTransform>().anchoredPosition = mobilePosition; 
        }
    }
}
