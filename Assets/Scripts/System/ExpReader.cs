using UnityEngine;
using Newtonsoft.Json.Linq;
using System;

public static class ExpReader
{
    public static int Get(int lvl, int stage)
    {
        TextAsset jsonAsset = Resources.Load<TextAsset>("stageExp");
        JObject jroot = JObject.Parse(jsonAsset.text);

        JToken jt = jroot["Lvl"];
        jt = jt["" + lvl];
        jt = jt["Stage"];
        jt = jt["" + stage];

        if (jt is JValue value)
            return (int)value;
        else
            return -99;
    }
    
    public static int GetTotal()
    {
        int lastLvl = PlayerPrefs.GetInt("LastLevelCompleted");
        int lastStage = PlayerPrefs.GetInt("LastStageCompleted");
        int totalExp = 0;

        TextAsset jsonAsset = Resources.Load<TextAsset>("stageExp");
        JObject jroot = JObject.Parse(jsonAsset.text);

        JToken jtroot = jroot["Lvl"];
        JToken jtlevel;
        JToken jtstage;

        jsonAsset = Resources.Load<TextAsset>("challengeInfo");
        jroot = JObject.Parse(jsonAsset.text);

        JToken jtChallRoot = jroot["Lvl"];
        JToken jtChallLevel;
        SaveData saveData = SaveManager.Load();

        //per tutti i livelli completati, aggiungi esperienza di tutti gli stage
        for (int l = 1; l <= lastLvl; l++)
        {
            jtlevel = jtroot["" + l];
            jtlevel = jtlevel["Stage"];
            jtChallLevel = jtChallRoot["" + l];
            jtChallLevel = jtChallLevel["Stage"];
            for (int s = 1; s <= 4; s++)
            {
                jtstage = jtlevel["" + s];
                if (jtstage is JValue value)
                    totalExp += (int)value;

                totalExp += EvaluateStageChallengeExp(jroot, jtChallLevel, saveData, l, s);
            }
        }
        
        // per livello non completato, MA possibili stage completati:
        jtlevel = jtroot["" + (lastLvl + 1)];
        if (jtlevel == null)
            return totalExp;
        jtlevel = jtlevel["Stage"];
        jtChallLevel = jtChallRoot["" + (lastLvl + 1)];
        jtChallLevel = jtChallLevel["Stage"];
        for (int s = 1; s <= lastStage; s++)
        {
            jtstage = jtlevel["" + s];
            if (jtstage is JValue value)
                totalExp += (int)value;
            
            totalExp += EvaluateStageChallengeExp(jroot, jtChallLevel, saveData, lastLvl + 1, s);
        }
        return totalExp;
    }

    private static int EvaluateStageChallengeExp(JObject jroot, JToken jtChallLevel, SaveData saveData, int l, int s)
    {
        int stageSaveIdx, challRecord, extraExp = 0;
        bool challengeWon;
        JToken jtChallStage;
        JToken jtChallType;
        stageSaveIdx = (l - 1) * 4 + s;
        challengeWon = saveData.StageCompleteStatus[stageSaveIdx] == 2;

        if (challengeWon)
        {
            jtChallStage = jtChallLevel["" + s];
            jtChallStage = jtChallStage["exp"];
            if (jtChallStage is JValue challExpVal)
                extraExp += (int)challExpVal;
        }

        if (saveData.StageChallengeRecords != null)
        {
            challRecord = saveData.StageChallengeRecords[stageSaveIdx];
            if (challRecord >= 0)
            {
                int type = 0, limit = 0;
                string logic = "";
                jtChallStage = jtChallLevel["" + s];
                jtChallStage = jtChallStage["limit"];
                if (jtChallStage is JValue challLimitVal)
                    limit = (int)challLimitVal;
                jtChallStage = jtChallLevel["" + s];
                jtChallStage = jtChallStage["type"];
                if (jtChallStage is JValue challTypeVal)
                    type = (int)challTypeVal;

                jtChallType = jroot["type"];
                jtChallType = jtChallType["" + type];
                jtChallType = jtChallType["logic"];
                if (jtChallType is JValue challLogic)
                    logic = (string)challLogic;

                extraExp += GetExtraExp(logic, challRecord, limit);
            }
        }

        return extraExp;
    }

    // check logic in StageManager L.264
    public static int GetExtraExp(string logic, int record, int limit)
    {
        int experience = 0;
        switch (logic)
        {
            case "lessThan":
                if (record <= limit)
                    experience = limit - record;
                break;
            case "greaterThanZero":
                if (record >= 0)
                    experience = record;
                break;
            default:
                experience = 0;
                break;
        }
        return experience;
    }

     public static int GetNewRecordExtraExp(string logic, int limit, int oldRecord, ChallengeWinInfo cwi)
    {
        int extraExp = 0;
        switch (logic)
        {
            case "lessThan":
                if (!cwi.chalAlrWon && cwi.chalWinNow)
                    extraExp = limit - cwi.recordValue;
                else if (cwi.chalAlrWon && cwi.chalWinNow)
                    extraExp = oldRecord - cwi.recordValue;
                break;
            case "greaterThanZero":
                extraExp = cwi.recordValue - Math.Max(oldRecord, 0);
                break;
        }
        return extraExp;
    }
}