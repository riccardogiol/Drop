using UnityEngine;
using UnityEngine.UI;

public class SwapTextChallengeDisabled : MonoBehaviour
{
    
    Text text;
    public string EnabledKey = "";
    public string DisabledKey = "";

    void Awake()
    {
        text = GetComponent<Text>();

        UpdateText();
    }

    public void UpdateText()
    {
        if (PlayerPrefs.GetInt("ChallengeDisabled", 0) == 1)
        {
            string localizedText = SingletonLocalizationManager.instance.GetComponent<LocalizationManager>().Get(DisabledKey);
            if (localizedText == null)
                return;
            text.text = localizedText;
        }
        else
        {
            string localizedText = SingletonLocalizationManager.instance.GetComponent<LocalizationManager>().Get(EnabledKey);
            if (localizedText == null)
                return;
            text.text = localizedText + " (LVL1-5)";
        }
    }
}
