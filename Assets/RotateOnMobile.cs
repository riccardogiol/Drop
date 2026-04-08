using UnityEngine;

public class RotateOnMobile : MonoBehaviour
{
    public float RotationOnZ;

    void Awake()
    {
        if (!Application.isMobilePlatform)
            return;
        GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, RotationOnZ);
    }
}
