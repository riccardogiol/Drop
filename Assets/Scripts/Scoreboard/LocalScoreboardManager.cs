using System.IO;
using UnityEngine;

public class LocalScoreboardManager : MonoBehaviour
{
    private string folderPath;
    private string scoresRecordPath;

    public ScoreboardData scoreboard = new ScoreboardData();

    void Awake()
    {
        folderPath = Path.Combine(Application.persistentDataPath, "ScoreData");
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);
        
        scoresRecordPath = Path.Combine(folderPath, "scores_record.json");
        
        Load();
    }

    // get the saveData object con USERID!!!!!!
    public void AddScore(string userID, int score)
    {
        // elaborate total score [later]
        // extract user and so the entry
        ScoreEntry oldScoreEntry = GetEntryByID(userID);
        if (oldScoreEntry != null)
        {
            scoreboard.entries[oldScoreEntry.position - 1].score = score; // or create a function that udate the value for an ID?
        } else
        {
            ScoreEntry entry = new ScoreEntry
            {
                userID = userID,
                score = score
            };
            scoreboard.entries.Add(entry);
        }

        SortScores();
        Save();
    }

    public ScoreEntry GetEntryByPos(int pos)
    {
        if (pos <1 || pos>scoreboard.entries.Count)
            return null;
        return scoreboard.entries[pos - 1];
    }

    // can be used to see if the name it's already in
    public ScoreEntry GetEntryByID(string id)
    {
        for (int i = 0; i < scoreboard.entries.Count; i++)
            if (scoreboard.entries[i].userID == id)
                return scoreboard.entries[i];
        return null;
    }

    void SortScores()
    {
        scoreboard.entries.Sort((a, b) => b.score.CompareTo(a.score)); // eventualmente non tutto da riordinare ma solo un entry

        for (int i = 0; i < scoreboard.entries.Count; i++)
            scoreboard.entries[i].position = i + 1;
    }

    public void Save()
    {
        string json = JsonUtility.ToJson(scoreboard, true);
        File.WriteAllText(scoresRecordPath, json);
    }

    public void Load()
    {
        if (File.Exists(scoresRecordPath))
        {
            string json = File.ReadAllText(scoresRecordPath);
            scoreboard = JsonUtility.FromJson<ScoreboardData>(json);
        }
        else
        {
            scoreboard = new ScoreboardData();
        }
    }
}
