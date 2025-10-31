using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    public GameObject StoryDisplay;
    public Button[] LevelButton;

    void Start()
    {
        for (int i = 0; i < LevelButton.Length; i++)
        {
            if (PlayerPrefs.GetInt(LevelButton[i].name, 0) == 1)
            {
                LevelButton[i].interactable = true;
            }
        }
        FindObjectOfType<AudioManager>().Play("OpeningMusic");
        FindObjectOfType<AudioManager>().StopStageMusic();
        if (PlayerPrefs.GetInt("FromMainMenu", 0) == 1)
            StoryDisplay.SetActive(true);
        PlayerPrefs.SetInt("FromMainMenu", 0);
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
