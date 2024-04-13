using UnityEngine;
using UnityEngine.UI;

public class PrizeMessageManager : MonoBehaviour
{
    StageManager stageManager;
    public int coinObtained = 0;
    public Text text;

    void Start()
    {
        stageManager = FindFirstObjectByType<StageManager>();
        string lvlCodePrize = "Lvl" + stageManager.currentLvl + "Prize";
        if (PlayerPrefs.GetInt(lvlCodePrize, 0) == 0)
        {
            if (coinObtained > 0)
            {
                text.text = "Obtain " + coinObtained + " raincoins!";
                PlayerPrefs.SetInt("CoinAmount", PlayerPrefs.GetInt("CoinAmount", 0) + coinObtained);
            }
            PlayerPrefs.SetInt(lvlCodePrize, 1);
        } else {
            gameObject.SetActive(false);
        }
    }
}
