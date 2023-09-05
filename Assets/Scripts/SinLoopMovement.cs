using System;
using UnityEngine;

public class SinLoopMovement : MonoBehaviour
{
    public float speed = 1;
    public float scaleX = 0;
    public float scaleY = 10;

    void FixedUpdate()
    {
        float value = (float)Math.Sin(Time.time * speed);
        GetComponent<RectTransform>().anchoredPosition = new Vector3(value * scaleX, value * scaleY, 0);
    }
}
