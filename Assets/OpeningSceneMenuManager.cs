using UnityEngine;
using UnityEngine.SceneManagement;

public class OpeningSceneMenuManager : MonoBehaviour
{

    void Awake()
    {
        FindObjectOfType<AudioManager>().Play("OpeningMusic");
        FindObjectOfType<AudioManager>().StopStageMusic();
        FindObjectOfType<AudioManager>().ResetSounds();
    }

    public void EndIntroductionSlides()
    {
        if (PlayerPrefs.GetInt("Lvl1", 0) == 0 && PlayerPrefs.GetInt("LastStageCompleted", 0) == 0)
        {
            SceneManager.LoadScene("Stage1-1");
        }
        else
        {
            SceneManager.LoadScene("WorldMap");
        }
    }
    
    public void ResetSounds()
    {
        FindObjectOfType<AudioManager>().ResetSounds();
    }
    
    public void GoToCredits()
    {
        PlayerPrefs.SetInt("TriggerEndingScene", 0);
        SceneManager.LoadScene("CreditScene");
    }

     public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
