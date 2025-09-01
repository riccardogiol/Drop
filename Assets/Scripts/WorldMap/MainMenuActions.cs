using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuActions : MonoBehaviour
{
    public Button continueButton;

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
        // add disclaimer in the case the game is already started
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1);
        float soundVolume = PlayerPrefs.GetFloat("SoundVolume", 1);
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("SoundVolume", soundVolume);
        PlayerPrefs.SetInt("Lvl0", 1);
        PlayerPrefs.SetInt("ShowButtonHint", 1);
        if (casual)
            PlayerPrefs.SetInt("EasyMode", 1);
        FindObjectOfType<AudioManager>().Play("SelectSound");
        SceneManager.LoadScene("OpeningScene");
    }

    public void ContinueGame()
    {
        FindObjectOfType<AudioManager>().Play("SelectSound");
        SceneManager.LoadScene("OpeningScene");
    }

    public void QuitGame()
    {
        FindObjectOfType<AudioManager>().Play("SelectSound");
        Application.Quit();
    }

    public void NewGameDisclaimer(GameObject disclaimerPanel)
    {
        if (PlayerPrefs.GetInt("Lvl1", 0) == 1)
            disclaimerPanel.SetActive(true);
        else
            NewGame(false);
    }
}
