using UnityEngine;
using UnityEngine.UI;

public class SwapTextEasyMode : MonoBehaviour
{
    Text text;
    FitBoxText fitBoxText;
    public string EasyModeKey = "";
    public string NormalModeKey = "";

    void Awake()
    {
        text = GetComponent<Text>();
        fitBoxText = GetComponent<FitBoxText>();

        if (PlayerPrefs.GetInt("EasyMode", 0) == 1)
        {
            string localizedText = SingletonLocalizationManager.instance.GetComponent<LocalizationManager>().Get(EasyModeKey);
            if (localizedText == null)
                return;
            text.text = localizedText;
        }
        else
        {
            string localizedText = SingletonLocalizationManager.instance.GetComponent<LocalizationManager>().Get(NormalModeKey);
            if (localizedText == null)
                return;
            text.text = localizedText;
        }

        if (fitBoxText != null)
            fitBoxText.Resize();
    }

    public void UpdateText()
    {
        if (PlayerPrefs.GetInt("EasyMode", 0) == 1)
        {
            string localizedText = SingletonLocalizationManager.instance.GetComponent<LocalizationManager>().Get(EasyModeKey);
            if (localizedText == null)
                return;
            text.text = localizedText; 
        }
        else
        {
            string localizedText = SingletonLocalizationManager.instance.GetComponent<LocalizationManager>().Get(NormalModeKey);
            if (localizedText == null)
                return;
            text.text = localizedText; 
        }

        if (fitBoxText != null)
            fitBoxText.Resize();
    }
}
