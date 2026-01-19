using System.Collections.Generic;

public class ChallengeFlameOrder : ChallengeScript
{
    bool stillRightOrder = true;
    int expectedIndex = 0;
    int orderLimit;

    public List<ChallengeFlameOrderColliderManager> collidersList; //lista ordinata dei collider a cui assegno il numero progressivo in Awake, dopo che gli ho passato mestesso come reference (così che quando toccati possano avvertirmi)

    void Awake()
    {
        // initialize local var
        challengeTitleKey = "Order Challenge";
        challengeTextKey = "Clean the level estiguish the flames in the right order";
        challengeTextKey = "Target"; // prendere dal fine di salvataggio
        challengeInfo = FindFirstObjectByType<ChallengeInfo>();
        stageManager = GetComponent<StageManager>();

        for(int i = 0; i < collidersList.Count; i++)
            collidersList[i].InitializeValue(this, i);
        orderLimit = collidersList.Count;
        
        challengeInfo.WriteText(expectedIndex + "/" + orderLimit + " In Order");
    }

    public void ColliderTriggered(int index)
    {
        if (stillRightOrder && index == expectedIndex)
        {
            expectedIndex ++;
            challengeInfo.WriteText(expectedIndex + "/" + orderLimit + " In Order");
        } else
        {
            stillRightOrder = false;
            challengeInfo.WriteText("Out of Order");
        }
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

    public override ChallengeResults GetResultNow(bool stop = false)
    {
        return new ChallengeResults(stillRightOrder, orderLimit, expectedIndex, "greaterThan"); // magari mettere valore come numero di fiamme prese in ordine
    }
}