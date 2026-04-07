using UnityEngine;

#if STEAMWORKS_NET
using Steamworks;
#endif

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
        
        #if STEAMWORKS_NET
        initialized = SteamAPI.Init();
        #endif

        if (!initialized)
        {
            Debug.LogWarning("Steam non inizializzato");
        }
    }
    void Start()
    {
        
        #if STEAMWORKS_NET
        if (initialized)
            Debug.Log("Steam App ID: " + SteamUtils.GetAppID());
        #endif
    }

    void Update()
    {
        
        #if STEAMWORKS_NET
        if (initialized)
            SteamAPI.RunCallbacks();
        #endif
    }

    public void UnlockAchievement(string id)
    {
        if (!initialized) return;

        if (PlayerPrefs.GetInt("DemoVersion", 0) == 1) return;

        #if STEAMWORKS_NET
        SteamUserStats.SetAchievement(id);
        SteamUserStats.StoreStats();
        #endif
    }

    void OnApplicationQuit()
    {
        if (initialized)
        {
            #if STEAMWORKS_NET
            SteamAPI.Shutdown();
            #endif
        }
    }
}
