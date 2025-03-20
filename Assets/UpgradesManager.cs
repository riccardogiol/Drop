using System.Collections.Generic;
using UnityEngine;

public class UpgradesManager : MonoBehaviour
{
    public List<GameObject> upgrades = new List<GameObject>();

    public int RefundAllUpgrades()
    {
        int totalCoins = 0;
        string upgradeCode;
        foreach (GameObject upgrade in upgrades)
        {
            upgradeCode = upgrade.GetComponent<ButtonActivationManager>().buttonKeyCode;
            if (PlayerPrefs.GetInt(upgradeCode, 0) == 1)
            {
                PlayerPrefs.SetInt(upgradeCode, 0);
                totalCoins += upgrade.GetComponent<SelectUpgradeButton>().upgradePrice;
                upgrade.GetComponent<ButtonActivationManager>().UpdateGFX();
            }
        }
        return totalCoins;
    }
}
