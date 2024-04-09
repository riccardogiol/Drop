using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services.Analytics;

public class StageManager : MonoBehaviour
{
    public int currentLvl = 1;
    public int currentStage = 1;
    public string stageMode = "puzzle";

    public bool finalStage = false;
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

    void Start()
    {
        FindObjectOfType<AudioManager>().Stop("OpeningMusic");
        FindObjectOfType<AudioManager>().Play("BackgroundMusic");
        playerMovementPath = FindObjectOfType<PlayerMovementPath>();
        playerMovementKeys = FindObjectOfType<PlayerMovementKeys>();
        cameraAnimationManager = FindObjectOfType<CameraAnimationManager>();
        eagleEyeMode = FindObjectOfType<EagleEyeMode>();
        victoryPositionTrigger = FindObjectOfType<VictoryPositionTrigger>();
        playerAnimationManager = FindObjectOfType<PlayerAnimationManager>();

        playerEventPrams = FindObjectOfType<PlayerEventParamsManager>();
        
        PlayerPrefs.SetInt("LastStagePlayed", currentStage);
        PlayerPrefs.SetInt("LastLevelPlayed", currentLvl);

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
            if (victoryPositionTrigger != null)
            {
                victoryPositionTrigger.ActivateCollider();
                playerMovementPath.NewTarget(victoryPositionTrigger.transform.position);
                playerMovementKeys.InterruptMovement(0.3f);
                menusManager.SetIsPause(true);
                yield return new WaitForSeconds(waitSeconds);
            } else {
                playerMovementPath.InterruptMovement();
                playerMovementKeys.InterruptMovement(0.3f);
                playerAnimationManager.PlayTriumph();
                menusManager.SetIsPause(true);
                yield return new WaitForSeconds(3);
            }

            if (PlayerPrefs.GetInt("Lvl" + currentLvl, 0) == 0)
                PlayerPrefs.SetInt("LastStageCompleted", 0);

            PlayerPrefs.SetInt("Lvl" + currentLvl, 1);

            menusManager.LevelCleared();
        } else
        {
            playerMovementPath.InterruptMovement();
            playerMovementKeys.InterruptMovement(0.3f);
            playerAnimationManager.PlayTriumph();
            menusManager.SetIsPause(true);
            yield return new WaitForSeconds(3);
            
            if (PlayerPrefs.GetInt("Lvl" + currentLvl, 0) == 0 && PlayerPrefs.GetInt("LastStageCompleted", 0) < currentStage)
                PlayerPrefs.SetInt("LastStageCompleted", currentStage);

            menusManager.StageCleared();
        }
    }

    public void GameOver(String deadCode)
    {
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
        Time.timeScale = 1f;
        MenusManager.isPaused = false;
        string nextSceneName = "Stage" + currentLvl + "-" + currentStage;
        SceneManager.LoadScene(nextSceneName);
    }

    public void RetryStage()
    {
        Time.timeScale = 1f;
        MenusManager.isPaused = false;
        string nextSceneName = "Stage" + currentLvl + "-" + currentStage;
        SceneManager.LoadScene(nextSceneName);
    }

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
