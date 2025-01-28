using UnityEngine;

public class RandomSprite : MonoBehaviour
{
    public Sprite[] sprites;
    SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
        if (Random.value < 0.5)
            spriteRenderer.flipX = true;
        
    }
}
