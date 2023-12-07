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

    public void NewGame()
    {
        // add disclaimer in the case the game is already started
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("Lvl0", 1);
        PlayerPrefs.SetInt("FromMainMenu", 1);
        FindObjectOfType<AudioManager>().Play("SelectSound");
        SceneManager.LoadScene("WorldMap");
    }

    public void ContinueGame()
    {
        PlayerPrefs.SetInt("FromMainMenu", 1);
        FindObjectOfType<AudioManager>().Play("SelectSound");
        SceneManager.LoadScene("WorldMap");
    }

    public void QuitGame()
    {
        FindObjectOfType<AudioManager>().Play("SelectSound");
        Application.Quit();
    }
}
