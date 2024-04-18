using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ActivateButtonOnCountdown : MonoBehaviour
{
    public int timer = 5;
    float countdown;
    bool countingDown = false;
    public Button button;
    public Text buttonText;
    public bool onAwake = true;

    string originalText;

    void Awake()
    {
        originalText = buttonText.text;
        if (onAwake)
        {
            countdown = timer;
            button.interactable = false;
            buttonText.text = "" + timer;
            countingDown = true;
            //StartCoroutine(Countdown());
        }
    }


    void Update()
    {
        Time.timeScale = 1;
        if (countingDown)
        {
            countdown -= Time.deltaTime;
            buttonText.text = "" + (int)Math.Ceiling(countdown);
            if (countdown < 0)
            {
                button.interactable = true;
                countingDown = false;
                buttonText.text = originalText;
            }
        }
        
    }

    // IEnumerator Countdown()
    // {
    //     if (countingDown)
    //     {
    //         buttonText.text = countdown.ToString();
    //         countdown -= 1;
    //         if (countdown < 0)
    //         {
    //             button.interactable = true;
    //             countingDown = false;
    //             buttonText.text = originalText;
    //         }
    //     }
    //     yield return new WaitForSeconds(1);
    // }
}
