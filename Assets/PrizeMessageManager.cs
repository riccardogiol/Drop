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
                string msg1 = SingletonLocalizationManager.instance.GetComponent<LocalizationManager>().Get("message.stage.level_clear.obtain1");
                if (msg1 == null)
                    msg1 = "You've obtained ";
                string msg2 = SingletonLocalizationManager.instance.GetComponent<LocalizationManager>().Get("message.stage.level_clear.obtain2");
                if (msg2 == null)
                    msg2 = " experience points";
                text.text = (msg1 + coinObtained + msg2).ToUpper();
                text.GetComponent<FitBoxText>().Resize();

                PlayerPrefs.SetInt("CoinAmount", PlayerPrefs.GetInt("CoinAmount", 0) + coinObtained);
            }
            PlayerPrefs.SetInt(lvlCodePrize, 1);
        } else {
            gameObject.SetActive(false);
        }
    }
}
