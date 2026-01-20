using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;

public class StagePanelManager : MonoBehaviour
{
    public Image stageImage;
    public Image cloudHiding;
    public Text titleText;
    public Text descriptionText;
    public ChallengeBoxManager challengeBoxManager;

    public int level;
    public int stage;

    bool isCompleted;

    public void UpdateInfo(int levelCode, int stageCode)
    {
        level = levelCode;
        stage = stageCode;

        if (PlayerPrefs.GetInt("LastLevelCompleted", 0) >= levelCode)
            isCompleted = true;
        else
        {
            if (PlayerPrefs.GetInt("LastStageCompleted", 0) >= stageCode)
                isCompleted = true;
            else
                isCompleted = false;
        }

        string levelLoc = SingletonLocalizationManager.instance.GetComponent<LocalizationManager>().Get("menu.stage.pause_menu.level");
        if (levelLoc == null)
            levelLoc = "Level";
        string stageLoc = SingletonLocalizationManager.instance.GetComponent<LocalizationManager>().Get("menu.stage.pause_menu.stage");
        if (stageLoc == null)
            stageLoc = "Stage";
        titleText.text = (levelLoc + " " + level + " - " + stageLoc + " " + stage).ToUpper();

        // compose description
        string aux;
        string typeLine = "- ";
        aux = SingletonLocalizationManager.instance.GetComponent<LocalizationManager>().Get("menu.world.type");
        if (aux == null)
            aux = "Type";
        typeLine += aux + ": ";
        aux = SingletonLocalizationManager.instance.GetComponent<LocalizationManager>().Get("content.stage_info.level" + levelCode + ".stage" + stageCode + ".type");
        if (aux == null)
            aux = "xxx";
        typeLine += aux + "\n";

        string diffLine = "- ";
        aux = SingletonLocalizationManager.instance.GetComponent<LocalizationManager>().Get("menu.world.difficulty");
        if (aux == null)
            aux = "Difficulty";
        diffLine += aux + ": ";
        aux = SingletonLocalizationManager.instance.GetComponent<LocalizationManager>().Get("content.stage_info.level" + levelCode + ".stage" + stageCode + ".difficulty");
        if (aux == null)
            aux = "xxx";
        diffLine += aux + "\n";

        string complLine = "- ";
        aux = SingletonLocalizationManager.instance.GetComponent<LocalizationManager>().Get("menu.world.completed");
        if (aux == null)
            aux = "Completed";
        complLine += aux + ": ";
        if (isCompleted)
            aux = SingletonLocalizationManager.instance.GetComponent<LocalizationManager>().Get("menu.world.comp_yes");
        else
            aux = SingletonLocalizationManager.instance.GetComponent<LocalizationManager>().Get("menu.world.comp_no");
        if (aux == null)
            aux = "no";
        complLine += aux + "\n";

        string tropLine = "- ";
        aux = SingletonLocalizationManager.instance.GetComponent<LocalizationManager>().Get("menu.world.trophy");
        if (aux == null)
            aux = "Trophy";
        tropLine += aux + ": ";
        aux = SingletonLocalizationManager.instance.GetComponent<LocalizationManager>().Get("content.stage_info.level" + levelCode + ".trophy");
        if (aux == null)
            aux = "bee";
        aux = SingletonLocalizationManager.instance.GetComponent<LocalizationManager>().Get("content.trophy." + aux + ".name");
        if (aux == null)
            aux = "bee";
        tropLine += aux;

        descriptionText.text = typeLine + diffLine + complLine + tropLine;

        //replaceImage
        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/StageOverview/lvl" + levelCode);
        Sprite sprite = System.Array.Find(sprites, s => s.name == stageCode.ToString());
        if (sprite == null)
            sprite = Resources.Load<Sprite>("Sprites/StageOverview/1-1");
        stageImage.sprite = sprite;
        if (!isCompleted)
        {
            cloudHiding.enabled = true;
            stageImage.color = new Color(0.5f, 0.5f, 0.5f);
        } else
        {
            cloudHiding.enabled = false;
            stageImage.color = new Color(1f, 1f, 1f);
        }

        // fill challenge infos
        int challengeType = 0;
        ChallengeResults challengeRecord = new ChallengeResults();
        string challengeTitleKey = "", challengeDescriptionKey = "", challengeLimitKey = "", challengeMedalKey = "";
        TextAsset jsonAsset = Resources.Load<TextAsset>("challengeInfo");
        JObject jroot = JObject.Parse(jsonAsset.text);
        JToken jt = jroot["Lvl"];
        jt = jt[levelCode + ""];
        jt = jt["Stage"];
        jt = jt[stageCode + ""];
        JToken jtType = jt["type"]; // check if there is?
        if (jtType is JValue value)
            challengeType = (int)value;
        JToken jtLim = jt["limit"]; // check if there is?
        if (jtLim is JValue value2)
            challengeRecord.limit = (int)value2;
        
        SaveData saveData = SaveManager.Load();
        if (saveData.StageChallengeRecords != null)
        {
            challengeRecord.value = saveData.StageChallengeRecords[(levelCode - 1) * 4 + stageCode];
            challengeRecord.win = challengeRecord.value >= 0; 
        }

        jt = jroot["type"];
        jt = jt[challengeType + ""];
        JToken jtTitle = jt["title"];
        if (jtTitle is JValue value3)
            challengeTitleKey = (string)value3;
        JToken jtDescription = jt["description"];
        if (jtDescription is JValue value4)
            challengeDescriptionKey = (string)value4;
        JToken jtLimit = jt["limit"];
        if (jtLimit is JValue value5)
            challengeLimitKey = (string)value5;
        JToken jtMedal = jt["medal_code"];
        if (jtMedal is JValue value6)
            challengeMedalKey = (string)value6;

        challengeBoxManager.DisplayMenuInfoMessage(challengeTitleKey, challengeDescriptionKey, challengeLimitKey, challengeMedalKey, challengeRecord);
    }

    public void PlayStage()
    {
        FindObjectOfType<AudioManager>().ResetSounds();
        SceneManager.LoadScene("Stage" + level + "-" + stage);
    }

}
