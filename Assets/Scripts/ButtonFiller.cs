using UnityEngine;
using UnityEngine.UI;

public class ButtonFiller : MonoBehaviour
{
    public Slider slider;

    public void SetMaxValue(float value)
    {
        slider.maxValue = value;
    }

    public void SetValue(float currentValue)
    {
        slider.value = currentValue;
    }
}
