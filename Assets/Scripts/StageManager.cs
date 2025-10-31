using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services.Analytics;
using Cinemachine;

public class StageManager : MonoBehaviour
{
    public int currentLvl = 1;
    public int currentStage = 1;
    public string stageMode = "puzzle";
    public string trophyName = "";

    public bool finalStage = false;
    public Transform bossTransform;
    VictoryPositionTrigger victoryPositionTrigger;
    bool gameOver = false;

    public MenusManager menusManager;

    PlayerAnimationManager playerAnimationManager;

    CameraAnimationManager cameraAnimationManager;
    EagleEyeMode eagleEyeMode;
    PlayerMovementPath playerMovementPath;
    PlayerMovementKeys playerMovementKeys;
    PlayerEventParamsManager playerEventPrams;

    float startTime;
    int stageInstanceCode;

    void Awake()
    {

        playerMovementPath = FindObjectOfType<PlayerMovementPath>();
        playerMovementKeys = FindObjectOfType<PlayerMovementKeys>();
        cameraAnimationManager = FindObjectOfType<CameraAnimationManager>();
        eagleEyeMode = FindObjectOfType<EagleEyeMode>();
        victoryPositionTrigger = FindObjectOfType<VictoryPositionTrigger>();
        playerAnimationManager = FindObjectOfType<PlayerAnimationManager>();

        playerEventPrams = FindObjectOfType<PlayerEventParamsManager>();

        PlayerPrefs.SetInt("LastStagePlayed", currentStage);
        PlayerPrefs.SetInt("LastLevelPlayed", currentLvl);

    }

    void Start()
    {
        FindObjectOfType<AudioManager>().Stop("OpeningMusic");
        if (bossTransform != null)
            FindObjectOfType<AudioManager>().StartBossMusic();
        else if (currentStage == 1)
            FindObjectOfType<AudioManager>().StartStageMusic(3);
        else if (currentStage == 4)
            FindObjectOfType<AudioManager>().StartStageMusic(2);
        else
            FindObjectOfType<AudioManager>().StartStageMusic(1);

        startTime = Time.time;
        stageInstanceCode = UnityEngine.Random.Range(1, 999999999);

        float currentRatio = 0;
        Camera cam = FindFirstObjectByType<Camera>();
        if (cam!= null)
            currentRatio = cam.aspect;

        try {
        AnalyticsService.Instance.RecordEvent(new StageStartEvent{
            StageID = currentStage,
            LevelID = currentLvl,
            StageInstanceCode = stageInstanceCode,
            CameraRatio = currentRatio
            });
        } catch
        {
            Debug.Log("Analytic service are not initialized, message not sent to cloud DB");
        }
    }
    
    public void WinGame(bool waterTiles = false, float waitSeconds = 6f)
    {
        PlayerPrefs.SetInt("ConsecutiveDeaths", 0);
        if (!MenusManager.isPaused)
            StartCoroutine(WinningScene(waterTiles, waitSeconds));
    }

    IEnumerator WinningScene(bool waterTiles, float waitSeconds)
    {
        eagleEyeMode.Exit();
        yield return new WaitForSeconds(0.5f);
        if (gameOver)
            yield break;
        FindFirstObjectByType<PlaygroundManager>().MakeRain(true, waterTiles);
        FindFirstObjectByType<PlaygroundManager>().EstinguishAllFlames();
        cameraAnimationManager.StartEndingAnimation();
        
        try {
        AnalyticsService.Instance.RecordEvent(new StageCompleteEvent{
            StageID = currentStage,
            LevelID = currentLvl,
            StageInstanceCode = stageInstanceCode,
            TimeElapsed = Time.time - startTime,
            PlayerPositionX = playerEventPrams.GetPositionX(),
            PlayerPositionY = playerEventPrams.GetPositionY(),
            WaterBulletUsage = playerEventPrams.GetWaterBulletUsage(),
            WaveUsage = playerEventPrams.GetWaveUsage(),
            HealthLeft = playerEventPrams.GetHealth(),
            ScoutCloudUsage = menusManager.ScoutCloudUsage
            });
        } catch
        {
            Debug.Log("Analytic service are not initialized, message not sent to cloud DB");
        }

        if (finalStage)
        {
            victoryPositionTrigger = FindObjectOfType<VictoryPositionTrigger>();
            if (victoryPositionTrigger != null)
            {
                victoryPositionTrigger.ActivateCollider();
                playerMovementPath.NewTarget(victoryPositionTrigger.transform.position);
            } else {
                playerMovementPath.InterruptMovement();
                playerAnimationManager.PlayTriumph();
                FindObjectOfType<AudioManager>().Play("WinJingle");
                FindObjectOfType<AudioManager>().PlayVoice("Win");
            }
            bool isBoss = bossTransform != null;
            if (isBoss)
            {
                CinemachineVirtualCamera cinemachineVirtual = FindFirstObjectByType<CinemachineVirtualCamera>();
                if (cinemachineVirtual != null)
                {
                    cinemachineVirtual.Follow = bossTransform;
                    cinemachineVirtual.LookAt = bossTransform;
                }
            }
            
            playerMovementKeys.InterruptMovement(0.3f);
            menusManager.SetIsPause(true);

            if (isBoss)
            {
                yield return new WaitForSeconds(waitSeconds);
            } else {
                yield return new WaitForSeconds(5);
            }

            if (PlayerPrefs.GetInt("Lvl" + currentLvl, 0) == 0)
            {
                PlayerPrefs.SetInt("LastStageCompleted", 0);
                PlayerPrefs.SetInt("LastLevelCompleted", currentLvl);
            }

            PlayerPrefs.SetInt("Lvl" + currentLvl, 1);
            
            SaveData saveData = SaveManager.Load();
            saveData.StageCompleteStatus[(currentLvl - 1) * 4 + currentStage] = 1;
            if (isBoss)
                for (int i = 1; i <= 4; i++)
                    saveData.StageCompleteStatus[(currentLvl - 1) * 4 + i] = 1;
            SaveManager.Save(saveData);

            menusManager.LevelCleared();
        } else
        {
            playerMovementPath.InterruptMovement();
            playerMovementKeys.InterruptMovement(0.3f);
            playerAnimationManager.PlayTriumph();
            FindObjectOfType<AudioManager>().Play("WinJingle");
            FindObjectOfType<AudioManager>().PlayVoice("Win");
            menusManager.SetIsPause(true);
            yield return new WaitForSeconds(3);

            if (PlayerPrefs.GetInt("Lvl" + currentLvl, 0) == 0 && PlayerPrefs.GetInt("LastStageCompleted", 0) < currentStage)
                PlayerPrefs.SetInt("LastStageCompleted", currentStage);
            
            SaveData saveData = SaveManager.Load();
            saveData.StageCompleteStatus[(currentLvl - 1) * 4 + currentStage] = 1;
            SaveManager.Save(saveData);

            menusManager.StageCleared();
        }
    }

    public void GameOver(String deadCode)
    {
        PlayerPrefs.SetInt("ConsecutiveDeaths", PlayerPrefs.GetInt("ConsecutiveDeaths", 0) + 1);
        gameOver = true;
        if (!MenusManager.isPaused)
            StartCoroutine(EvaporatingScene(deadCode));
    }

    IEnumerator EvaporatingScene(String deadCode)
    {
        eagleEyeMode.Exit();
        menusManager.SetIsPause(true);
        if (playerMovementPath != null)
            playerMovementPath.InterruptMovement();
        playerMovementKeys.InterruptMovement(0.3f);
        cameraAnimationManager.StartEndingAnimation();
        if (deadCode != "no_flower")
            playerAnimationManager.PlayEvaporation();
        FindObjectOfType<AudioManager>().Play("LoseJingle");
        FindObjectOfType<AudioManager>().PlayVoice("Die");

        yield return new WaitForSeconds(3);
        
        try {

        AnalyticsService.Instance.RecordEvent(new GameOverEvent{
            StageID = currentStage,
            LevelID = currentLvl,
            StageInstanceCode = stageInstanceCode,
            TimeElapsed = Time.time - startTime,
            PlayerPositionX = playerEventPrams.GetPositionX(),
            PlayerPositionY = playerEventPrams.GetPositionY(),
            WaterBulletUsage = playerEventPrams.GetWaterBulletUsage(),
            WaveUsage = playerEventPrams.GetWaveUsage(),
            HealthLeft = playerEventPrams.GetHealth(),
            ScoutCloudUsage = menusManager.ScoutCloudUsage,
            GameOverCode = deadCode
            });
        } catch
        {
            Debug.Log("Analytic service are not initialized, message not sent to cloud DB");
        }

        menusManager.GameOver(deadCode);
    }
    
    public void Reborn()
    {
        if (!MenusManager.isPaused)
            StartCoroutine(RebornEvaporatingScene());
    }

    IEnumerator RebornEvaporatingScene()
    {
        eagleEyeMode.Exit();
        menusManager.SetIsPause(true);
        if (playerMovementPath != null)
            playerMovementPath.InterruptMovement();
        playerMovementKeys.InterruptMovement(0.3f);
        cameraAnimationManager.StartEndingAnimation();
        playerAnimationManager.PlayReborn();

        yield return new WaitForSeconds(3);

        cameraAnimationManager.RevertEndingAnimation();

        yield return new WaitForSeconds(2);

        menusManager.SetIsPause(false);
        
    }

    //Close stage functions

    public void RestartStage()
    {
        try {
        AnalyticsService.Instance.RecordEvent(new StageRestartEvent{
            StageID = currentStage,
            LevelID = currentLvl,
            StageInstanceCode = stageInstanceCode,
            TimeElapsed = Time.time - startTime,
            PlayerPositionX = playerEventPrams.GetPositionX(),
            PlayerPositionY = playerEventPrams.GetPositionY(),
            WaterBulletUsage = playerEventPrams.GetWaterBulletUsage(),
            WaveUsage = playerEventPrams.GetWaveUsage(),
            HealthLeft = playerEventPrams.GetHealth(),
            ScoutCloudUsage = menusManager.ScoutCloudUsage
            });
        } catch
        {
            Debug.Log("Analytic service are not initialized, message not sent to cloud DB");
        }
        PlayerPrefs.SetInt("ConsecutiveDeaths", PlayerPrefs.GetInt("ConsecutiveDeaths", 0) + 1);
        Time.timeScale = 1f;
        MenusManager.isPaused = false;
        string nextSceneName = "Stage" + currentLvl + "-" + currentStage;

        FindObjectOfType<AudioManager>().ResetSounds();
        SceneManager.LoadScene(nextSceneName);
    }

    public void RetryStage()
    {
        Time.timeScale = 1f;
        MenusManager.isPaused = false;
        string nextSceneName = "Stage" + currentLvl + "-" + currentStage;
        
        FindObjectOfType<AudioManager>().ResetSounds();
        SceneManager.LoadScene(nextSceneName);
    }

    public void GoNextStage()
    {
        Time.timeScale = 1f;
        MenusManager.isPaused = false;
        string nextSceneName = "Stage" + currentLvl + "-" + (currentStage + 1);
        
        FindObjectOfType<AudioManager>().ResetSounds();
        SceneManager.LoadScene(nextSceneName);
    }

    public void GoWorldMap()
    {
        Time.timeScale = 1f;
        MenusManager.isPaused = false;
        
        FindObjectOfType<AudioManager>().ResetSounds();
        SceneManager.LoadScene("WorldMap");
    }

    public void LeaveStage()
    {
        try {
        AnalyticsService.Instance.RecordEvent(new StageRestartEvent{
            StageID = currentStage,
            LevelID = currentLvl,
            StageInstanceCode = stageInstanceCode,
            TimeElapsed = Time.time - startTime,
            PlayerPositionX = playerEventPrams.GetPositionX(),
            PlayerPositionY = playerEventPrams.GetPositionY(),
            WaterBulletUsage = playerEventPrams.GetWaterBulletUsage(),
            WaveUsage = playerEventPrams.GetWaveUsage(),
            HealthLeft = playerEventPrams.GetHealth(),
            ScoutCloudUsage = menusManager.ScoutCloudUsage
            });
        } catch
        {
            Debug.Log("Analytic service are not initialized, message not sent to cloud DB");
        }
        Debug.Log("LeaveStage");
        Time.timeScale = 1f;
        MenusManager.isPaused = false;
        
        FindObjectOfType<AudioManager>().ResetSounds();
        SceneManager.LoadScene("WorldMap");
    }

    public void ShowButtonDescription()
    {
        menusManager.ShowDescriptions();
    }

    public void HideButtonDescription()
    {
        menusManager.HideDescriptions();
    }
}
