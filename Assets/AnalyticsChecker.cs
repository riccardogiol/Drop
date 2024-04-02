using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;


public class AnalyticsChecker : MonoBehaviour
{
    bool freezeTime;
    async void Start()
    {
        if (PlayerPrefs.GetInt("FromWorldMap", 0) == 1)
        {
            Time.timeScale = 1;
            freezeTime = false;
            gameObject.SetActive(false);
            PlayerPrefs.SetInt("FromWorldMap", 0);
            return;
        }
        await UnityServices.InitializeAsync();
        Debug.Log(UnityServices.State);
        freezeTime = true;
    }

    void LateUpdate()
    {
        if (freezeTime)
        {
            Time.timeScale = 0;
            freezeTime = false;
        }
    }

    public void TakeChoice(bool consentAnalytics)
    {
        if (consentAnalytics)
            AnalyticsService.Instance.StartDataCollection();
        Time.timeScale = 1;
        freezeTime = false;
        gameObject.SetActive(false);
    }
}
