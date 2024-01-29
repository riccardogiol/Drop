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

    public void EndIntroductionSlides(GameObject message)
    {
        if (PlayerPrefs.GetInt("Lvl1", 0) == 0 && PlayerPrefs.GetInt("LastStageCompleted", 0) == 0 )
        {
            SceneManager.LoadScene("Stage1-1");
        } else {
            message.SetActive(false);
        }
    }

    public void GoMainMenu()
    {
        FindObjectOfType<AudioManager>().Play("SelectSound");
        SceneManager.LoadScene("MainMenu");
    }
}
