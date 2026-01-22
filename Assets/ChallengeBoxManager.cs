using System;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeBoxManager : MonoBehaviour
{
    public Image medalGFX;
    public Text text;

    public Text title;
    public Text description;
    public GameObject descriptionPanel;
    bool showPanel = false;

    public Color disabledColor;
    public Color potentialColor;
    public Color wonColor;

    public void DisplayEndStageMessage(ChallengeResults cr, ChallengeWinInfo cwi, ChallengeScript challenge)
    {
        if (challenge == null)
        {
            string noChallengeText = "Challenge Disabled"; // rimpiazza con localization stesso di challenge info file in type 0
            text.text = noChallengeText;
            return;
        }

        SetMedalGFX(challenge.challengeMedalKey);
        SetMedalState(0);

        string textToDisplay = "";
        textToDisplay = textToDisplay + cr.value + "/" +  cr.limit + " ";
        if (cwi.chalWinNow)
        {
            textToDisplay = textToDisplay + "Challenge Complete\n"; // rimpiazzare con localization
        } else
        {
            textToDisplay = textToDisplay + "Challenge Missed\n"; // rimpiazzare con localization
        }
        
        if (!cwi.chalAlrWon && !cwi.chalWinNow)
        {
            if (cwi.newRec)
            {
                textToDisplay = textToDisplay + cwi.recordValue + "/" + cr.limit + " "; // rimpiazzare con localization
                textToDisplay = textToDisplay + "New Record!<color=#E2C72A>"; // rimpiazzare con localization
                if (cwi.chalWonExp > 0)
                    textToDisplay = textToDisplay + " +" + cwi.chalWonExp;
                if (cwi.extraExp > 0)
                    textToDisplay = textToDisplay + " +" + cwi.extraExp;
                textToDisplay = textToDisplay + "exp</color>";
            } else if (cwi.recordValue > 0)
            {
                textToDisplay = textToDisplay + cwi.recordValue + "/" + cr.limit + " "; // rimpiazzare con localization
                textToDisplay = textToDisplay + "Old Record"; // rimpiazzare con localization
            } else
                textToDisplay = textToDisplay + "--/-- " + "No Record"; // rimpiazzare con localization
            
        } else
        {
            textToDisplay = textToDisplay + cwi.recordValue + "/" + cr.limit + " "; // rimpiazzare con localization
            if (cwi.newRec)
            {
                textToDisplay = textToDisplay + "New Record!<color=#E2C72A>"; // rimpiazzare con localization
                if (cwi.chalWonExp > 0)
                    textToDisplay = textToDisplay + " +" + cwi.chalWonExp;
                if (cwi.extraExp > 0)
                    textToDisplay = textToDisplay + " +" + cwi.extraExp;
                textToDisplay = textToDisplay + "exp</color>";
            } else
            {
                textToDisplay = textToDisplay + "Old Record"; // rimpiazzare con localization
            }
            SetMedalState(2);
        }
        text.text = textToDisplay;
    }

    public void DisplayMenuInfoMessage(String titleKey, String descriptionKey, string limitKey, string medalKey, ChallengeResults cRecord)
    {
        title.text = titleKey; // sostituire con localization

        string textToDisplay = "";
        textToDisplay = textToDisplay + descriptionKey; // sostituire con localization
        if (cRecord.limit > 0)
        {
            textToDisplay = textToDisplay + "\n";
            textToDisplay = textToDisplay + limitKey + ": "; // sostituire con localization
            textToDisplay = textToDisplay + cRecord.limit;
        }
        if (cRecord.value > 0)
        {
            textToDisplay = textToDisplay + "\n";
            textToDisplay = textToDisplay + "Old record: "; // sostituire con localization
            textToDisplay = textToDisplay + cRecord.value;
        }
        if (cRecord.value == -1)
        {
            textToDisplay = textToDisplay + "\n";
            textToDisplay = textToDisplay + "Old record: --"; // sostituire con localization
        }
        description.text = textToDisplay;

        SetMedalGFX(medalKey);
        if (cRecord.win)
            SetMedalState(2);
        else
            SetMedalState(0);

        showPanel = false;
        descriptionPanel.SetActive(showPanel);
    }

    public void ToggleDescriptionPanel()
    {
        showPanel = !showPanel;
        descriptionPanel.SetActive(showPanel);
    }

    public void SetMedalState(int state)
    {
        switch (state)
        {
            case 2:
                medalGFX.color = wonColor;
                break;
            case 1:
                medalGFX.color = potentialColor;
                break;
            default:
                medalGFX.color = disabledColor;
                break;
        }
    }

    void SetMedalGFX(string medalCode)
    {
        Sprite medalSprite = Resources.Load<Sprite>("Sprites/Elements/" + medalCode);
        if (medalSprite != null)
            medalGFX.sprite = medalSprite;
    }

    public void DisableMenuInfo()
    {
        gameObject.SetActive(false);
    }
}
