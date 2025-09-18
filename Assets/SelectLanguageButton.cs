using UnityEngine;
using UnityEngine.UI;

public class SelectLanguageButton : MonoBehaviour
{
    public Button confirmButton;
    Button itselfButton;
    public string langCode = "eng";

    void Awake()
    {
        itselfButton = GetComponent<Button>();
        if (PlayerPrefs.GetString("LanguagePreference", "eng") == langCode)
            itselfButton.interactable = false;
    }

    public void Select()
    {
        confirmButton.interactable = true;
        confirmButton.GetComponent<ConfirmLanguageButton>().selectedLangCode = langCode;
    }
}
