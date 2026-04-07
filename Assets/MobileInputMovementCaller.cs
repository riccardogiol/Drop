using UnityEngine;

public class MobileInputMovementCaller : MonoBehaviour
{
    PlayerMovementKeys pvk;
    public Vector3 direction;

    bool isPressed;

    void Start()
    {
        pvk = FindFirstObjectByType<PlayerMovementKeys>();
        if (pvk == null)
        {
            gameObject.SetActive(false);
            return;
        }
    }

    void Update()
    {
        if (isPressed)
            pvk.ReadInputMobile(direction);
    }

    public void StartPressing()
    {
        isPressed = true;
    }

    public void StopPressing()
    {
        isPressed = false;
    }
}
