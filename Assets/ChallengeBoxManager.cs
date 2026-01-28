using System;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeBoxManager : MonoBehaviour
{
    public Image medalGFX;
    public Text text;

    public TextLocalizer title;
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
            
            string noChallengeText =  SingletonLocalizationManager.instance.GetComponent<LocalizationManager>().Get("content.challenge.disabled.title");
            if (noChallengeText == null)
                noChallengeText = "Challenge Disabled";
            text.text = noChallengeText;
            return;
        }

        SetMedalGFX(challenge.challengeMedalKey);
        SetMedalState(0);

        string textToDisplay = "";
        textToDisplay = textToDisplay + cr.value + "/" +  cr.limit + " ";
        if (cwi.chalWinNow)
        {
            string localizedText =  SingletonLocalizationManager.instance.GetComponent<LocalizationManager>().Get("content.challenge.general.challenge_won");
            if (localizedText == null)
                localizedText = "Challenge Complete";
            textToDisplay = textToDisplay + localizedText + "\n";
        } else
        {
            string localizedText =  SingletonLocalizationManager.instance.GetComponent<LocalizationManager>().Get("content.challenge.general.challenge_missed");
            if (localizedText == null)
                localizedText = "Challenge Missed";
            textToDisplay = textToDisplay + localizedText + "\n";
        }

        string recordText;
        
        if (!cwi.chalAlrWon && !cwi.chalWinNow)
        {
            if (cwi.newRec)
            {
                textToDisplay = textToDisplay + cwi.recordValue + "/" + cr.limit + " ";
                recordText =  SingletonLocalizationManager.instance.GetComponent<LocalizationManager>().Get("content.challenge.general.new_record");
                if (recordText == null)
                    recordText = "New Record";
                textToDisplay = textToDisplay + recordText + "!<color=#E2C72A>";
                if (cwi.chalWonExp > 0)
                    textToDisplay = textToDisplay + " +" + cwi.chalWonExp;
                if (cwi.extraExp > 0)
                    textToDisplay = textToDisplay + " +" + cwi.extraExp;
                textToDisplay = textToDisplay + "exp</color>";
            } else if (cwi.recordValue > 0)
            {
                recordText =  SingletonLocalizationManager.instance.GetComponent<LocalizationManager>().Get("content.challenge.general.old_record");
                if (recordText == null)
                    recordText = "Old Record";
                textToDisplay = textToDisplay + cwi.recordValue + "/" + cr.limit + " ";
                textToDisplay = textToDisplay + recordText;
            } else
            {
                recordText =  SingletonLocalizationManager.instance.GetComponent<LocalizationManager>().Get("content.challenge.general.no_record");
                if (recordText == null)
                    recordText = "No Record";
                textToDisplay = textToDisplay + "--/-- " + recordText;
            }
            
        } else
        {
            textToDisplay = textToDisplay + cwi.recordValue + "/" + cr.limit + " ";
            if (cwi.newRec)
            {
                recordText =  SingletonLocalizationManager.instance.GetComponent<LocalizationManager>().Get("content.challenge.general.new_record");
                if (recordText == null)
                    recordText = "New Record";
                textToDisplay = textToDisplay + recordText + "!<color=#E2C72A>";
                if (cwi.chalWonExp > 0)
                    textToDisplay = textToDisplay + " +" + cwi.chalWonExp;
                if (cwi.extraExp > 0)
                    textToDisplay = textToDisplay + " +" + cwi.extraExp;
                textToDisplay = textToDisplay + "exp</color>";
            } else
            {
                recordText =  SingletonLocalizationManager.instance.GetComponent<LocalizationManager>().Get("content.challenge.general.old_record");
                if (recordText == null)
                    recordText = "Old Record";
                textToDisplay = textToDisplay + recordText;
            }
            SetMedalState(2);
        }
        text.text = textToDisplay;
    }

    public void DisplayMenuInfoMessage(String titleKey, String descriptionKey, string limitKey, string medalKey, ChallengeResults cRecord)
    {
        title.key = titleKey;
        title.Localize();

        string textToDisplay = "";
        string descriptionText =  SingletonLocalizationManager.instance.GetComponent<LocalizationManager>().Get(descriptionKey);
        if (descriptionText == null)
            descriptionText = descriptionKey;
        textToDisplay = textToDisplay + descriptionText; 
        if (cRecord.limit > 0)
        {
            textToDisplay = textToDisplay + "\n";
            string limitText =  SingletonLocalizationManager.instance.GetComponent<LocalizationManager>().Get(limitKey);
            if (limitText == null)
                limitText = limitKey;
            textToDisplay = textToDisplay + limitText + ": ";
            textToDisplay = textToDisplay + cRecord.limit;
        }
        if (cRecord.value > 0)
        {
            textToDisplay = textToDisplay + "\n";
            string oldRecordText =  SingletonLocalizationManager.instance.GetComponent<LocalizationManager>().Get("content.challenge.general.old_record");
            if (oldRecordText == null)
                oldRecordText = "Old record";
            textToDisplay = textToDisplay + oldRecordText + ": ";
            textToDisplay = textToDisplay + cRecord.value;
        }
        if (cRecord.value == -1)
        {
            textToDisplay = textToDisplay + "\n";
            string noRecordText =  SingletonLocalizationManager.instance.GetComponent<LocalizationManager>().Get("content.challenge.general.no_record");
            if (noRecordText == null)
                noRecordText = "No record";
            textToDisplay = textToDisplay + noRecordText;
        }
        description.text = textToDisplay;
        /*FitBoxText fitBoxText = description.GetComponent<FitBoxText>();
        if (fitBoxText != null)
            fitBoxText.Resize();
        */

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
