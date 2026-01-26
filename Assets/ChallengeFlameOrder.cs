using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class ChallengeFlameOrder : ChallengeScript
{
    bool stillRightOrder = true;
    int expectedIndex = 0;
    int orderLimit;

    public List<ChallengeFlameOrderColliderManager> collidersList; //lista ordinata dei collider a cui assegno il numero progressivo in Awake, dopo che gli ho passato mestesso come reference (così che quando toccati possano avvertirmi)

    void Awake()
    {
        // initialize local var
        challengeInfo = FindFirstObjectByType<ChallengeInfo>();
        stageManager = GetComponent<StageManager>();

        for(int i = 0; i < collidersList.Count; i++)
            collidersList[i].InitializeValue(this, i);
        orderLimit = collidersList.Count;
        
        challengeInfo.WriteText(expectedIndex + "/" + orderLimit + " In Order");

        TextAsset jsonAsset = Resources.Load<TextAsset>("challengeInfo"); // da tirare fuori?
        JObject jroot = JObject.Parse(jsonAsset.text);
        JToken jt = jroot["type"];
        jt = jt["2"];
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
        return new ChallengeResults(stillRightOrder, orderLimit, expectedIndex, "greaterThanZero");
    }
}