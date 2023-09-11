using System;
using UnityEngine;

public class SpriteChangingOnValue : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public bool randomlyFlipX = false;
    public Sprite[] sprites;
    public float[] thresholds;

    public void Evaluate(float value)
    {
        for (int i = 0; i < thresholds.Length; i++)
        {
            if (value <= thresholds[i])
            {
                spriteRenderer.sprite = sprites[ Math.Min(i, sprites.Length - 1)];
                if (randomlyFlipX && UnityEngine.Random.value > 0.5)
                    spriteRenderer.flipX = true;
                return;
            }
        }
        spriteRenderer.sprite = sprites[sprites.Length - 1];
        if (randomlyFlipX && UnityEngine.Random.value > 0.5)
            spriteRenderer.flipX = true;
    }

}
