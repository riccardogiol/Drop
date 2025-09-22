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
    GameObject diffChangeMessage;


    public int ScoutCloudUsage;
    
    public GameObject openMessage;

    public StageManager stageManager;

    public static bool isPaused = false;
    public bool messageOnScreen = false;
    public GameObject[] overlayMessages;

    public Text[] descriptions;

    public GameObject[] buttonHints;
    public GameObject movementKeys;

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

        auxTrans = transform.Find("OnScreenMessageDifficulty");
        if (auxTrans == null)
            return;
        diffChangeMessage = auxTrans.gameObject;

        eagleEye = FindFirstObjectByType<EagleEyeMode>();
        if (eagleEye == null)
            return;

        shader.SetActive(false);
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        stageClearedMenu.SetActive(false);
        levelClearedMenu.SetActive(false);
        messageOnScreen = false;

        if ((PlayerPrefs.GetInt("ConsecutiveDeaths", 0) >= PlayerPrefs.GetInt("ConsecutiveDeathsLimit", 3)) && (PlayerPrefs.GetInt("EasyMode", 0) == 0))
        {
            diffChangeMessage.SetActive(true);
            PlayerPrefs.SetInt("ConsecutiveDeathsLimit", PlayerPrefs.GetInt("ConsecutiveDeathsLimit", 3) + 3);
        }

        string stageMode = SingletonLocalizationManager.instance.GetComponent<LocalizationManager>().Get("menu.stage.interface_info." + stageManager.stageMode);
        if (stageMode == null)
            stageMode = stageManager.stageMode;
        
        stageSpecsInfo.GetComponent<Text>().text = stageManager.currentLvl + "." + stageManager.currentStage + " - " + stageMode;

        if (PlayerPrefs.GetInt("ShowButtonHint", 0) == 0)
        {
            foreach (GameObject bh in buttonHints)
                    bh.SetActive(false);
        }
        movementKeys.SetActive(false);

        HideDescriptions();

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
        else
        {
            if (overlayMessages.Length > 0)
            {
                foreach (GameObject om in overlayMessages)
                    om.SetActive(true);
            }
        }

        ScoutCloudUsage = 0;

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
        movementKeys.SetActive(true);
        ShowDescriptions();
        isPaused = true;
        Transform auxTrans = pauseMenu.transform.Find("ResumeButton");
        if (auxTrans == null)
            return;
        auxTrans.GetComponent<Button>().Select();
        
        auxTrans = pauseMenu.transform.Find("LevelText");
        if (auxTrans == null)
            return;

        string levelLoc = SingletonLocalizationManager.instance.GetComponent<LocalizationManager>().Get("menu.stage.pause_menu.level");
        if (levelLoc == null)
            levelLoc = "Level";

        string stageLoc = SingletonLocalizationManager.instance.GetComponent<LocalizationManager>().Get("menu.stage.pause_menu.stage");
        if (stageLoc == null)
            stageLoc = "Stage";
        
        string stageMode = SingletonLocalizationManager.instance.GetComponent<LocalizationManager>().Get("menu.stage.interface_info." + stageManager.stageMode);
        if (stageMode == null)
            stageMode = stageManager.stageMode;
        auxTrans.GetComponent<Text>().text = levelLoc + " " + stageManager.currentLvl + " - " + stageLoc + " " + stageManager.currentStage + "\n" + stageMode;
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

        string levelLoc = SingletonLocalizationManager.instance.GetComponent<LocalizationManager>().Get("menu.stage.pause_menu.level");
        if (levelLoc == null)
            levelLoc = "Level";

        string stageLoc = SingletonLocalizationManager.instance.GetComponent<LocalizationManager>().Get("menu.stage.pause_menu.stage");
        if (stageLoc == null)
            stageLoc = "Stage";

        auxTrans.GetComponent<Text>().text = (levelLoc + " " + stageManager.currentLvl + " - " + stageLoc + " " + stageManager.currentStage).ToUpper();
    }

    public void GameOver(String deadCode)
    {
        messageOnScreen = true;
        Time.timeScale = 0f;
        Transform auxTrans = gameOverMenu.transform.Find("GameOverText");
        string messageLoc;
        switch (deadCode)
        {
            case "health":
                messageLoc = SingletonLocalizationManager.instance.GetComponent<LocalizationManager>().Get("message.stage.game_over_health");
                if (messageLoc == null)
                    messageLoc = "Energy exhausted\nOh no, you've evaporated :(";
                auxTrans.GetComponent<Text>().text = messageLoc;
                break;
            case "heat":
                messageLoc = SingletonLocalizationManager.instance.GetComponent<LocalizationManager>().Get("message.stage.game_over_heat");
                if (messageLoc == null)
                    messageLoc = "Stage too hot!\nOh no, you've evaporated :(";
                auxTrans.GetComponent<Text>().text = messageLoc;
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
        
        string levelLoc = SingletonLocalizationManager.instance.GetComponent<LocalizationManager>().Get("menu.stage.pause_menu.level");
        if (levelLoc == null)
            levelLoc = "Level";

        string stageLoc = SingletonLocalizationManager.instance.GetComponent<LocalizationManager>().Get("menu.stage.pause_menu.stage");
        if (stageLoc == null)
            stageLoc = "Stage";

        auxTrans.GetComponent<Text>().text = (levelLoc + " " + stageManager.currentLvl + " - " + stageLoc + " " + stageManager.currentStage).ToUpper();
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

        // label? Livello xxx ripulito?
        auxTrans = levelClearedMenu.transform.Find("LevelText");
        string levelLoc = SingletonLocalizationManager.instance.GetComponent<LocalizationManager>().Get("menu.stage.pause_menu.level");
        if (levelLoc == null)
            levelLoc = "Level";
        string stageLoc = SingletonLocalizationManager.instance.GetComponent<LocalizationManager>().Get("menu.stage.pause_menu.stage");
        if (stageLoc == null)
            stageLoc = "Stage";
        if (auxTrans != null)
            auxTrans.GetComponent<Text>().text = (levelLoc + " " + stageManager.currentLvl + " - " + stageLoc + " " + stageManager.currentStage).ToUpper();


        auxTrans = levelClearedMenu.transform.Find("Title");
        string nameWarticle = SingletonLocalizationManager.instance.GetComponent<LocalizationManager>().Get("content.trophy." + stageManager.trophyName + ".name_w_article");
        if (nameWarticle == null)
            nameWarticle = "the XXX";
        string title1, title2;
        if (stageManager.bossTransform != null)
        {
            title1 = SingletonLocalizationManager.instance.GetComponent<LocalizationManager>().Get("message.stage.level_clear.titleBoss");
            if (title1 == null)
                title1 = "You released the waters of ";
            title2 = "";
        }
        else
        {
            title1 = SingletonLocalizationManager.instance.GetComponent<LocalizationManager>().Get("message.stage.level_clear.title1");
            if (title1 == null)
                title1 = "You saved ";
            title2 = SingletonLocalizationManager.instance.GetComponent<LocalizationManager>().Get("message.stage.level_clear.title2");
            if (title2 == null)
                title2 = " from fire";
        }
        if (auxTrans != null)
            auxTrans.GetComponent<Text>().text = (title1 + nameWarticle + title2).ToUpper();

        auxTrans = levelClearedMenu.transform.Find("ImageBox").Find("ImageNameBox").Find("ImageName");
        string latinName = SingletonLocalizationManager.instance.GetComponent<LocalizationManager>().Get("content.trophy." + stageManager.trophyName + ".latin_name");
        if (latinName == null)
            latinName = "XXXum";
        if (auxTrans != null)
        {
            auxTrans.GetComponent<Text>().text = latinName.ToUpper();
            auxTrans.GetComponent<FitBoxText>().Resize();
        }

        auxTrans = levelClearedMenu.transform.Find("DescriptionBox").Find("DescriptionText");
        string description = SingletonLocalizationManager.instance.GetComponent<LocalizationManager>().Get("content.trophy." + stageManager.trophyName + ".description");
        if (description == null)
            description = "Lorem ipsum ...";
        if (auxTrans != null)
        {
            auxTrans.GetComponent<Text>().text = description;
            auxTrans.GetComponent<FitBoxText>().Resize();
        }

        auxTrans = levelClearedMenu.transform.Find("DescriptionBox").Find("DescriptionTitleBox").Find("DescriptionTitle");
        string prize = SingletonLocalizationManager.instance.GetComponent<LocalizationManager>().Get("content.trophy." + stageManager.trophyName + ".prize");
        if (prize != null)
        {
            if (auxTrans != null)
            {
                auxTrans.GetComponent<Text>().text = prize;
                auxTrans.GetComponent<FitBoxText>().Resize();
            }
        }
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        if (PlayerPrefs.GetInt("ShowButtonHint", 0) == 0)
        {
            foreach (GameObject bh in buttonHints)
                    bh.SetActive(false);
        }
        movementKeys.SetActive(false);
        HideDescriptions();
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

    public void LeaveStage()
    {
        stageManager.LeaveStage();
    }

    public void GoNextStage()
    {
        stageManager.GoNextStage();
    }

    public void RestartStage()
    {
        stageManager.RestartStage();
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
            ScoutCloudUsage++;
            eagleEye.Enter();
        //}
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
        if (PlayerPrefs.GetInt("ShowButtonHint", 0) == 1)
            return;
        foreach (Text description in descriptions)
        {
            description.enabled = false;
        }
    }
}
