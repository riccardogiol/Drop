using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public string nextSceneName = "WorldMap";

    public bool finalStage = false;
    public string LevelCleaned = "Lvl1";
    
    public void WinGame()
    {
        if (finalStage)
        {
            // popup menu saying you won (press enter in that menu?)
            PlayerPrefs.SetInt(LevelCleaned, 1);
            SceneManager.LoadScene("WorldMap");
        } else {
            // popup menu saying you cleared the stage
            SceneManager.LoadScene(nextSceneName);
        }
    }

    public void GameOver()
    {
         // popup menu saying you evaporated (press enter in that menu?)
        SceneManager.LoadScene("WorldMap");
    }
}
