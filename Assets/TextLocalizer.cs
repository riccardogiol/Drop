using UnityEngine;
using UnityEngine.UI;

public class TextLocalizer : MonoBehaviour
{
    public string key = "point.format";
    FitBoxText fitBoxText;

    public bool upperCase = false;

    void Awake()
    {
        Text textComponent = GetComponent<Text>();
        fitBoxText = GetComponent<FitBoxText>();

        if (textComponent == null)
            return;
        string localizedText = SingletonLocalizationManager.instance.GetComponent<LocalizationManager>().Get(key);
        if (localizedText == null)
            return;
        textComponent.text = localizedText;

        if (upperCase)
            textComponent.text = textComponent.text.ToUpper();

        if (fitBoxText != null)
                fitBoxText.Resize();
    }
}
