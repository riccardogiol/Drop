using UnityEngine;
using UnityEngine.UI;

public class SuperBarAndButtonManager : MonoBehaviour
{
    public Slider slider;
    public Button button;

    public void SetSliderMax(float value)
    {
        slider.maxValue = value;
    }

    public void UpdateSlider(float value)
    {
        slider.value = value;
    }

    public void SetButtonInteractable(bool state)
    {
        button.interactable = state;
    }
}
