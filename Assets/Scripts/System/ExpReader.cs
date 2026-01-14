using UnityEngine;
using Newtonsoft.Json.Linq;

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

        //per tutti i livelli completati, aggiungi esperienza di tutti gli stage
        for (int l = 1; l <= lastLvl; l++)
        {
            jtlevel = jtroot["" + l];
            jtlevel = jtlevel["Stage"];
            for (int s = 1; s <= 4; s++)
            {
                jtstage = jtlevel["" + s];
                if (jtstage is JValue value)
                    totalExp += (int)value;
            }
        }
        // per livello non completato, MA possibili stage completati:
        jtlevel = jtroot["" + (lastLvl + 1)];
        if (jtlevel == null)
            return totalExp;
        jtlevel = jtlevel["Stage"];
        for (int s = 1; s <= lastStage; s++)
        {
            jtstage = jtlevel["" + s];
            if (jtstage is JValue value)
                totalExp += (int)value;
        }
        return totalExp;
    }

    // TODO aggiungere esperienza delle sfide + extra
}