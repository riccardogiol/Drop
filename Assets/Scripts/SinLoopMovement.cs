using System;
using UnityEngine;

public class SinLoopMovement : MonoBehaviour
{
    public float speed = 1;
    public float scaleX = 0;
    public float scaleY = 10;

    Vector3 startingPosition;

    public bool timeInvariant = false;

    void Awake()
    {
        startingPosition = GetComponent<RectTransform>().anchoredPosition;
    }

    void Update()
    {
        float value = (float)Math.Sin(Time.time * speed);
        if (timeInvariant)
            value = (float)Math.Sin(Time.unscaledTime * speed);
        GetComponent<RectTransform>().anchoredPosition = new Vector3(startingPosition.x + value * scaleX, startingPosition.y + value * scaleY, 0);
    }
}
