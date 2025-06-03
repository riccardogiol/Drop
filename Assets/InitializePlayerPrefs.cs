using UnityEngine;

public class InitializePlayerPrefs : MonoBehaviour
{
    
    void Awake()
    {
        /*
        
        PlayerPrefs.SetInt("Upgrade1", 0);
        PlayerPrefs.SetInt("Upgrade2", 0);
        PlayerPrefs.SetInt("WavePurchased", );
        PlayerPrefs.SetInt("SuperPurchased", 0);
        PlayerPrefs.SetInt("Super1Purchased", 0);
        PlayerPrefs.SetInt("LastStagePlayed", 2);
        PlayerPrefs.SetInt("LastLevelPlayed", 8);
        PlayerPrefs.SetInt("Lvl5Prize", 0);
        PlayerPrefs.SetInt("Lvl11", 1);
        PlayerPrefs.SetInt("Lvl12", 1);
        PlayerPrefs.SetInt("Lvl13", 1);
        PlayerPrefs.SetInt("Lvl14", 1);
        PlayerPrefs.SetInt("Lvl15", 0);

        */

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

        PlayerPrefs.SetInt("EasyMode", 1);

        PlayerPrefs.SetInt("CoinAmount", 1000);
    }

}
