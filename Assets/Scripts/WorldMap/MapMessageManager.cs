using UnityEngine;

public class MapMessageManager : MonoBehaviour
{
    public GameObject shader;
    public GameObject storeMessage;
    public bool messageOnScreen = false;

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

    public void ExitMessage(GameObject message)
    {
        shader.SetActive(false);
        message.SetActive(false);
        messageOnScreen = false;
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
        messageOnScreen = false;
    }

}
