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
        /*
        int totalCoinsRefund = 0;
        foreach (UpgradesManager upg in upgradesManagers)
        {
            totalCoinsRefund += upg.RefundAllUpgrades();
        }
        int currentCoin = PlayerPrefs.GetInt("CoinAmount", 0);
        int totalExp = ExpReader.GetTotal(); // in verità qua é proprio il total score, non vado a chiamare il calcolatore di esperienza
        */
        
        int totalCoin = PlayerPrefs.GetInt("TotalScore", 0);
        if (PlayerPrefs.GetInt("WavePurchased", 0) == 1)
            totalCoin -= WaveCost;

        //if (totalExp != totalCoinsRefund + currentCoin) // probabilmente fare solo totalExp... o tenere i due con controllo che poi tanto non legge nessuno?
        //    Debug.Log("Experience difference between memory and save");
        
        PlayerPrefs.SetInt("CoinAmount", totalCoin);

        coinCounterUpdate.Refresh();

        defaultButton.Select();
    }
}
