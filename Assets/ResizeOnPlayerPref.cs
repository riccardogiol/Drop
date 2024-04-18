using UnityEngine;

public class ResizeOnPlayerPref : MonoBehaviour
{
    public string key = "LvlX";
    public float width, height;

    void Awake()
    {
        if (PlayerPrefs.GetInt(key, 0) == 1)
        {
            GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
        }
        
    }
}
