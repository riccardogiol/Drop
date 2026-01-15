using UnityEngine;
using Newtonsoft.Json.Linq;

public class ChallengeTime : ChallengeScript
{
    int timeLimit = 0;
    float timerSinceStart = 0;
    bool stopTimer = false;

    ChallengeInfo challengeInfo;
    StageManager stageManager;

    void Awake()
    {
        challengeTitleKey = "Time Challenge";
        challengeTextKey = "Clean the level before the time limit.";
        challengeInfo = FindFirstObjectByType<ChallengeInfo>();
        stageManager = GetComponent<StageManager>();
        TextAsset jsonAsset = Resources.Load<TextAsset>("challengeInfo");
        JObject jroot = JObject.Parse(jsonAsset.text);
        JToken jt = jroot["Lvl"];
        jt = jt[stageManager.currentLvl + ""];
        jt = jt["Stage"];
        jt = jt[stageManager.currentStage + ""];
        jt = jt["limit"]; // check if there is?
        if (jt is JValue value)
            timeLimit = (int)value;
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
        return new ChallengeResults(timerSinceStart <= timeLimit, timeLimit, (int)timerSinceStart, "lessThen");
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
