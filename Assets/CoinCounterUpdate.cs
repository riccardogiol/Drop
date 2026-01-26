using UnityEngine;
using UnityEngine.UI;

public class CoinCounterUpdate : MonoBehaviour
{
    Text text;
    void Start()
    {
        text = GetComponent<Text>();
        Refresh();
    }

    public void Refresh()
    {
        text.text = PlayerPrefs.GetInt("CoinAmount", 0) + "/\n" + PlayerPrefs.GetInt("TotalScore", 0);
    }
}
