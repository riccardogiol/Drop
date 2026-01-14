using UnityEngine;
using UnityEngine.UI;

public class ChallengeBoxManager : MonoBehaviour
{
    public Image medalImage;
    public Text text;

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
}
