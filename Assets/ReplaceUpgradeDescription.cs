using UnityEngine;

public class ReplaceUpgradeDescription : MonoBehaviour
{
    public string whenVarTrue = "DemoVersion";
    public string newUpgradeDescriptionKey;
    SelectUpgradeButton selectUpgradeButton;

    void Start()
    {
        if (PlayerPrefs.GetInt(whenVarTrue, 0) == 1)
        {
            selectUpgradeButton = GetComponent<SelectUpgradeButton>();
            selectUpgradeButton.upgradeDescriptionKey = newUpgradeDescriptionKey;
        }
    }
}
