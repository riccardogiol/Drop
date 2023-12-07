using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelMessageManager : MonoBehaviour
{
    public int levelCode;
    public Button continueButton;
    void Start()
    {
        if ((PlayerPrefs.GetInt("Lvl" + levelCode, 0) == 0) && (PlayerPrefs.GetInt("LastStageCompleted", 0) > 0))
            continueButton.interactable = true;
        else
            continueButton.interactable = false;
        
    }

    public void StartAction()
    {
        PlayerPrefs.SetInt("LastLevelPlayed", levelCode);
        FindObjectOfType<AudioManager>().Play("SelectSound");
        SceneManager.LoadScene("Stage" + levelCode + "-1");
    }

    public void ContinueAction()
    {
        PlayerPrefs.SetInt("LastLevelPlayed", levelCode);
        FindObjectOfType<AudioManager>().Play("SelectSound");
        SceneManager.LoadScene("Stage" + levelCode + "-" + (PlayerPrefs.GetInt("LastStageCompleted", 0) + 1));
    }
}
