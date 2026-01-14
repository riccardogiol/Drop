using UnityEngine;
using Newtonsoft.Json.Linq;

public class ChallengeTime : MonoBehaviour
{
    int timeLimit = 0;
    float timerSinceStart = 0;
    bool stopTimer = false;
    string challengeTitleKey;
    string challengeTextKey;

    ChallengeInfo challengeInfo;
    StageManager stageManager;

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

    public ChallengeResults GetResultNow(bool stop = false)
    {
        stopTimer = stop;
        return new ChallengeResults(timerSinceStart <= timeLimit, timeLimit, (int)timerSinceStart, "lessThen");
    }
}


public class ChallengeResults
{
    public bool win;
    public int limit;
    public int value;
    public string logic;

    public ChallengeResults(bool v1 = false, int l = 0, int v = 0, string v2 = "")
    {
        win = v1;
        limit = l;
        value = v;
        logic = v2;
    }
}

public class ChallengeWinInfo
{
    public bool chalAlrWon= false, chalWinNow= false, newRec = false;
    public int recordValue = 0;
    public int chalWonExp = 0, extraExp = 0;

    public ChallengeWinInfo()
    {}
}
