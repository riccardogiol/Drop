using UnityEngine;

public class InitializePlayerPrefs : MonoBehaviour
{

    void Awake()
    {
        /*
        
        PlayerPrefs.SetInt("Upgrade1", 0);
        PlayerPrefs.SetInt("Upgrade2", 0);
        PlayerPrefs.SetInt("SuperPurchased", 0);
        PlayerPrefs.SetInt("Super1Purchased", 0);
        PlayerPrefs.SetInt("Lvl5Prize", 0);

        PlayerPrefs.SetInt("WavePurchased", 0);

        PlayerPrefs.SetInt("LastStagePlayed", 3);
        PlayerPrefs.SetInt("LastLevelPlayed", 7);

        */
        PlayerPrefs.SetInt("Lvl1", 1);
        PlayerPrefs.SetInt("Lvl1Prize", 0);
        PlayerPrefs.SetInt("Lvl2Prize", 0);
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
        PlayerPrefs.SetInt("Lvl16", 0);
        PlayerPrefs.SetInt("Lvl17", 0);
        PlayerPrefs.SetInt("Lvl18", 0);
        PlayerPrefs.SetInt("Lvl19", 0);
        PlayerPrefs.SetInt("Lvl20", 0);
        PlayerPrefs.SetInt("Lvl21", 0);
        PlayerPrefs.SetInt("Lvl22", 0);
        PlayerPrefs.SetInt("Lvl23", 0);
        PlayerPrefs.SetInt("Lvl24", 0);
        PlayerPrefs.SetInt("Lvl25", 0);

        PlayerPrefs.SetInt("LastStagePlayed", 1);
        PlayerPrefs.SetInt("LastLevelPlayed", 15);

        PlayerPrefs.SetInt("LastStageCompleted", 1);
        PlayerPrefs.SetInt("LastLevelCompleted", 15);



        PlayerPrefs.SetInt("DemoVersion", 0);

        PlayerPrefs.SetInt("CoinAmount", 2000);
    }

}
