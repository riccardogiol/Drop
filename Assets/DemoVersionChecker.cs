using UnityEngine;
using UnityEngine.SceneManagement;

public class DemoVersionChecker : MonoBehaviour
{
    public StageManager stageManager;

    public void CheckVersionNextStage()
    {
        if (PlayerPrefs.GetInt("DemoVersion", 0) == 0)
            stageManager.GoNextStage();
        else
        {
            Time.timeScale = 1f;
            MenusManager.isPaused = false;
            SceneManager.LoadScene("DemoEpilogueScene");
        }
    }
}
