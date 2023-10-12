using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public int currentLvl = 1;
    public int currentStage = 1;

    public bool finalStage = false;

    public MenusManager menusManager;

    public Animator cameraAnimator;
    public Animator playerAnimator;
    public ParticleSystem rainEffect;
    PlayerMovementPath playerMovementPath;
    DecorationManager decorationManager;

    void Start()
    {
        FindObjectOfType<AudioManager>().Stop("OpeningMusic");
        FindObjectOfType<AudioManager>().Play("BackgroundMusic");
        playerMovementPath = FindObjectOfType<PlayerMovementPath>();
        decorationManager = FindObjectOfType<DecorationManager>();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            WinGame();
        }
    }
    
    public void WinGame()
    {
        StartCoroutine(WinningScene());
    }

    IEnumerator WinningScene()
    {
        menusManager.SetIsPause(true);
        if (playerMovementPath != null)
            playerMovementPath.InterruptMovement(); // actually goes to a specific point
        if (decorationManager != null)
            decorationManager.SetGreenSprites();
        MakeRain(true);
        cameraAnimator.SetTrigger("ZoomIn");
        playerAnimator.SetTrigger("Triumph");

        yield return new WaitForSeconds(3);

        if (finalStage)
        {
            PlayerPrefs.SetInt("Lvl" + currentLvl, 1);
            menusManager.LevelCleared();
        } else {
            menusManager.StageCleared();
        }
    }

    public void MakeRain(bool isRaining)
    {
        if (isRaining)
            rainEffect.Play();
        else
            rainEffect.Stop();
    }

    public void GameOver()
    {
         menusManager.GameOver();
    }

    //Close stage functions

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
}
