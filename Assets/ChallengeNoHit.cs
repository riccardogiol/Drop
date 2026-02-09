using UnityEngine;
using Newtonsoft.Json.Linq;

public class ChallengeNoHit : ChallengeScript
{
    int hitsLimit = 0;
    int type = 0;
    int hitsSinceStart = 0;
    bool stopCounter = false;

    string hitsText;
    void Awake()
    {
        challengeInfo = FindFirstObjectByType<ChallengeInfo>();
        stageManager = GetComponent<StageManager>();


        hitsText = SingletonLocalizationManager.instance.GetComponent<LocalizationManager>().Get("content.challenge.noHit.num_hits");
        if (hitsText == null)
            hitsText = "hits";

        //take values for the stage challenge logic
        TextAsset jsonAsset = Resources.Load<TextAsset>("challengeInfo");
        JObject jroot = JObject.Parse(jsonAsset.text);
        JToken jt = jroot["Lvl"];
        jt = jt[stageManager.currentLvl + ""];
        jt = jt["Stage"];
        jt = jt[stageManager.currentStage + ""];
        JToken jtLimitVal = jt["limit"];
        if (jtLimitVal is JValue limitValue)
            hitsLimit = (int)limitValue;
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

        currentState = -1;
        
        challengeInfo.SetMedalGFX(challengeMedalKey);
        IncreaseHitCounter(0);
    }

    public void IncreaseHitCounter(int amount = 1)
    {
        if (stopCounter)
            return;
        hitsSinceStart += amount;
        challengeInfo.WriteText(hitsSinceStart + "/" + hitsLimit + " " + hitsText);
        if (!recordChallengeWon)
        {
            if (currentState != 1 && hitsSinceStart <= hitsLimit)
            {
                currentState = 1;
                challengeInfo.SetMedalState(1);
            } else if (currentState != 0 && hitsSinceStart > hitsLimit)
            {
                currentState = 0;
                challengeInfo.SetMedalState(0);
            }
        } 
    }

    public override ChallengeResults GetResultNow(bool stop = false)
    {
        stopCounter = stop;
        return new ChallengeResults(hitsSinceStart <= hitsLimit, hitsLimit, hitsSinceStart, "lessThan");
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
