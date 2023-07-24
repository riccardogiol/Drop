using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public int currentLvl = 1;
    public int currentStage = 1;

    public bool finalStage = false;

    public MenusManager menusManager;

    //Start procedure functions
    
    public void WinGame()
    {
        if (finalStage)
        {
            PlayerPrefs.SetInt("Lvl" + currentLvl, 1);
            menusManager.LevelCleared();
        } else {
            menusManager.StageCleared();
        }
    }

    public void GameOver()
    {
         menusManager.GameOver();
    }

    //Close stage functions

    public void GoNextStage()
    {
        Time.timeScale = 1f;
        MenusManager.isPaused = false;
        string nextSceneName = "Stage" + currentLvl + "-" + (currentStage + 1);
        SceneManager.LoadScene(nextSceneName);
    }

    public void GoWorldMap()
    {
        Time.timeScale = 1f;
        MenusManager.isPaused = false;
        SceneManager.LoadScene("WorldMap");
    }
}
