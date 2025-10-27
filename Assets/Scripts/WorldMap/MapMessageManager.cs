using System.Collections;
using UnityEngine;

public class MapMessageManager : MonoBehaviour
{
    public GameObject shader;
    public GameObject storeMessage;
    public StagePanelManager stagePanelManager;
    public GameObject CollectionOpening;
    public static bool messageOnScreen = false;

    void Awake()
    {
        messageOnScreen = false;
    }

    void Start()
    {
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
    }
    
    public void ShowStagePanel(int lvlCode, int stageCode = 1)
    {
        if (messageOnScreen)
            return;
        stagePanelManager.UpdateInfo(lvlCode, stageCode);
        stagePanelManager.gameObject.SetActive(true);
        shader.SetActive(true);
        messageOnScreen = true;
    }

    public void ShowCollectionPanel()
    {
        if (messageOnScreen)
            return;
        CollectionOpening.SetActive(true);
        shader.SetActive(true);
        messageOnScreen = true;
    }

    public void ExitMessage(GameObject message)
    {
        shader.SetActive(false);
        message.SetActive(false);
        StartCoroutine(delayMessageOnScreenExit());
    }

    public void OpenStore()
    {
        shader.SetActive(true);
        storeMessage.SetActive(true);
        messageOnScreen = true;
    }

    public void CloseStore()
    {
        shader.SetActive(false);
        storeMessage.SetActive(false);
        StartCoroutine(delayMessageOnScreenExit());
    }

    IEnumerator delayMessageOnScreenExit()
    {
        yield return null;
        messageOnScreen = false;
    }

}
