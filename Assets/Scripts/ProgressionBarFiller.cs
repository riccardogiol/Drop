using UnityEngine;
using UnityEngine.UI;

public class ProgressionBarFiller : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;

    public Image barr;
    public Image barrTop;

    public void SetMaxValue(float value)
    {
        slider.maxValue = value;
    }

    public void SetMinValue(float value)
    {
        slider.minValue = value;
    }

    public void SetValue(float currentValue)
    {
        slider.value = currentValue;
        barr.color = gradient.Evaluate(currentValue);
        barrTop.color = gradient.Evaluate(currentValue);
    }
}
