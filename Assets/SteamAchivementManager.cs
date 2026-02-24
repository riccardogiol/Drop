using UnityEngine;
using Steamworks;

public class SteamAchivementManager : MonoBehaviour
{
    public static SteamAchivementManager instance;

    public bool initialized;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        initialized = SteamAPI.Init();

        if (!initialized)
        {
            Debug.LogWarning("Steam non inizializzato");
        }
    }
    void Start()
    {
        Debug.Log("Steam App ID: " + SteamUtils.GetAppID());
    }

    void Update()
    {
        if (initialized)
            SteamAPI.RunCallbacks();
    }

    public void UnlockAchievement(string id)
    {
        if (!initialized) return;

        SteamUserStats.SetAchievement(id);
        SteamUserStats.StoreStats();
    }

    void OnApplicationQuit()
    {
        if (initialized)
        {
            SteamAPI.Shutdown();
        }
    }
}
