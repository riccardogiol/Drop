using UnityEngine;

public class ChangeMode : MonoBehaviour
{
    public SwapTextEasyMode swapTextEasyMode;
    public SwapTextChallengeDisabled swapTextCD;
    public ButtonActivationOnPP toggleChallengeButton;
    public MapMessageManager mmm;

    public void SetEasyMode()
    {
        PlayerPrefs.SetInt("ConsecutiveDeaths", 0);
        PlayerPrefs.SetInt("ConsecutiveDeathsLimit", 3);
        PlayerPrefs.SetInt("EasyMode", 1);
        toggleChallengeButton.UpdateButtonInteraction();
        PlayerPrefs.SetInt("ChallengeDisabled", 1);
        swapTextEasyMode.UpdateText();
        swapTextCD.UpdateText();
        if (mmm != null)
            mmm.ExitMessage(gameObject);
        else
            gameObject.SetActive(false);

    }

    public void SetNormalMode()
    {
        PlayerPrefs.SetInt("EasyMode", 0);
        toggleChallengeButton.UpdateButtonInteraction();
        swapTextEasyMode.UpdateText();
        swapTextCD.UpdateText();
        if (mmm != null)
            mmm.ExitMessage(gameObject);
        else
            gameObject.SetActive(false);

    }

    public void ToggleChallengeActive()
    {
        int currentValue = PlayerPrefs.GetInt("ChallengeDisabled", 0);
        if (currentValue == 0)
            PlayerPrefs.SetInt("ChallengeDisabled", 1);
        else
            PlayerPrefs.SetInt("ChallengeDisabled", 0);
        swapTextCD.UpdateText();
    }
}
