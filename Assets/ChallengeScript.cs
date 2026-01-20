using UnityEngine;

public abstract class ChallengeScript : MonoBehaviour
{
    public string challengeTitleKey {get; protected set;}
    public string challengeTextKey {get; protected set;}
    public string challengeLimitKey {get; protected set;}
    public string challengeMedalKey {get; protected set;}

    protected ChallengeInfo challengeInfo;
    protected StageManager stageManager;

    public abstract ChallengeResults GetResultNow(bool stop = false);
    public abstract ChallengeWinInfo EvaluateWinInfo(ChallengeResults challengeResults, ChallengeResults challengeRecord);
    public void UpdateWinCondition(int winState)
    {
        challengeInfo.SetMedalState(winState);
    }
}


public class ChallengeResults
{
    public bool win;
    public int limit;
    public int value;
    public string logic;

    public ChallengeResults(bool winState = false, int challengeLimit = 0, int currentValue = 0, string conditionExplain = "")
    {
        win = winState;
        limit = challengeLimit;
        value = currentValue;
        logic = conditionExplain;
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
