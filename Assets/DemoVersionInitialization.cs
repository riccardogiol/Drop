using UnityEngine;

public class DemoVersionInitialization : MonoBehaviour
{
    
    void Awake()
    {
        PlayerPrefs.SetInt("DemoVersion", 1);
    }

}
