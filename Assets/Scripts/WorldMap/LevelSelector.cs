using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    public Button[] LevelButton;

    void Start()
    {
        PlayerPrefs.SetInt("Lvl0", 1);
        for (int i = 0; i < LevelButton.Length; i++)
        {
            if (PlayerPrefs.GetInt(LevelButton[i].name, 0) == 1)
            {
                LevelButton[i].interactable = true;
            }
        }
        
        FindObjectOfType<AudioManager>().Play("OpeningMusic");
        FindObjectOfType<AudioManager>().Stop("BackgroundMusic");

    }

    public void OpenLevel(string sceneName)
    {
        FindObjectOfType<AudioManager>().Play("SelectSound");
        SceneManager.LoadScene(sceneName);
    }

    public void GoMainMenu()
    {
        FindObjectOfType<AudioManager>().Play("SelectSound");
        SceneManager.LoadScene("MainMenu");
    }
}
