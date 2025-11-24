using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CameraEffectManager : MonoBehaviour
{
    public VolumeProfile volumeProfile;

    float minVignette = 0.15f, maxVignette = 0.4f;
    float minTemperature = 0, maxtemperature = 60;

    float minExp = 0, maxExp = 0f;
    float minSat = -15, maxSat = 7;

    WhiteBalance whiteBalance;
    Vignette vignette;
    ColorAdjustments colorAdjustments;

    void Awake()
    {
        volumeProfile.TryGet<WhiteBalance>(out whiteBalance);
        volumeProfile.TryGet<Vignette>(out vignette);

        volumeProfile.TryGet<ColorAdjustments>(out colorAdjustments);
    }

    public void SetEffect(float perc)
    {
        if (vignette == null)
            return;
        vignette.intensity.value = minVignette + (maxVignette - minVignette) * (1-perc);
        whiteBalance.temperature.value = minTemperature + (maxtemperature - minTemperature) * (1-perc);

        colorAdjustments.postExposure.value = minExp + (maxExp - minExp) * perc*perc;
        colorAdjustments.saturation.value = minSat + (maxSat - minSat) * perc*perc;
    }
}
