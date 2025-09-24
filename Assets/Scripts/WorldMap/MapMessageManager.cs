using System.Collections;
using UnityEngine;

public class MapMessageManager : MonoBehaviour
{
    public GameObject shader;
    public GameObject storeMessage;
    public StagePanelManager stagePanelManager;
    public static bool messageOnScreen = false;

    void Awake()
    {
        messageOnScreen = false;
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
