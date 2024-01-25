using System;
using UnityEngine;
using UnityEngine.UI;

public class MenusManager : MonoBehaviour
{
    GameObject stageSpecsInfo;
    GameObject pauseMenu;
    GameObject gameOverMenu;
    GameObject stageClearedMenu;
    GameObject levelClearedMenu;
    GameObject shader;
    EagleEyeMode eagleEye;
    
    public GameObject openMessage;

    public StageManager stageManager;

    public static bool isPaused = false;
    public bool messageOnScreen = false;
    bool eagleEyeState = false;

    public Text[] descriptions;

    void Start()
    {
        Transform auxTrans = transform.Find("StageSpecification");
        if (auxTrans == null)
            return;
        stageSpecsInfo = auxTrans.gameObject;
        auxTrans = transform.Find("PauseMenu");
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

        eagleEye = FindFirstObjectByType<EagleEyeMode>();
        if (eagleEye == null)
            return;

        shader.SetActive(false);
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        stageClearedMenu.SetActive(false);
        levelClearedMenu.SetActive(false);
        messageOnScreen = false;

        
        stageSpecsInfo.GetComponent<Text>().text = "Level " + stageManager.currentLvl + " - Stage " + stageManager.currentStage + "\n" + stageManager.stageMode;

        if (openMessage != null)
        {
            messageOnScreen = true;
            Time.timeScale = 0f;
            openMessage.SetActive(true);
            shader.SetActive(true);
            isPaused = true;
            auxTrans = openMessage.transform.Find("ContinueButton");
            if (auxTrans == null)
                return;
            auxTrans.GetComponent<Button>().Select();
        }

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Pause();
    }

    public void Pause()
    {
        if (messageOnScreen)
            return;
        if (eagleEyeState)
            return;
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        shader.SetActive(true);
        isPaused = true;
        Transform auxTrans = pauseMenu.transform.Find("ResumeButton");
        if (auxTrans == null)
            return;
        auxTrans.GetComponent<Button>().Select();
        auxTrans = pauseMenu.transform.Find("LevelText");
        if (auxTrans == null)
            return;
        auxTrans.GetComponent<Text>().text = "Level " + stageManager.currentLvl + " - Stage " + stageManager.currentStage;
    }

    public void StageCleared()
    {
        messageOnScreen = true;
        Time.timeScale = 0f;
        stageClearedMenu.SetActive(true);
        shader.SetActive(true);
        isPaused = true;
        Transform auxTrans = stageClearedMenu.transform.Find("NextStageButton");
        if (auxTrans == null)
            return;
        auxTrans.GetComponent<Button>().Select();
        auxTrans = stageClearedMenu.transform.Find("LevelText");
        if (auxTrans == null)
            return;
        auxTrans.GetComponent<Text>().text = "Level " + stageManager.currentLvl + " - Stage " + stageManager.currentStage;
    }

    public void GameOver(String deadCode)
    {
        messageOnScreen = true;
        Time.timeScale = 0f;
        Transform auxTrans = gameOverMenu.transform.Find("GameOverText");
        switch (deadCode)
        {
            case "health":
                auxTrans.GetComponent<Text>().text = "Oh no, the water in your reservoir is exshausted!\nYou've evaporated :(";
                break;
            case "heat":
                auxTrans.GetComponent<Text>().text = "Oh no, the stage became too hot!\nYou've evaporated :(";
                break;
            case "no_flower":
                auxTrans.GetComponent<Text>().text = "Oh no, there are no more flowers on the stage!\nThe biome can't be settle :(";
                break;
        }
        gameOverMenu.SetActive(true);
        shader.SetActive(true);
        isPaused = true;
        auxTrans = gameOverMenu.transform.Find("RetryButton");
        if (auxTrans == null)
            return;
        auxTrans.GetComponent<Button>().Select();
        auxTrans = gameOverMenu.transform.Find("LevelText");
        if (auxTrans == null)
            return;
        auxTrans.GetComponent<Text>().text = "Level " + stageManager.currentLvl + " - Stage " + stageManager.currentStage;
    }

    public void LevelCleared()
    {
        messageOnScreen = true;
        Time.timeScale = 0f;
        levelClearedMenu.SetActive(true);
        shader.SetActive(true);
        isPaused = true;
        Transform auxTrans = levelClearedMenu.transform.Find("WorldMapButton");
        if (auxTrans == null)
            return;
        auxTrans.GetComponent<Button>().Select();
        auxTrans = levelClearedMenu.transform.Find("LevelText");
        if (auxTrans == null)
            return;
        auxTrans.GetComponent<Text>().text = "Level " + stageManager.currentLvl + " - Stage " + stageManager.currentStage;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        if (openMessage != null)
            openMessage.SetActive(false);
        shader.SetActive(false);
        isPaused = false;
        messageOnScreen = false;
    }

    public void ExitMessage(GameObject message)
    {
        Time.timeScale = 1f;
        shader.SetActive(false);
        message.SetActive(false);
        isPaused = false;
        messageOnScreen = false;
    }

    public void GoToWorldMap()
    {
        stageManager.GoWorldMap();
    }

    public void GoNextStage()
    {
        stageManager.GoNextStage();
    }

    public void RetryStage()
    {
        stageManager.RetryStage();
    }

    public void SetIsPause(bool value)
    {
        isPaused = value;
    }

    public void ToggleEagleEye()
    {
        if (messageOnScreen)
            return;

        if (eagleEyeState)
        {
            Time.timeScale = 1f;
            isPaused = false;
            eagleEyeState = false;
            eagleEye.Exit();
        } else {
            Time.timeScale = 0.2f;
            isPaused = true;
            eagleEyeState = true;
            eagleEye.Enter();
        }
    }

    public void ShowDescriptions()
    {
        foreach(Text description in descriptions)
        {
            description.enabled = true;
        }
    }

    public void HideDescriptions()
    {
        foreach(Text description in descriptions)
        {
            description.enabled = false;
        }
    }
}
