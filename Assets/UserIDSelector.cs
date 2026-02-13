using UnityEngine;
using UnityEngine.UI;

public class UserIDSelector : MonoBehaviour
{
    public Text idDisplay;
    public LocalScoreboardManager lsm;
    public GameObject scoreboardPanel;
    public ScoreboardTeter scoreboardInit;

    int currentScore;
    string currentID;

    char[] chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
    
    
    // Start is called before the first frame update
    void OnEnable()
    {
        currentScore = PlayerPrefs.GetInt("TotalScore", 0);
        SaveData saveData = SaveManager.Load();
        currentID = saveData.playerPublicName;
        if (currentID != "")
            SwitchToLeaderboardPanel();
        else
            GenerateRandomID();
    }

    public void GenerateRandomID()
    {
        string newID = "";
        int index = 0;
        for (int i = 0; i < 3; i++)
        {
            index = Random.Range(0, chars.Length);
            newID = newID + chars[index];
        }
        ScoreEntry scoreEntry = lsm.GetEntryByID(newID);
        if (scoreEntry != null)
            GenerateRandomID();
        else
        {
            idDisplay.text = newID;
            currentID = newID;
        }
    }

    public void ConfirmUserID()
    {
        SaveData saveData = SaveManager.Load();
        saveData.playerPublicName = currentID;
        SaveManager.Save(saveData);
        SwitchToLeaderboardPanel();
    }

    void SwitchToLeaderboardPanel()
    {
        scoreboardInit.userID = currentID;
        scoreboardInit.score = currentScore;
        scoreboardPanel.SetActive(true);
        gameObject.SetActive(false);
    }

}
