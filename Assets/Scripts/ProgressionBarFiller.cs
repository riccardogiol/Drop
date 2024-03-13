using UnityEngine;
using UnityEngine.UI;

public class ProgressionBarFiller : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Gradient bossLifeGradient;

    bool isBossLife = false;

    public Image barr;
    public Image barrTop;
    
    public GameObject logo;

    public Transform gameoverLimit;
    public Transform rainLimit;

    public void SetImage(Sprite sprite, Vector3 position, Vector3 scale)
    {
        logo.GetComponent<Image>().sprite = sprite;
        logo.GetComponent<Image>().SetNativeSize();
        logo.GetComponent<RectTransform>().anchoredPosition = position;
        logo.GetComponent<RectTransform>().localScale = scale;
    }

    public void ShowBossLife()
    {
        isBossLife = true;
        gameoverLimit.gameObject.SetActive(false);
        rainLimit.gameObject.SetActive(false);
    }

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
            gameoverLimit.GetComponent<RectTransform>().anchoredPosition = new Vector2(-(value*400 + 20), 30);
    }

    public void SetRainLimit(float value)
    {
        if (value == 1)
            rainLimit.gameObject.SetActive(false);
        else
            rainLimit.GetComponent<RectTransform>().anchoredPosition = new Vector2(-(value*400 + 20), 30);
    }

    public void SetValue(float currentValue)
    {
        slider.value = currentValue;
        if (isBossLife)
        {
            barr.color = bossLifeGradient.Evaluate(currentValue);
            barrTop.color = bossLifeGradient.Evaluate(currentValue);

        } else {
            barr.color = gradient.Evaluate(currentValue);
            barrTop.color = gradient.Evaluate(currentValue);
        }
    }
}
