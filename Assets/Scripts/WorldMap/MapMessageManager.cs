using UnityEngine;
using UnityEngine.UI;

public class MapMessageManager : MonoBehaviour
{
    public GameObject shader;

    public void ShowLevelMessage(int lvlCode, int stageCode = 1)
    {   
        Transform auxTrans = transform.Find("LevelPresentationMessage" + lvlCode);
        if (auxTrans == null)
            return;
        auxTrans.GetComponent<LevelMessageManager>().SetMessage(stageCode);
        auxTrans.gameObject.SetActive(true);
        shader.SetActive(true);
    }

    public void ExitMessage(GameObject message)
    {
        shader.SetActive(false);
        message.SetActive(false);
        //isPaused = false;
        //messageOnScreen = false;
    }
}
