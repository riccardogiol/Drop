using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenusManager : MonoBehaviour
{
    GameObject pauseMenu;
    GameObject gameOverMenu;
    GameObject stageClearedMenu;
    GameObject levelClearedMenu;
    GameObject shader;

    public StageManager stageManager;
    public PlayerMovement playerMovement;

    public bool isPaused = false;
    public bool messageOnScreen = false;

    
    void Start()
    {
        Transform auxTrans = transform.Find("PauseMenu");
        if (auxTrans == null)
            return;
        pauseMenu = auxTrans.gameObject;
        auxTrans = transform.Find("PlaygroundShader");
        if (auxTrans == null)
            return;
        shader = auxTrans.gameObject;
        
        auxTrans = transform.Find("GameOverMenu");
        if (auxTrans == null)
            return;
        gameOverMenu = auxTrans.gameObject;
        
        auxTrans = transform.Find("StageClearedMenu");
        if (auxTrans == null)
            return;
        stageClearedMenu = auxTrans.gameObject;
        
        auxTrans = transform.Find("LevelClearedMenu");
        if (auxTrans == null)
            return;
        levelClearedMenu = auxTrans.gameObject;

        shader.SetActive(false);
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        stageClearedMenu.SetActive(false);
        levelClearedMenu.SetActive(false);
        messageOnScreen = false;

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!messageOnScreen)
            {
                if (isPaused)
                    Resume();
                else
                    Pause();
            }
        }
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        shader.SetActive(true);
        playerMovement.enabled = false;
        isPaused = true;
        Transform auxTrans = pauseMenu.transform.Find("ResumeButton");
        if (auxTrans == null)
            return;
        auxTrans.GetComponent<Button>().Select();
        auxTrans = pauseMenu.transform.Find("LevelText");
        if (auxTrans == null)
            return;
        auxTrans.GetComponent<TextMeshProUGUI>().text = "Level " + stageManager.currentLvl + " - Stage " + stageManager.currentStage;
    }

    public void StageCleared()
    {
        messageOnScreen = true;
        Time.timeScale = 0f;
        stageClearedMenu.SetActive(true);
        shader.SetActive(true);
        playerMovement.enabled = false;
        isPaused = true;
        Transform auxTrans = stageClearedMenu.transform.Find("NextStageButton");
        if (auxTrans == null)
            return;
        auxTrans.GetComponent<Button>().Select();
        auxTrans = stageClearedMenu.transform.Find("LevelText");
        if (auxTrans == null)
            return;
        auxTrans.GetComponent<TextMeshProUGUI>().text = "Level " + stageManager.currentLvl + " - Stage " + stageManager.currentStage;
    }

    public void GameOver()
    {
        messageOnScreen = true;
        Time.timeScale = 0f;
        gameOverMenu.SetActive(true);
        shader.SetActive(true);
        playerMovement.enabled = false;
        isPaused = true;
        Transform auxTrans = gameOverMenu.transform.Find("WorldMapButton");
        if (auxTrans == null)
            return;
        auxTrans.GetComponent<Button>().Select();
        auxTrans = gameOverMenu.transform.Find("LevelText");
        if (auxTrans == null)
            return;
        auxTrans.GetComponent<TextMeshProUGUI>().text = "Level " + stageManager.currentLvl + " - Stage " + stageManager.currentStage;
    }

    public void LevelCleared()
    {
        messageOnScreen = true;
        Time.timeScale = 0f;
        levelClearedMenu.SetActive(true);
        shader.SetActive(true);
        playerMovement.enabled = false;
        isPaused = true;
        Transform auxTrans = levelClearedMenu.transform.Find("WorldMapButton");
        if (auxTrans == null)
            return;
        auxTrans.GetComponent<Button>().Select();
        auxTrans = levelClearedMenu.transform.Find("LevelText");
        if (auxTrans == null)
            return;
        auxTrans.GetComponent<TextMeshProUGUI>().text = "Level " + stageManager.currentLvl + " - Stage " + stageManager.currentStage;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        shader.SetActive(false);
        playerMovement.enabled = true;
        isPaused = false;
    }
}
