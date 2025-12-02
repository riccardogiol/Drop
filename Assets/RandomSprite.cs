using UnityEngine;

public class RandomSprite : MonoBehaviour
{
    public Sprite[] sprites;
    SpriteRenderer spriteRenderer;
    public float flipXProb = 0.5f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
        if (Random.value < flipXProb)
            spriteRenderer.flipX = true;
        
    }
}
