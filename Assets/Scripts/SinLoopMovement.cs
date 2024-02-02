using System;
using UnityEngine;

public class SinLoopMovement : MonoBehaviour
{
    public float speed = 1;
    public float scaleX = 0;
    public float scaleY = 10;

    Vector3 startingPosition;

    void Awake()
    {
        startingPosition = GetComponent<RectTransform>().anchoredPosition;
    }

    void FixedUpdate()
    {
        float value = (float)Math.Sin(Time.time * speed);
        GetComponent<RectTransform>().anchoredPosition = new Vector3(startingPosition.x + value * scaleX, startingPosition.y + value * scaleY, 0);
    }
}
