using Newtonsoft.Json.Linq;
using UnityEngine;

public class ChallengeBoss2 :  ChallengeScript
{
    public PlayerSuperPower psp;
    int type = 0;
    int limit;
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
            limit = (int)limitValue;
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
        challengeInfo.WriteText("Not in SuperState");
    }

    void Start()
    {
      if (!psp.enabled)
        stopChecking = true;
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
        if (psp.IsInSuperState())
        {
            challengeInfo.WriteText("In Super State");
            if (!recordChallengeWon)
                challengeInfo.SetMedalState(1);
        } else 
        {
            challengeInfo.WriteText("Not in Super State");
            if (!recordChallengeWon)
                challengeInfo.SetMedalState(0);
        }
    }

    public override ChallengeResults GetResultNow(bool stop = false)
    {
        CheckCondition();
        stopChecking = stop;
        return new ChallengeResults(psp.IsInSuperState(), limit, 1, challengeLogic);
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
