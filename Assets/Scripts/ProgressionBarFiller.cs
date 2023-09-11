using UnityEngine;
using UnityEngine.UI;

public class ProgressionBarFiller : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;

    public Image barr;
    public Image barrTop;

    public Transform gameoverLimit;

    public void SetMaxValue(float value)
    {
        slider.maxValue = value;
    }

    public void SetMinValue(float value)
    {
        slider.minValue = value;
    }

    public void SetGameOverLimit(float value)
    {
        if (value == 0)
            gameoverLimit.gameObject.SetActive(false);
        else
            gameoverLimit.Translate(- (value*400), 0, 0);
    }

    public void SetValue(float currentValue)
    {
        slider.value = currentValue;
        barr.color = gradient.Evaluate(currentValue);
        barrTop.color = gradient.Evaluate(currentValue);
    }
}
