using UnityEngine;

public class SingletonLocalizationManager : MonoBehaviour
{
    public static SingletonLocalizationManager instance;

    void Awake() {

        if (instance == null)
            instance = this;
        else
        {
            DestroyImmediate(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
}
