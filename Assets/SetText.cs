using System;
using UnityEngine;
using UnityEngine.UI;

public class SetText : MonoBehaviour
{
    public Text text;
    public float fadingTimer = 0.0f;

    float countdown = 0.0f;
    Color auxColor;

    void Start ()
    {
        if (fadingTimer > 0.0f)
            countdown = fadingTimer;
    }

    void Update()
    {
        if (fadingTimer > 0.0f)
        {
            countdown -= Time.deltaTime;
            auxColor = text.color;
            auxColor.a = Math.Max(countdown/fadingTimer, 0);
            text.color = auxColor;
        }
        
    }

    public void SetInt(int value)
    {
        text.text = value.ToString();
    }
}
