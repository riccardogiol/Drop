using UnityEngine;

public class InitializePlayerPrefs : MonoBehaviour
{
    
    void Awake()
    {
        PlayerPrefs.SetInt("Lvl8", 0);
        PlayerPrefs.SetInt("LastStagePlayed", 3);
        PlayerPrefs.SetInt("LastLevelPlayed", 7);
        //PlayerPrefs.SetInt("CoinAmount", 1000);
        //PlayerPrefs.SetInt("Upgrade1", 0);
        //PlayerPrefs.SetInt("Upgrade2", 0);
        //PlayerPrefs.SetInt("WavePurchased", 0);
        //PlayerPrefs.SetInt("SuperPurchased", 0);
        //PlayerPrefs.SetInt("Super1Purchased", 0);
    }

}
