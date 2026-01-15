using UnityEngine;

public abstract class ChallengeScript : MonoBehaviour
{
    public string challengeTitleKey {get; protected set;}
    public string challengeTextKey {get; protected set;}

    ChallengeInfo challengeInfo;
    StageManager stageManager;

    public abstract ChallengeResults GetResultNow(bool stop = false);
    public abstract ChallengeWinInfo EvaluateWinInfo(ChallengeResults challengeResults, ChallengeResults challengeRecord);
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
