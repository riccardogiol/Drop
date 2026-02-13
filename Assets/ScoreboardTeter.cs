using UnityEngine;

public class ScoreboardTeter : MonoBehaviour
{
    public string userID;
    public int score;

    public LocalScoreboardManager lsm;
    public ScoreboardDisplay sd;

    void Start()
    {
        lsm.AddScore(userID, score);
        ScoreEntry thisEntry = lsm.GetEntryByID(userID);
        sd.Display5Entries(thisEntry.position, userID);
    }
}
