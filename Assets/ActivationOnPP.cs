using UnityEngine;

public class ActivationOnPP : MonoBehaviour
{
    public string playerPrefsKey = "";
    public int triggerValue = 1;
    public bool disableObj = true;
    void Awake()
    {
        if (PlayerPrefs.GetInt(playerPrefsKey, 0) == triggerValue)
        {
            if (disableObj)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
