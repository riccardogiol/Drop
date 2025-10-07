using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CounterText : MonoBehaviour
{
    List<string> codes = new List<string>();
    public List<ShelfButtonsManager> buttonParents;
    Text text;

    SelectCollectionButton scbAUX;
    int total, current = 0;

    void Awake()
    {
        text = GetComponent<Text>();
        if (text == null)
            return;
        
        foreach (ShelfButtonsManager buttonParent in buttonParents)
        {
            foreach (Transform button in buttonParent.elements)
            {
                scbAUX = button.GetComponent<SelectCollectionButton>();
                if (scbAUX != null)
                    codes.Add(scbAUX.unlockingCode);
            }
        }
        total = codes.Count;
        current = 0;
        foreach (string code in codes)
            if (PlayerPrefs.GetInt(code) > 0)
                current++;

        text.text = current + "/" + total;

        if (GetComponent<FitBoxText>() != null)
            GetComponent<FitBoxText>().Resize();
    }

}
