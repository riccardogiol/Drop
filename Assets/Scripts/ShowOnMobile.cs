using UnityEngine;

public class ShowOnMobile : MonoBehaviour
{
    void Start()
    {
        if (!Application.isMobilePlatform)
            gameObject.SetActive(false);
    }
}
