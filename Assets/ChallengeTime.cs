using UnityEngine;
using Newtonsoft.Json.Linq;

public class ChallengeTime : ChallengeScript
{
    int timeLimit = 0;
    float timerSinceStart = 0;
    bool stopTimer = false;

    void Awake()
    {
        challengeInfo = FindFirstObjectByType<ChallengeInfo>();
        // if playerPrefs == challenges disabled, challengeInfo.disable
        stageManager = GetComponent<StageManager>();
        //take values for the stage challenge logic
        TextAsset jsonAsset = Resources.Load<TextAsset>("challengeInfo");
        JObject jroot = JObject.Parse(jsonAsset.text);
        JToken jt = jroot["Lvl"];
        jt = jt[stageManager.currentLvl + ""];
        jt = jt["Stage"];
        jt = jt[stageManager.currentStage + ""];
        jt = jt["limit"]; // check if there is?
        if (jt is JValue value)
            timeLimit = (int)value;
        //take values for the info on the challenge type
        jt = jroot["type"];
        jt = jt["1"];
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
        
        challengeInfo.SetMedalGFX(challengeMedalKey);
    }

    void Update()
    {
        if (stopTimer)
           return;
        timerSinceStart += Time.deltaTime;   
        challengeInfo.WriteText((int)timerSinceStart + "/" + timeLimit + " sec");
    }

    public override ChallengeResults GetResultNow(bool stop = false)
    {
        stopTimer = stop;
        return new ChallengeResults(timerSinceStart <= timeLimit, timeLimit, (int)timerSinceStart, "lessThan");
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
