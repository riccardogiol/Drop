using System;
using UnityEngine;

public class SinLoopScaling : MonoBehaviour
{
    public float speed = 1;
    public float strenght = 0.5f;
    public float delay = 0f;

    Vector3 startingScale;

    void Awake()
    {
        startingScale = transform.localScale;
    }

    void FixedUpdate()
    {
        float value = 1 + Math.Max((float)Math.Sin(Time.time * speed - delay*0.7), -0.5f) * strenght;
        transform.localScale = new Vector3(startingScale.x * value, startingScale.y * value, startingScale.z);
    }
}
