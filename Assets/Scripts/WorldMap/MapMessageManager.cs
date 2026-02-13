using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class MapMessageManager : MonoBehaviour
{
    public GameObject shader;
    public GameObject storeMessage;
    public StagePanelManager stagePanelManager;
    public GameObject CollectionOpening;
    public GameObject difficultySelectionMessage;
    public GameObject chooseIDMessage;
    public static bool messageOnScreen = false;

    void Awake()
    {
        messageOnScreen = false;
        MenusManager.isPaused = false;
    }

    void Start()
    {
        messageOnScreen = false;
        MenusManager.isPaused = false;
        if (PlayerPrefs.GetInt("ResetExpFlag", 0) == 1)
            StartCoroutine(StoreOpenAndReset());
    }

    IEnumerator StoreOpenAndReset()
    {
        OpenStore();
        yield return null;
        yield return null;
        storeMessage.transform.Find("RefundButton").GetComponent<StoreRefund>().RefundAll();
        if (PlayerPrefs.GetInt("LastLevelCompleted", 0) < 5)
            CloseStore();
        PlayerPrefs.SetInt("ResetExpFlag", 0);
    }

    void Update()
    {
        if (messageOnScreen)
           return;
        if (Gamepad.current != null)
        {
            if (Gamepad.current.buttonEast.wasPressedThisFrame && PlayerPrefs.GetInt("Lvl5", 0) == 1)
            {
                OpenStore();
                return;
            }
            if (Gamepad.current.buttonNorth.wasPressedThisFrame)
            {
                ShowCollectionPanel();
                return;
            }
            if (Gamepad.current.buttonWest.wasPressedThisFrame)
            {
                ShowMessage(difficultySelectionMessage);
                return;
            }
            if (Gamepad.current.selectButton.wasPressedThisFrame)
            {
                ShowMessage(chooseIDMessage);
                return;
            }
        }
    }

    public void ShowLevelMessage(int lvlCode, int stageCode = 1)
    {   
        if (messageOnScreen)
        {
            return;
        }
        Transform auxTrans = transform.Find("LevelPresentationMessage" + lvlCode);
        if (auxTrans == null)
            return;
        auxTrans.GetComponent<LevelMessageManager>().SetMessage(stageCode);
        auxTrans.gameObject.SetActive(true);
        shader.SetActive(true);
        messageOnScreen = true;
        FindObjectOfType<AudioManager>().LowFilerEnter();
    }
    
    public void ShowStagePanel(int lvlCode, int stageCode = 1)
    {
        if (messageOnScreen)
            return;
        stagePanelManager.UpdateInfo(lvlCode, stageCode);
        stagePanelManager.gameObject.SetActive(true);
        shader.SetActive(true);
        messageOnScreen = true;
        FindObjectOfType<AudioManager>().LowFilerEnter();
    }

    public void ShowCollectionPanel()
    {
        if (messageOnScreen)
            return;
        CollectionOpening.SetActive(true);
        shader.SetActive(true);
        messageOnScreen = true;
        FindObjectOfType<AudioManager>().LowFilerEnter();
    }

    public void ShowMessage(GameObject message)
    {
        shader.SetActive(true);
        message.SetActive(true);
        messageOnScreen = true;
        FindObjectOfType<AudioManager>().LowFilerEnter();
    }

    public void ExitMessage(GameObject message)
    {
        shader.SetActive(false);
        message.SetActive(false);
        FindObjectOfType<AudioManager>().LowFilerExit();
        StartCoroutine(delayMessageOnScreenExit());
    }

    public void OpenStore()
    {
        shader.SetActive(true);
        storeMessage.SetActive(true);
        messageOnScreen = true;
        FindObjectOfType<AudioManager>().LowFilerEnter();
    }

    public void CloseStore()
    {
        shader.SetActive(false);
        storeMessage.SetActive(false);
        FindObjectOfType<AudioManager>().LowFilerExit();
        StartCoroutine(delayMessageOnScreenExit());
    }

    IEnumerator delayMessageOnScreenExit()
    {
        yield return null;
        messageOnScreen = false;
    }

    public void StartEndingScene()
    {
        messageOnScreen = true;
        gameObject.SetActive(false);
    }

}
