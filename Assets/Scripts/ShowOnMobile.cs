using UnityEngine;

public class ShowOnMobile : MonoBehaviour
{
    public bool show = true;
    void Start()
    {
        if (Application.isMobilePlatform)
            gameObject.SetActive(show);
    }
}
