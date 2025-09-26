using UnityEngine;
using UnityEngine.UI;

public class TextLocalizer : MonoBehaviour
{
    public string key = "point.format";
    FitBoxText fitBoxText;
    Text textComponent;

    public bool upperCase = false;

    void Awake()
    {
        textComponent = GetComponent<Text>();
        fitBoxText = GetComponent<FitBoxText>();

        Localize();
    }

    public void Localize()
    {
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
