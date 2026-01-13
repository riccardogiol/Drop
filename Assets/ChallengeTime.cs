using UnityEngine;

public class ChallengeTime : MonoBehaviour
{
    public int timeLimit = 0;
    float timerSinceStart = 0;
    string challengeTitleKey;
    string challengeTextKey;

    ChallengeInfo challengeInfo;

    void Awake()
    {
      challengeInfo = FindFirstObjectByType<ChallengeInfo>();  
    }

    void Update()
    {
        timerSinceStart += Time.deltaTime;   
        challengeInfo.WriteText((int)timerSinceStart + "/" + timeLimit + " sec");
    }

    public ChallengResults GetResultNow()
    {
        return new ChallengResults(timerSinceStart <= timeLimit, timeLimit, (int)timerSinceStart, "lessThen");
    }
}


public class ChallengResults
{
    bool win;
    int limit;
    int value;
    string logic;

    public ChallengResults(bool v1, int l, int v, string v2)
    {
        win = v1;
        limit = l;
        value = v;
        logic = v2;
    }
}
