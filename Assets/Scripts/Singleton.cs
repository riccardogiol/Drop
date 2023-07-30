using UnityEngine;

public class Singleton : MonoBehaviour
{
    public static Singleton instance;

    void Awake() {

        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
}
