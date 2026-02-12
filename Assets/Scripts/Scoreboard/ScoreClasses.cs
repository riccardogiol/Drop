using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScoreEntry
{
    public string userID;
    public int score;
    public int position; // opzionale, puoi anche calcolarla runtime
}

[System.Serializable]
public class ScoreboardData
{
    public List<ScoreEntry> entries = new List<ScoreEntry>();
}