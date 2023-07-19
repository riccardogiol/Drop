using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public string nextSceneName = "WorldMap";

    public bool finalStage = false;
    public string[] LevelsUnlocked;
    
    public void WinGame()
    {
        if (finalStage)
        {
            // popup menu saying you won (press enter in that menu?)
            for (int i = 0; i < LevelsUnlocked.Length; i++)
            {
                PlayerPrefs.SetInt(LevelsUnlocked[i], 1);
            }
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
