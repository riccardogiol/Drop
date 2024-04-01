using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;


public class AnalyticsChecker : MonoBehaviour
{
    async void Start()
    {
        await UnityServices.InitializeAsync();
        Debug.Log(UnityServices.State);
        // ask for consent
        AnalyticsService.Instance.StartDataCollection();
    }
}
