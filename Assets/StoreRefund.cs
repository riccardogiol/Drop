using System.Collections.Generic;
using UnityEngine;

public class StoreRefund : MonoBehaviour
{
    public List<UpgradesManager> upgradesManagers= new List<UpgradesManager>();
    public SelectUpgradeButton defaultButton;

    public int WaveCost = 160;
    
    CoinCounterUpdate coinCounterUpdate;

     void Awake()
    {
        coinCounterUpdate = FindFirstObjectByType<CoinCounterUpdate>();
    }


    public void RefundAll()
    {
        int totalCoinsRefund = 0;
        foreach (UpgradesManager upg in upgradesManagers)
        {
            totalCoinsRefund += upg.RefundAllUpgrades();
        }
        int currentCoin = PlayerPrefs.GetInt("CoinAmount", 0);
        int totalExp = ExpReader.GetTotal();
        if (PlayerPrefs.GetInt("WavePurchased", 0) == 1)
            totalExp -= WaveCost;

        if (totalExp != totalCoinsRefund + currentCoin)
            Debug.Log("Experience difference between memory and save");
        PlayerPrefs.SetInt("CoinAmount", totalExp);

        coinCounterUpdate.Refresh();

        defaultButton.Select();
    }
}
