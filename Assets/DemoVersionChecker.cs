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
            SceneManager.LoadScene("OpeningScene");
    }
}
