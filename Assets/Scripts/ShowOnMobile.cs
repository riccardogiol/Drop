using UnityEngine;
using UnityEngine.InputSystem;

public class ShowOnMobile : MonoBehaviour
{
    public bool show = true;
    public bool hideShowWithGamepad = false;
    void Start()
    {
        if (Application.isMobilePlatform)
            gameObject.SetActive(show);
        if (hideShowWithGamepad)
           if (Gamepad.current == null)
              gameObject.SetActive(false);
    }
}
