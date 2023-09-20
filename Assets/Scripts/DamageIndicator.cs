using System;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour
{
    float timer = 0.8f;
    float countdown = 0f;
    int valueToDisplay = 0;

    Color auxColor;
    public Text text;

    public void ShowDamage(int damage)
    {
        if (countdown > 0)
            valueToDisplay = valueToDisplay - damage;
        else
            valueToDisplay = - damage;
        SetText();
    }

    public void ShowEnergy(int energy)
    {
        if (countdown > 0)
            valueToDisplay = valueToDisplay + energy;
        else
            valueToDisplay = energy;
        SetText();
    }

    void SetText()
    {
        if (valueToDisplay >= 0)
            text.text = "+" + valueToDisplay;
        else
            text.text = "" + valueToDisplay;
        auxColor = text.color;
        auxColor.a = 1;
        text.color = auxColor;
        countdown = timer;
    }

    void Update()
    {
        if (countdown >= 0)
        {
            countdown -= Time.deltaTime;
            auxColor = text.color;
            auxColor.a = Math.Max(countdown/timer, 0);
            text.color = auxColor;
        }
        
    }
}
