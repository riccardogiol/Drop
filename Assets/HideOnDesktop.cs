using UnityEngine;

public class HideOnDesktop : MonoBehaviour
{
    void Awake()
    {
        if (!Application.isMobilePlatform)
            gameObject.SetActive(false);
    }
}
