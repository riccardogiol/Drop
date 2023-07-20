using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public string nextSceneName = "WorldMap";

    public string currentLvl = "1";
    public string currentStage = "1";

    public bool finalStage = false;
    public string LevelCleaned = "Lvl1";

    public MenusManager menusManager;
    
    public void WinGame()
    {
        if (finalStage)
        {
            PlayerPrefs.SetInt(LevelCleaned, 1);
            menusManager.LevelCleared();
        } else {
            menusManager.StageCleared();
        }
    }

    public void GameOver()
    {
         menusManager.GameOver();
    }

    public void GoNextStage()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(nextSceneName);
    }

    public void GoWorldMap()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("WorldMap");
    }
}
