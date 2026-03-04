using UnityEngine;

public class PhoenixTrickAchievementOnEnable : MonoBehaviour
{
    public PlayerHealth playerHealth;
    void OnEnable()
    {
        if (playerHealth.rebornThisStage && SteamAchivementManager.instance != null)
            SteamAchivementManager.instance.UnlockAchievement("ACH_PHX_TRK");
    }
}
