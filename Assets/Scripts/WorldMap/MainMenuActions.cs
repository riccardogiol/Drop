using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuActions : MonoBehaviour
{
    public Button continueButton;

    void Awake()
    {
        SaveData saveData = SaveManager.Load();
        PPInitializer(saveData);
    }

    void Start()
    {
        if ((PlayerPrefs.GetInt("Lvl1", 0) == 1) || PlayerPrefs.GetInt("LastStageCompleted", 0) > 0)
            continueButton.interactable = true;
        else
            continueButton.interactable = false;
        FindObjectOfType<AudioManager>().Play("OpeningMusic");
    }

    public void NewGame(bool casual = false)
    {
        SaveManager.Restart();
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1);
        float soundVolume = PlayerPrefs.GetFloat("SoundVolume", 1);
        string language = PlayerPrefs.GetString("LanguagePreference", "eng");
        int fv = PlayerPrefs.GetInt("FullVersion", 0);
        int dv = PlayerPrefs.GetInt("DemoVersion", 0);
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("SoundVolume", soundVolume);
        PlayerPrefs.SetString("LanguagePreference", language);
        PlayerPrefs.SetInt("DemoVersion", dv);
        PlayerPrefs.SetInt("FullVersion", fv);
        PlayerPrefs.SetInt("Lvl0", 1);
        PlayerPrefs.SetInt("ShowButtonHint", 1);
        if (casual)
            PlayerPrefs.SetInt("EasyMode", 1);
        SceneManager.LoadScene("OpeningScene");
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene("OpeningScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void NewGameDisclaimer(GameObject disclaimerPanel)
    {
        if (PlayerPrefs.GetInt("Lvl1", 0) == 1)
            disclaimerPanel.SetActive(true);
        else
            NewGame(false);
    }

    void PPInitializer(SaveData saveData)
    {
        int lastStageCode = 0;
        for (int i = 0; i < saveData.StageCompleteStatus.Length; i++)
        {
            if (saveData.StageCompleteStatus[i] > 0)
                lastStageCode = i;
        }

        int quotient = lastStageCode / 4;
        int remainder = lastStageCode % 4;

        PlayerPrefs.SetInt("LastLevelCompleted", quotient);
        PlayerPrefs.SetInt("LastStageCompleted", remainder);

        for (int i = 1; i <= quotient; i++)
        {
            PlayerPrefs.SetInt("Lvl" + i, 1);
            PlayerPrefs.SetInt("Lvl" + i + "Prize", 1);
        }
        for (int i = quotient + 1; i <= saveData.StageCompleteStatus.Length; i++)
        {
            PlayerPrefs.SetInt("Lvl" + i, 0);
            PlayerPrefs.SetInt("Lvl" + i + "Prize", 0);
        }

    }
}
