using UnityEngine;
using UnityEngine.UI;

public class ToggleLayoutButton : MonoBehaviour
{
    public Text buttonText;
    void Start()
    {
        UpdateLayoutToggleText();
    }

    public void ToggleLayout()
    {
        if (PlayerPrefs.GetInt("AzertyLayout", 0) == 0)
            PlayerPrefs.SetInt("AzertyLayout", 1);
        else
            PlayerPrefs.SetInt("AzertyLayout", 0);
        UpdateLayoutToggleText();
    }

    void UpdateLayoutToggleText()
    {
        if (PlayerPrefs.GetInt("AzertyLayout", 0) == 1)
            buttonText.text = "AZERTY\nLAYOUT";
        else
            buttonText.text = "QWERTY\nLAYOUT";
    }
}
