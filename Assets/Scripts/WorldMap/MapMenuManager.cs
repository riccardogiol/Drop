using UnityEngine;
using UnityEngine.SceneManagement;

public class MapMenuManager : MonoBehaviour
{
    public GameObject StoryDisplay;
    public OutOfWallDecorationManager outOfWallDecorationManager;

    void Awake()
    {
        FindObjectOfType<AudioManager>().Play("OpeningMusic");
        FindObjectOfType<AudioManager>().Stop("BackgroundMusic");
        if (PlayerPrefs.GetInt("FromMainMenu", 0) == 1)
            StoryDisplay.SetActive(true);
        PlayerPrefs.SetInt("FromMainMenu", 0);
    }

    void Start()
    {
        outOfWallDecorationManager.SpawnDecorations(15, 15);
        outOfWallDecorationManager.SetCleanValue(1);
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
