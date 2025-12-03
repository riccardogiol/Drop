using UnityEngine;

public class DestroyAndNeverShowAgain : MonoBehaviour
{
    public string code;
    public bool liveCheck = false;

    void Awake()
    {
        if (PlayerPrefs.GetInt(code, 0) == 1)
            Destroy(gameObject);
    }

    void Update()
    {
        if (!liveCheck)
            return;
        if (PlayerPrefs.GetInt(code, 0) == 1)
            Destroy(gameObject);
    }

    public void DestroyForever()
    {
        PlayerPrefs.SetInt(code, 1);
        Destroy(gameObject);
    }
}
