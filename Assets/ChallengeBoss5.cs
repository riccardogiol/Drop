using UnityEngine;
using Newtonsoft.Json.Linq;

public class ChallengeBoss5 : ChallengeScript
{
    int type = 0;

    bool winCondition = false;

    void Awake()
    {
        challengeInfo = FindFirstObjectByType<ChallengeInfo>();
        stageManager = GetComponent<StageManager>();

        //take values for the stage challenge logic
        TextAsset jsonAsset = Resources.Load<TextAsset>("challengeInfo");
        JObject jroot = JObject.Parse(jsonAsset.text);
        JToken jt = jroot["Lvl"];
        jt = jt[stageManager.currentLvl + ""];
        jt = jt["Stage"];
        jt = jt[stageManager.currentStage + ""];
        JToken jtTypeVal = jt["type"];
        if (jtTypeVal is JValue typeValue)
            type = (int)typeValue;
        //take values for the info on the challenge type
        jt = jroot["type"];
        jt = jt[type + ""];
        JToken jtTitle = jt["title"];
        if (jtTitle is JValue value3)
            challengeTitleKey = (string)value3;
        JToken jtDescription = jt["description"];
        if (jtDescription is JValue value4)
            challengeTextKey = (string)value4;
        JToken jtLimit = jt["limit"];
        if (jtLimit is JValue value5)
            challengeLimitKey = (string)value5;
        JToken jtMedal = jt["medal_code"];
        if (jtMedal is JValue value6)
            challengeMedalKey = (string)value6;
        JToken jtLogic = jt["logic"];
        if (jtLogic is JValue value7)
            challengeLogic = (string)value7;
        
        challengeInfo.SetMedalGFX(challengeMedalKey);

        if (PlayerPrefs.GetInt("Hero1Purchased", 0) == 0 && 
            PlayerPrefs.GetInt("Waterbullet1Purchased", 0) == 0 && 
            PlayerPrefs.GetInt("Wave1Purchased", 0) == 0 && 
            PlayerPrefs.GetInt("SuperPurchased", 0) == 0)
            winCondition = true;
        
        if (winCondition)
        {
            challengeInfo.SetMedalState(1);
            challengeInfo.WriteText("condition fulfilled");
        } else
        {
            challengeInfo.SetMedalState(0);
            challengeInfo.WriteText("condition wrong");
        }
    }

    public override ChallengeResults GetResultNow(bool stop = false)
    {
        return new ChallengeResults(winCondition, 1, 1, challengeLogic);
    }

    public override ChallengeWinInfo EvaluateWinInfo(ChallengeResults challengeResults, ChallengeResults challengeRecord)
    {
        ChallengeWinInfo cwi = new ChallengeWinInfo();
        if (challengeResults != null)
        {
            if (challengeRecord.win)
            {
                cwi.chalAlrWon = true;
                cwi.recordValue = challengeRecord.value;
                if (challengeResults.win)
                    cwi.chalWinNow = true;

            } else
            {
                if (challengeResults.win)
                {
                    cwi.chalWinNow = true;
                    cwi.newRec = true;
                    cwi.recordValue = challengeResults.value;
                }
            }
        }
        return cwi;
    }
}
