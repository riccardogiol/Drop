using UnityEngine;
using UnityEngine.SceneManagement;

public class MapMenuManager : MonoBehaviour
{
    public GameObject StoryDisplay;

    void Start()
    {
        FindObjectOfType<AudioManager>().Play("OpeningMusic");
        FindObjectOfType<AudioManager>().Stop("BackgroundMusic");
        if (PlayerPrefs.GetInt("FromMainMenu", 0) == 1)
            StoryDisplay.SetActive(true);
        PlayerPrefs.SetInt("FromMainMenu", 0);
    }

    public void OpenLevel(int levelCode)
    {
        PlayerPrefs.SetInt("LastLevelPlayed", levelCode);
        FindObjectOfType<AudioManager>().Play("SelectSound");
        SceneManager.LoadScene("Stage" + levelCode + "-1");
    }

    public void GoMainMenu()
    {
        FindObjectOfType<AudioManager>().Play("SelectSound");
        SceneManager.LoadScene("MainMenu");
    }
}
