using Newtonsoft.Json.Linq;
using UnityEngine;

public class ChallengeEnvElements : ChallengeScript //Boss3?
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

        currentState = -1;
        //challengeInfo.SetMedalState(1);
        
        challengeInfo.SetMedalGFX(challengeMedalKey);
        IncreaseCounter(0);
    }

    public void IncreaseCounter(int amount = 1)
    {
        if (stopCounter)
            return;
        counter += amount;
        challengeInfo.WriteText(counter + "/" + objective + " kills");
    }


    public override ChallengeResults GetResultNow(bool stop = false)
    {
        stopCounter = stop;
        return new ChallengeResults(counter >= objective, objective, counter, "greaterThanZero");
    }

    public override ChallengeWinInfo EvaluateWinInfo(ChallengeResults challengeResults, ChallengeResults challengeRecord)
    {
        ChallengeWinInfo cwi = new ChallengeWinInfo();
        if (challengeResults != null)
        {
            if (challengeRecord.win)
            {
                cwi.chalAlrWon = true;
                if (challengeResults.win)
                {
                    cwi.chalWinNow = true;
                    if (challengeResults.value > challengeRecord.value)
                    {
                        cwi.newRec = true;
                        cwi.recordValue = challengeResults.value;
                    } else
                        cwi.recordValue = challengeRecord.value;
                } else
                    cwi.recordValue = challengeRecord.value;
            } else
            {
                if (challengeResults.win)
                {
                    cwi.chalWinNow = true;
                    cwi.newRec = true;
                    cwi.recordValue = challengeResults.value;
                } else
                {
                    if (challengeResults.value > 0 && challengeResults.value > challengeRecord.value)
                    {
                        cwi.newRec = true;
                        cwi.recordValue = challengeResults.value;
                    } else if (challengeRecord.value > 0)
                        cwi.recordValue = challengeRecord.value;
                }
            }
        }
        return cwi;
    }
}
