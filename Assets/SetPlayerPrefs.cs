using UnityEngine;

public class SetPlayerPrefs : MonoBehaviour
{
    public string varName = "";
    public int value;

    void Awake()
    {
        PlayerPrefs.SetInt(varName, value);
    }
}
