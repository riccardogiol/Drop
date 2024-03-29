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
    public GameObject[] overlayMessages;

    bool musicState;

    public Text[] descriptions;

    public GameObject[] buttonHints;

    void Start()
    {
        Transform auxTrans = transform.Find("ProgressionBar");
        if (auxTrans == null)
            return;
        auxTrans = auxTrans.Find("StageSpecification");
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

        musicState = PlayerPrefs.GetInt("MusicState", 0) == 1;
        
        stageSpecsInfo.GetComponent<Text>().text = stageManager.currentLvl + "." + stageManager.currentStage + " - " + stageManager.stageMode;

        foreach(GameObject bh in buttonHints)
            bh.SetActive(false);

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
        } else {
            if (overlayMessages.Length > 0)
            {
                foreach(GameObject om in overlayMessages)
                    om.SetActive(true);
            }
        }

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenu.activeSelf)
                Resume();
            else
                Pause();
        }
        if (Input.GetKeyDown(KeyCode.H))
            ToggleEagleEye();
    }

    public void Pause()
    {
        if (messageOnScreen)
            return;
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        shader.SetActive(true);
        foreach(GameObject bh in buttonHints)
            bh.SetActive(true);
        isPaused = true;
        Transform auxTrans = pauseMenu.transform.Find("ResumeButton");
        if (auxTrans == null)
            return;
        auxTrans.GetComponent<Button>().Select();

        UpdateMusicToggleText();
        
        auxTrans = pauseMenu.transform.Find("LevelText");
        if (auxTrans == null)
            return;
        auxTrans.GetComponent<Text>().text = "Level " + stageManager.currentLvl + " - Stage " + stageManager.currentStage + "\n" + stageManager.stageMode;
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
                auxTrans.GetComponent<Text>().text = "Energy exhausted\nOh no, you've evaporated :(";
                break;
            case "heat":
                auxTrans.GetComponent<Text>().text = "Stage too hot!\nOh no, you've evaporated :(";
                break;
            case "no_flower":
                auxTrans.GetComponent<Text>().text = "No more flowers\nOh no, the biome can't be settle :(";
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
        foreach(GameObject bh in buttonHints)
            bh.SetActive(false);
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
        if (overlayMessages.Length > 0)
        {
            foreach(GameObject om in overlayMessages)
                om.SetActive(true);
        }
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

    //this function manage the button to activate EE: countdown if it is possible to press it or not
    public void ToggleEagleEye()
    {
        if (messageOnScreen)
            return;

        // sostituire con un counter per il bottone come per i poteri
        // exit arriva dopo qualche secondo dell'enter OPPURE se finisce il livello
        // if possible to activate
        //if (eagleEyeState)
        //{
            eagleEye.Enter();
        //}
    }

    public void ToggleMusic()
    {
        if (musicState)
        {
            FindFirstObjectByType<AudioManager>().SetVolume(0f);
            musicState = false;
            PlayerPrefs.SetInt("MusicState", 0);
        } else {
            FindFirstObjectByType<AudioManager>().SetVolume(0.2f);
            musicState = true;
            PlayerPrefs.SetInt("MusicState", 1);
        }
        UpdateMusicToggleText();
    }

    void UpdateMusicToggleText()
    {
        Transform auxTrans = pauseMenu.transform.Find("ToggleMusicButton");
        auxTrans = auxTrans.transform.Find("Text");
        if (musicState)
            auxTrans.GetComponent<Text>().text = "Music: off";
        else
            auxTrans.GetComponent<Text>().text = "Music: on";

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
