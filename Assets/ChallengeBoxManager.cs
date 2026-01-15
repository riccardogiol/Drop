using System;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeBoxManager : MonoBehaviour
{
    public Image medalImage;
    public Text text;

    public Text title;
    public Text description;
    public GameObject descriptionPanel;
    bool showPanel = false;

    public void DisplayEndStageMessage(ChallengeResults cr, ChallengeWinInfo cwi)
    {
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
            textToDisplay = textToDisplay + "--/-- " + "No Record"; // rimpiazzare con localization
        } else
        {
            textToDisplay = textToDisplay + cwi.recordValue + "/" + cr.limit + " "; // rimpiazzare con localization
            if (cwi.newRec)
            {
                textToDisplay = textToDisplay + "New Record!"; // rimpiazzare con localization
                if (cwi.chalWonExp > 0)
                    textToDisplay = textToDisplay + " +" + cwi.chalWonExp;
                if (cwi.extraExp > 0)
                    textToDisplay = textToDisplay + " +" + cwi.extraExp;
            } else
            {
                textToDisplay = textToDisplay + "Old Record"; // rimpiazzare con localization
            }
        }
        text.text = textToDisplay;
    }

    public void DisplayMenuInfoMessage(String titleKey, String descriptionKey, ChallengeResults cRecord)
    {
        title.text = titleKey; // sostituire con localization

        string textToDisplay = "";
        textToDisplay = textToDisplay + descriptionKey; // sostituire con localization
        if (cRecord.limit > 0)
        {
            textToDisplay = textToDisplay + "\n";
            textToDisplay = textToDisplay + "Limit: "; // sostituire con localization
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

        showPanel = false;
        descriptionPanel.SetActive(showPanel);
    }

    public void ToggleDescriptionPanel()
    {
        showPanel = !showPanel;
        descriptionPanel.SetActive(showPanel);
    }
}
