using UnityEngine;

public class SingletonAudioManager : MonoBehaviour
{
    public static SingletonAudioManager instance;

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
