using UnityEngine;
using UnityEngine.UI;

public class TextLocalizer : MonoBehaviour
{
    public string key = "point.format";
    void Awake()
    {
        Text textComponent = GetComponent<Text>();
        if (textComponent == null)
            return;
        string localizedText = SingletonLocalizationManager.instance.GetComponent<LocalizationManager>().Get(key);
        if (localizedText == null)
            return;
        textComponent.text = localizedText; 
    }
}
