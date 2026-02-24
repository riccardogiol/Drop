using UnityEngine;

public class UnlockAchievementOnEnable : MonoBehaviour
{
    public string achievementID = "ACH_XXX_YYY";

    void OnEnable()
    {
        if (SteamAchivementManager.instance != null)
            SteamAchivementManager.instance.UnlockAchievement(achievementID);    
    }
}
