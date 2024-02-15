using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public int currentLvl = 1;
    public int currentStage = 1;
    public string stageMode = "puzzle";

    public bool finalStage = false;
    VictoryPositionTrigger victoryPositionTrigger;

    public MenusManager menusManager;

    PlayerAnimationManager playerAnimationManager;

    CameraAnimationManager cameraAnimationManager;
    PlayerMovementPath playerMovementPath;
    PlayerMovementKeys playerMovementKeys;

    void Start()
    {
        FindObjectOfType<AudioManager>().Stop("OpeningMusic");
        FindObjectOfType<AudioManager>().Play("BackgroundMusic");
        playerMovementPath = FindObjectOfType<PlayerMovementPath>();
        playerMovementKeys = FindObjectOfType<PlayerMovementKeys>();
        cameraAnimationManager = FindObjectOfType<CameraAnimationManager>();
        victoryPositionTrigger = FindObjectOfType<VictoryPositionTrigger>();
        playerAnimationManager = FindObjectOfType<PlayerAnimationManager>();
    }
    
    public void WinGame()
    {
        if (!MenusManager.isPaused)
            StartCoroutine(WinningScene());
    }

    IEnumerator WinningScene()
    {
        FindFirstObjectByType<PlaygroundManager>().MakeRain(true);
        cameraAnimationManager.StartEndingAnimation();

        if (finalStage)
        {
            if (victoryPositionTrigger != null)
            {
                victoryPositionTrigger.ActivateCollider();
                playerMovementPath.NewTarget(victoryPositionTrigger.transform.position);
                playerMovementKeys.InterruptMovement(0.3f);
                menusManager.SetIsPause(true);
                yield return new WaitForSeconds(6);
            } else {
                playerMovementPath.InterruptMovement();
                playerMovementKeys.InterruptMovement(0.3f);
                playerAnimationManager.PlayTriumph();
                menusManager.SetIsPause(true);
                yield return new WaitForSeconds(3);
            }

            PlayerPrefs.SetInt("Lvl" + currentLvl, 1);
            PlayerPrefs.SetInt("LastStageCompleted", 0);

            menusManager.LevelCleared();
        } else
        {
            playerMovementPath.InterruptMovement();
            playerMovementKeys.InterruptMovement(0.3f);
            playerAnimationManager.PlayTriumph();
            menusManager.SetIsPause(true);
            yield return new WaitForSeconds(3);

            PlayerPrefs.SetInt("LastStageCompleted", currentStage);
            menusManager.StageCleared();
        }
    }

    public void GameOver(String deadCode)
    {
        if (!MenusManager.isPaused)
            StartCoroutine(EvaporatingScene(deadCode));
    }

    IEnumerator EvaporatingScene(String deadCode)
    {
        menusManager.SetIsPause(true);
        if (playerMovementPath != null)
            playerMovementPath.InterruptMovement();
        playerMovementKeys.InterruptMovement(0.3f);
        cameraAnimationManager.StartEndingAnimation();
        if (deadCode != "no_flower")
            playerAnimationManager.PlayEvaporation();

        yield return new WaitForSeconds(3);

        menusManager.GameOver(deadCode);
    }

    //Close stage functions

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

    public void ShowButtonDescription()
    {
        menusManager.ShowDescriptions();
    }

    public void HideButtonDescription()
    {
        menusManager.HideDescriptions();
    }
}
