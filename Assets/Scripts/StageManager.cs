using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services.Analytics;
using Cinemachine;
using Newtonsoft.Json.Linq;

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

    bool challengeDisabled = false;
    ChallengeScript challenge;
    ChallengeResults challengeResults;
    ChallengeResults challengeRecord;
    ChallengeWinInfo cwi;
    int challengeExp;

    int stageSaveIdx;

    void Awake()
    {

        playerMovementPath = FindObjectOfType<PlayerMovementPath>();
        playerMovementKeys = FindObjectOfType<PlayerMovementKeys>();
        cameraAnimationManager = FindObjectOfType<CameraAnimationManager>();
        eagleEyeMode = FindObjectOfType<EagleEyeMode>();
        victoryPositionTrigger = FindObjectOfType<VictoryPositionTrigger>();
        playerAnimationManager = FindObjectOfType<PlayerAnimationManager>();
       
        challenge = GetComponent<ChallengeScript>();
        challengeRecord = new ChallengeResults();
        cwi = new ChallengeWinInfo();

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
        
        stageSaveIdx = (currentLvl - 1) * 4 + currentStage;

        challengeDisabled = (challenge == null) || PlayerPrefs.GetInt("ChallengeDisabled", 0) == 1;
        if (challengeDisabled)
        {
            challenge = null;
            menusManager.DisableChallengeInfo();
        } else
        {
            SaveData saveData = SaveManager.Load();
            if (saveData.StageChallengeRecords != null)
            {
                challengeRecord.value = saveData.StageChallengeRecords[stageSaveIdx];
                challengeRecord.win = saveData.StageCompleteStatus[stageSaveIdx] == 2; 
            }
            if (challengeRecord.win)
                challenge.UpdateWinCondition(2);

            // se é già vinta, posso evitare la lettura exp e chiamo il gestore medaglia per cambiarne la grafica. comunque tengo la challenge attiva
            
            TextAsset jsonAsset = Resources.Load<TextAsset>("challengeInfo");
            JObject jroot = JObject.Parse(jsonAsset.text);
            JToken jt = jroot["Lvl"];
            jt = jt[currentLvl + ""];
            jt = jt["Stage"];
            jt = jt[currentStage + ""];
            JToken jtExp = jt["exp"]; // check if there is?
            if (jtExp is JValue value)
                challengeExp = (int)value;
            JToken jtLim = jt["limit"]; // check if there is?
            if (jtLim is JValue value2)
                challengeRecord.limit = (int)value2;
            menusManager.UpdateChallengeInfo(challenge.challengeTitleKey, challenge.challengeTextKey, challenge.challengeLimitKey, challenge.challengeMedalKey, challengeRecord);
        }
       
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
    
    public bool WinGame(bool waterTiles = false, float waitSeconds = 6f)
    {
        PlayerPrefs.SetInt("ConsecutiveDeaths", 0);
        if (MenusManager.isPaused)
            return false;
        if (challenge != null)
            challengeResults = challenge.GetResultNow(true);
        StartCoroutine(WinningScene(waterTiles, waitSeconds));
        return true;
    }

    IEnumerator WinningScene(bool waterTiles, float waitSeconds)
    {
        eagleEyeMode.Exit();
        yield return new WaitForSeconds(0.5f);
        if (gameOver)
            yield break;
        FindFirstObjectByType<PlaygroundManager>().MakeRain(true, waterTiles);
        FindFirstObjectByType<PlaygroundManager>().EstinguishAllFlames();
        FindFirstObjectByType<PlaygroundManager>().HideBurntCellHighlighter();
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
            saveData.StageCompleteStatus[stageSaveIdx] = 1;
            if (isBoss)
                for (int i = 1; i <= 4; i++)
                    saveData.StageCompleteStatus[(currentLvl - 1) * 4 + i] = 1;
            SaveManager.Save(saveData);

            EvaluateAndSaveChallengeInfo();

            menusManager.LevelCleared(cwi, challengeResults, challenge);
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
            saveData.StageCompleteStatus[stageSaveIdx] = 1;
            SaveManager.Save(saveData);

            EvaluateAndSaveChallengeInfo();

            menusManager.StageCleared(cwi, challengeResults, challenge);
        }
    }

    private void EvaluateAndSaveChallengeInfo()
    {
        if (challengeDisabled)
            return;

        cwi = challenge.EvaluateWinInfo(challengeResults, challengeRecord);

        SaveData saveData = SaveManager.Load();
        if (cwi.newRec && saveData.StageChallengeRecords != null)
            saveData.StageChallengeRecords[stageSaveIdx] = cwi.recordValue;
        if (cwi.chalAlrWon || cwi.chalWinNow)
            saveData.StageCompleteStatus[stageSaveIdx] = 2;
        SaveManager.Save(saveData);
        if (!cwi.chalAlrWon && cwi.chalWinNow)
            cwi.chalWonExp = challengeExp;
        
        if (cwi.newRec)
            cwi.extraExp = ExpReader.GetNewRecordExtraExp(challengeResults.logic, challengeResults.limit, challengeRecord.value, cwi);

        PlayerPrefs.SetInt("CoinAmount", PlayerPrefs.GetInt("CoinAmount", 0) + cwi.chalWonExp + cwi.extraExp);
        PlayerPrefs.SetInt("TotalScore", PlayerPrefs.GetInt("TotalScore", 0) + cwi.chalWonExp + cwi.extraExp);
    }

    public void GameOver(String deadCode)
    {
        PlayerPrefs.SetInt("ConsecutiveDeaths", PlayerPrefs.GetInt("ConsecutiveDeaths", 0) + 1);
        if (!MenusManager.isPaused)
        {
            gameOver = true;
            StartCoroutine(EvaporatingScene(deadCode));
        }
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
        {
            menusManager.SetIsPause(true);
            StartCoroutine(RebornEvaporatingScene());
        }
    }

    IEnumerator RebornEvaporatingScene()
    {
        eagleEyeMode.Exit();
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
        //MenusManager.isPaused = false;
        string nextSceneName = "Stage" + currentLvl + "-" + currentStage;

        FindObjectOfType<AudioManager>().ResetSounds();
        SceneManager.LoadScene(nextSceneName);
    }

    public void RetryStage()
    {
        Time.timeScale = 1f;
        //MenusManager.isPaused = false;
        string nextSceneName = "Stage" + currentLvl + "-" + currentStage;
        
        FindObjectOfType<AudioManager>().ResetSounds();
        SceneManager.LoadScene(nextSceneName);
    }

    public void GoNextStage()
    {
        Time.timeScale = 1f;
        //MenusManager.isPaused = false;
        string nextSceneName = "Stage" + currentLvl + "-" + (currentStage + 1);
        
        FindObjectOfType<AudioManager>().ResetSounds();
        SceneManager.LoadScene(nextSceneName);
    }

    public void GoWorldMap()
    {
        Time.timeScale = 1f;
        //MenusManager.isPaused = false;
        
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
        //MenusManager.isPaused = false;
        
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
