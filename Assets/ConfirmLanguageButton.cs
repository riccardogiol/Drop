using UnityEngine;
using UnityEngine.SceneManagement;

public class ConfirmLanguageButton : MonoBehaviour
{
    public string selectedLangCode = "";

    public void Confirm()
    {
        PlayerPrefs.SetString("LanguagePreference", selectedLangCode); // aggiorna anche la flag di selezione
        Destroy(SingletonLocalizationManager.instance);
        SceneManager.LoadScene("MainMenu");
    }
}
