using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GamepadButtonHintReplacer : MonoBehaviour
{
    public string gamepadButtonCode = "X";
    public RectTransform box;
    public float newBoxWide = 0;

    public GameObject elementToHideGP;
    public GameObject elementToShowGP;
    void Start()
    {
        if (Gamepad.current != null)
        {
            if (elementToShowGP != null)
            {
                elementToHideGP.SetActive(false);
                elementToShowGP.SetActive(true);
                GetComponent<Text>().enabled = false;
                return;
            }
            GetComponent<Text>().text = gamepadButtonCode;
            if (newBoxWide != 0 && box != null)
                box.sizeDelta = new Vector2(newBoxWide, box.sizeDelta.y);
        }
    }
}
