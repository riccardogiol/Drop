using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ButtonClickerKey : MonoBehaviour
{
    Button button;
    public bool onlyGamepad = false;

    [Header("Set Action Type to Button")]
    public InputAction inputAction;
    
    void Awake()
    {
        if (onlyGamepad && Gamepad.current == null)
            enabled = false;
        if (inputAction == null)
            enabled = false;
        button = GetComponent<Button>();
        if (button == null)
            enabled = false;
    }

    void Update()
    {
        if (button.interactable && inputAction.WasPressedThisFrame())
             button.onClick.Invoke();
    }

    private void OnEnable()
    {
        inputAction.Enable();
    }

private void OnDisable()
    {
        inputAction.Disable();
    }
}
