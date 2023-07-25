using UnityEngine;
using UnityEngine.UI;

public class LevelButtonManager : MonoBehaviour
{
    public string unlockingLvl = "Lvl1";
    public string codeLvl = "Lvl1";

    Image lockedImage;
    Image unlockedImage;
    Image cleanedImage;
    Button button;

    void Start()
    {
        Transform auxTransf = transform.Find("LockedImage");
        if (auxTransf == null)
            return;
        lockedImage = auxTransf.GetComponent<Image>();
        auxTransf = transform.Find("UnlockedImage");
        if (auxTransf == null)
            return;
        unlockedImage = auxTransf.GetComponent<Image>();
        auxTransf = transform.Find("CleanedImage");
        if (auxTransf == null)
            return;
        cleanedImage = auxTransf.GetComponent<Image>();
        auxTransf = transform.Find("Button");
        // modify this to have only image case?
        if (auxTransf == null)
            return;
        button = auxTransf.GetComponent<Button>();

        button.interactable = false;
        lockedImage.enabled = false;
        unlockedImage.enabled = false;
        cleanedImage.enabled = false;

        if (PlayerPrefs.GetInt(unlockingLvl, 0) == 1)
        {
            button.interactable = true;
            if (PlayerPrefs.GetInt(codeLvl, 0) == 1)
                cleanedImage.enabled = true;
            else
                unlockedImage.enabled = true;
        } else
            lockedImage.enabled = true;
    }
}
