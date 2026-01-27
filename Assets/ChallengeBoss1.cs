using UnityEngine;
using Newtonsoft.Json.Linq;

public class ChallengeBoss1 : ChallengeScript
{
    public Transform flameParent;
    int type = 0;
    int flameLimit = 0;
    int currentNumFlames = 0;
    bool stopChecking = false;

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
        JToken jtLimitVal = jt["limit"];
        if (jtLimitVal is JValue limitValue)
            flameLimit = (int)limitValue;
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

        currentState = -1;
        
        challengeInfo.SetMedalGFX(challengeMedalKey);
        challengeInfo.WriteText("no flames");
    }

    void Update()
    {
        if (stopChecking)
            return;
        CheckCondition();  
    }

    public void CheckCondition()
    {
        if (stopChecking)
            return;
        currentNumFlames = 0;
        foreach (Transform child in flameParent)
        {
            if (child.GetComponent<PickFlame>() != null)
                currentNumFlames++;
        }
        challengeInfo.WriteText(currentNumFlames + "/" + flameLimit + " flames");
    }

    public override ChallengeResults GetResultNow(bool stop = false)
    {
        CheckCondition();
        stopChecking = stop;
        return new ChallengeResults(currentNumFlames <= flameLimit, flameLimit, currentNumFlames, challengeLogic);
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
                    if (challengeResults.value < challengeRecord.value)
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
                }
            }
        }
        return cwi;
    }
}
