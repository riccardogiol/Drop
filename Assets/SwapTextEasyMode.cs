using UnityEngine;
using UnityEngine.UI;

public class SwapTextEasyMode : MonoBehaviour
{
    Text text;
    public string EasyModeText = "";
    public string NormalModeText = "";

    void Awake()
    {
        text = GetComponent<Text>();
        if (PlayerPrefs.GetInt("EasyMode", 0) == 1)
            text.text = EasyModeText;
        else
            text.text = NormalModeText;
    }

    public void UpdateText()
    {
        if (PlayerPrefs.GetInt("EasyMode", 0) == 1)
            text.text = EasyModeText;
        else
            text.text = NormalModeText;
    }
}
