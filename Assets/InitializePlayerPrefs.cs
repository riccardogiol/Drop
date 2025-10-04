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


        PlayerPrefs.SetInt("LastStagePlayed", 3);

        PlayerPrefs.SetInt("LastLevelPlayed", 7);

        PlayerPrefs.SetInt("WavePurchased", 0);
        */

        PlayerPrefs.SetInt("Lvl1Prize", 0);
        PlayerPrefs.SetInt("Lvl2Prize", 0);
        PlayerPrefs.SetInt("Lvl3Prize", 0);
        for (int i = 1; i <= 25; i++)
            PlayerPrefs.SetInt("Lvl" + i, 1);
        //for (int i = 22; i <= 25; i++)
            //PlayerPrefs.SetInt("Lvl" + i, 0);

        PlayerPrefs.SetInt("LastStagePlayed", 1);
        PlayerPrefs.SetInt("LastLevelPlayed", 5);

        PlayerPrefs.SetInt("LastStageCompleted", 1);
        PlayerPrefs.SetInt("LastLevelCompleted", 5);



        PlayerPrefs.SetInt("DemoVersion", 0);
        PlayerPrefs.SetInt("FullVersion", 1);

        PlayerPrefs.SetInt("CoinAmount", 2000);
    }

}
