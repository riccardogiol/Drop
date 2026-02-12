using UnityEngine;
using UnityEngine.UI;

public class ScoreboardDisplay : MonoBehaviour
{

    public LocalScoreboardManager lsm;
    public Text panel;
    
    public void Display5Entries(int position) // aggiungine se ce ne sono chiedendo il primo o lultimo
    {
        ScoreEntry[] entries = new ScoreEntry[5];
        for (int i = 0; i < 5; i++)
        {
            entries[i] = lsm.GetEntryByPos(position - 2 + i);
        }

        string entriesToString = "";

        for (int i = 0; i < 5; i++)
        {
            if (entries[i] != null)
            {
                entriesToString = entriesToString + entries[i].position + "\t\t" + entries[i].userID + "\t\t" + entries[i].score + "\n";
            }
        }
        panel.text = entriesToString;
        
    }
}
