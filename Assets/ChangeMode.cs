using UnityEngine;

public class ChangeMode : MonoBehaviour
{
    public SwapTextEasyMode swapTextEasyMode;

    public void SetEasyMode()
    {
        PlayerPrefs.SetInt("ConsecutiveDeaths", 0);
        PlayerPrefs.SetInt("ConsecutiveDeathsLimit", 3);
        PlayerPrefs.SetInt("EasyMode", 1);
        swapTextEasyMode.UpdateText();
        gameObject.SetActive(false);
    }

    public void SetNormalMode()
    {
        PlayerPrefs.SetInt("EasyMode", 0);
        swapTextEasyMode.UpdateText();
        gameObject.SetActive(false);
    }
}
