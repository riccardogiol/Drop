using UnityEngine;
using UnityEngine.SceneManagement;

public class DemoVersionChecker : MonoBehaviour
{
    public StageManager stageManager;
    public bool toWorldMap = false;

    public void CheckVersionNextStage()
    {
        if (PlayerPrefs.GetInt("DemoVersion", 0) == 0)
        {
            if (toWorldMap)
                stageManager.GoWorldMap();
            else
                stageManager.GoNextStage();
        }
        else
        {
            Time.timeScale = 1f;
            MenusManager.isPaused = false;
            SceneManager.LoadScene("DemoEpilogueScene");
        }
    }
}
