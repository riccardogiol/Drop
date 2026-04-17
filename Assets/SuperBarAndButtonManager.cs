using UnityEngine;
using UnityEngine.UI;

public class SuperBarAndButtonManager : MonoBehaviour
{
    public Slider slider;
    public Button button;
    public ParticleSystem psEffect;
    public Image mobileFiller;

    public void SetSliderMax(float value)
    {
        slider.maxValue = value;
    }

    public void UpdateSlider(float value)
    {
        slider.value = value;
        if (Application.isMobilePlatform)
            mobileFiller.fillAmount = value/slider.maxValue;
    }

    public void SetButtonInteractable(bool state)
    {
        button.interactable = state;
        if (state)
        {
            if (Application.isMobilePlatform)
                mobileFiller.fillAmount = 0;
            psEffect.Play();
        }
        else
            psEffect.Stop();
    }
}
