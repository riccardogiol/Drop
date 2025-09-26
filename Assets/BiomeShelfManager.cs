using UnityEngine;
using UnityEngine.UI;

public class BiomeShelfManager : MonoBehaviour
{
    public Text altitudeLabel;
    public string textKey = "menu.world.altitude";
    public string meters = "0-500m";
    public GameObject previousBiomePage;
    public GameObject nextBiomePage;

    void Awake()
    {
        string localText = SingletonLocalizationManager.instance.GetComponent<LocalizationManager>().Get(textKey);
        if (localText == null)
            localText = "Altitude";
        if (meters != "")
            localText = localText + "\n" + meters;
        altitudeLabel.text = localText.ToUpper();
    }

    public void NextBiomePage()
    {
        nextBiomePage.SetActive(true);
        gameObject.SetActive(false);
    }

    public void PreviousBiomePage()
    {
        previousBiomePage.SetActive(true);
        gameObject.SetActive(false);
    }

}
