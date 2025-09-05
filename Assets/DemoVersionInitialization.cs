using UnityEngine;

public class DemoVersionInitialization : MonoBehaviour
{
    public bool isDemo = false;
    
    void Awake()
    {
        if (isDemo)
        {
            PlayerPrefs.SetInt("DemoVersion", 1);
            PlayerPrefs.SetInt("FullVersion", 0);
        }
        else
        {
            PlayerPrefs.SetInt("DemoVersion", 0);
            PlayerPrefs.SetInt("FullVersion", 1);
        }
    }

}
