using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GamepadButtonHintReplacer : MonoBehaviour
{
    public string gamepadButtonCode = "X";
    public RectTransform box;
    public float newBoxWide = 0;
    void Start()
    {
        if (Gamepad.current != null)
        {
            GetComponent<Text>().text = gamepadButtonCode;
            if (newBoxWide != 0 && box != null)
                box.sizeDelta = new Vector2(newBoxWide, box.sizeDelta.y);
        }
    }
}
