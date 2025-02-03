using UnityEngine;

public class InitializePlayerPrefs : MonoBehaviour
{
    
    void Awake()
    {
        PlayerPrefs.SetInt("Lvl1", 1);
        PlayerPrefs.SetInt("Lvl2", 1);
        PlayerPrefs.SetInt("Lvl3", 1);
        PlayerPrefs.SetInt("Lvl4", 1);
        PlayerPrefs.SetInt("Lvl5", 1);
        PlayerPrefs.SetInt("Lvl6", 1);
        PlayerPrefs.SetInt("Lvl7", 1);
        PlayerPrefs.SetInt("Lvl8", 1);
        PlayerPrefs.SetInt("Lvl9", 1);
        PlayerPrefs.SetInt("Lvl10", 1);
        PlayerPrefs.SetInt("Lvl11", 1);
        PlayerPrefs.SetInt("Lvl12", 1);
        PlayerPrefs.SetInt("Lvl13", 1);
        PlayerPrefs.SetInt("Lvl14", 1);
        PlayerPrefs.SetInt("Lvl15", 1);
        PlayerPrefs.SetInt("LastStagePlayed", 3);
        PlayerPrefs.SetInt("LastLevelPlayed", 7);
        PlayerPrefs.SetInt("CoinAmount", 1000);
        //PlayerPrefs.SetInt("Upgrade1", 0);
        //PlayerPrefs.SetInt("Upgrade2", 0);
        //PlayerPrefs.SetInt("WavePurchased", 0);
        //PlayerPrefs.SetInt("SuperPurchased", 0);
        //PlayerPrefs.SetInt("Super1Purchased", 0);
    }

}
