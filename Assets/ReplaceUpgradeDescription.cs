using UnityEngine;

public class ReplaceUpgradeDescription : MonoBehaviour
{
    public string whenVarTrue = "DemoVersion";
    [TextArea]
    public string newUpgradeDescription;
    SelectUpgradeButton selectUpgradeButton;

    void Awake()
    {
        if (PlayerPrefs.GetInt(whenVarTrue, 0) == 1)
        {
            selectUpgradeButton = GetComponent<SelectUpgradeButton>();
            selectUpgradeButton.upgradeDescription = newUpgradeDescription;
        }
    }
}
