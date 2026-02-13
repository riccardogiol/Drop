using UnityEngine;
using UnityEngine.UI;

public class ScoreboardDisplay : MonoBehaviour
{

    public LocalScoreboardManager lsm;
    public Text panel;

    public int currentPosition;
    public string currentUser;
    
    public void Display5Entries(int position, string userID = "") // aggiungine se ce ne sono chiedendo il primo o l'ultimo !!!!
    {
        if (userID != "")
            currentUser = userID;
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
                if (entries[i].userID == currentUser)
                    entriesToString =  entriesToString + "<color=#E2C72A>" + entries[i].position + "\t\t" + entries[i].userID + "\t\t" + entries[i].score + "</color>\n";
                else
                    entriesToString = entriesToString + entries[i].position + "\t\t" + entries[i].userID + "\t\t" + entries[i].score + "\n";
            }
        }
        panel.text = entriesToString;
        currentPosition = position;
    }

    public void ShowPositionDown()
    {
        int requestedPosition = currentPosition + 1;
        if (requestedPosition < 1 || requestedPosition > lsm.scoreboard.entries.Count)
            return;
        Display5Entries(requestedPosition);
    }

    public void ShowPositionUp()
    {
        int requestedPosition = currentPosition - 1;
        if (requestedPosition < 1 || requestedPosition > lsm.scoreboard.entries.Count)
            return;
        Display5Entries(requestedPosition);
    }

    public void ShowPositionTop()
    {
        Display5Entries(3);
    }

    public void ShowPositionUser()
    {
        int userPosition = lsm.GetEntryByID(currentUser).position;
        Display5Entries(userPosition);
    }
}
