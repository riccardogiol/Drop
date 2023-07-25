using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuActions : MonoBehaviour
{
    public Button continueButton;
    void Start()
    {
        if (PlayerPrefs.GetInt("Lvl1", 0) == 1)
            continueButton.interactable = true;
        else
            continueButton.interactable = false;
    }

    public void NewGame()
    {
        // add disclaimer in the case the game is already started
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("WorldMap");
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene("WorldMap");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
