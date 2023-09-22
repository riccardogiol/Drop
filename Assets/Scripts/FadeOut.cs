using System;
using UnityEngine;

public class FadeOut : MonoBehaviour
{
    public float timer = 0.8f;
    float countdown = 0f;

    Color auxColor;
    public SpriteRenderer sprite;

    void Start()
    {
        countdown = timer;
    }

    void Update()
    {
        if (countdown >= 0)
        {
            countdown -= Time.deltaTime;
            auxColor = sprite.color;
            auxColor.a = Math.Max(countdown/timer, 0);
            sprite.color = auxColor;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
