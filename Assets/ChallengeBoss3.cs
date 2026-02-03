using Newtonsoft.Json.Linq;
using UnityEngine;

public class ChallengeBoss3 : ChallengeScript
{
    int objective = 0;
    int counter = 0;
    int type;

    bool stopCounter = false;

    void Awake()
    {
        challengeInfo = FindFirstObjectByType<ChallengeInfo>();
        stageManager = GetComponent<StageManager>();

        TextAsset jsonAsset = Resources.Load<TextAsset>("challengeInfo");
        JObject jroot = JObject.Parse(jsonAsset.text);
        JToken jt = jroot["Lvl"];
        jt = jt[stageManager.currentLvl + ""];
        jt = jt["Stage"];
        jt = jt[stageManager.currentStage + ""];
        JToken jtLimitVal = jt["limit"];
        if (jtLimitVal is JValue limitValue)
            objective = (int)limitValue;
        JToken jtTypeVal = jt["type"];
        if (jtTypeVal is JValue typeValue)
            type = (int)typeValue;
        
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

        currentState = -1;
        
        challengeInfo.SetMedalGFX(challengeMedalKey);
        IncreaseCounter(0);
    }

    public void IncreaseCounter(int amount = 1)
    {
        if (stopCounter)
            return;
        counter += amount;
        challengeInfo.WriteText(counter + "/" + objective + " kills");
        if (!recordChallengeWon)
        {
            if (counter < objective)
                challengeInfo.SetMedalState(0);
            else
                challengeInfo.SetMedalState(1);
        }

    }


    public override ChallengeResults GetResultNow(bool stop = false)
    {
        stopCounter = stop;
        return new ChallengeResults(counter >= objective, objective, counter, challengeLogic);
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
