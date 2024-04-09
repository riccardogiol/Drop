using UnityEngine;
using UnityEngine.UI;

public class CoinCounterUpdate : MonoBehaviour
{
    Text text;
    void Start()
    {
        text = GetComponent<Text>();
        text.text = "" + PlayerPrefs.GetInt("CoinAmount", 0);
    }

    public void Refresh()
    {
        text.text = "" + PlayerPrefs.GetInt("CoinAmount", 0);
    }
}
