using UnityEngine;
using UnityEngine.UI;

public class LevelImageManager : MonoBehaviour
{
    public string unlockingLvl = "Lvl1";

    Image lockedImage;
    Image cleanedImage;

    void Start()
    {
        Transform auxTransf = transform.Find("LockedImage");
        if (auxTransf == null)
            return;
        lockedImage = auxTransf.GetComponent<Image>();
        auxTransf = transform.Find("ClearedImage");
        if (auxTransf == null)
            return;
        cleanedImage = auxTransf.GetComponent<Image>();

        lockedImage.enabled = false;
        cleanedImage.enabled = false;

        if (PlayerPrefs.GetInt(unlockingLvl, 0) == 1)
            cleanedImage.enabled = true;
        else
            lockedImage.enabled = true;
    }
}
