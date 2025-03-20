using System.Collections.Generic;
using UnityEngine;

public class StoreRefund : MonoBehaviour
{
    public List<UpgradesManager> upgradesManagers= new List<UpgradesManager>();
    public SelectUpgradeButton defaultButton;
    
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
        PlayerPrefs.SetInt("CoinAmount", currentCoin + totalCoinsRefund);

        coinCounterUpdate.Refresh();

        defaultButton.Select();
    }
}
