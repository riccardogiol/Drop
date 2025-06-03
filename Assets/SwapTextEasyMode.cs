using UnityEngine;
using UnityEngine.UI;

public class SwapTextEasyMode : MonoBehaviour
{
    Text text;
    public string EasyModeText = "";

    void Awake()
    {
        text = GetComponent<Text>();
        if (PlayerPrefs.GetInt("EasyMode", 0) == 1)
            text.text = EasyModeText; 
    }
}
