using System.Collections;
using UnityEngine;

public class BossEventManager : MonoBehaviour
{
    public MenusManager menusManager;
    public GameObject messageToShow;
    bool messageShown = false;

    public void ShowMessageOnce()
    {
        if (messageShown)
            return;
        messageShown = true;
        StartCoroutine(DelayShow());
    }

    IEnumerator DelayShow()
    {
        yield return new WaitForSeconds(2);
        menusManager.ShowMessage(messageToShow);
    }
}
