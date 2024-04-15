using UnityEngine;

public class InitializePlayerPrefs : MonoBehaviour
{
    
    void Awake()
    {
        PlayerPrefs.SetInt("Lvl5", 1);
        PlayerPrefs.SetInt("CoinAmount", 1000);
        PlayerPrefs.SetInt("Upgrade1", 0);
        PlayerPrefs.SetInt("Upgrade2", 0);
        PlayerPrefs.SetInt("WavePurchased", 0);
        PlayerPrefs.SetInt("SuperPurchased", 0);
        PlayerPrefs.SetInt("Super1Purchased", 0);
    }

}
