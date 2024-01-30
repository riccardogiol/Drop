using UnityEngine;

public class DestroyAndNeverShowAgain : MonoBehaviour
{
    public string code;

    void Awake()
    {
        if (PlayerPrefs.GetInt(code, 0) == 1)
        {
            Destroy(gameObject);
        }
    }

    public void DestroyForever()
    {
        PlayerPrefs.SetInt(code, 1);
        Destroy(gameObject);
    }
}
