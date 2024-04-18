using UnityEngine;
using UnityEngine.UI;

public class FontSizeOnPlayerPref : MonoBehaviour
{

    public string key = "LvlX";
    public int fontSize = 10;

    void Awake()
    {
        if (PlayerPrefs.GetInt(key, 0) == 1)
        {
            GetComponent<Text>().fontSize = fontSize;
        }
        
    }

}
