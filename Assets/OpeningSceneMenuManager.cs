using UnityEngine;
using UnityEngine.SceneManagement;

public class OpeningSceneMenuManager : MonoBehaviour
{

    void Awake()
    {
        FindObjectOfType<AudioManager>().Play("OpeningMusic");
        FindObjectOfType<AudioManager>().Stop("BackgroundMusic");
    }

    public void EndIntroductionSlides()
    {
        if (PlayerPrefs.GetInt("Lvl1", 0) == 0 && PlayerPrefs.GetInt("LastStageCompleted", 0) == 0 )
        {
            SceneManager.LoadScene("Stage1-1");
        } else {
            SceneManager.LoadScene("WorldMap");
        }
    }
}
