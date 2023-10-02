using UnityEngine;

public class ChangeAspect : MonoBehaviour
{
    public bool isBurnt = true;

    public Sprite burntSprite;
    public Sprite greenSprite;

    public SpriteRenderer spriteRenderer;

    void Start()
    {
        if (isBurnt)
            spriteRenderer.sprite = burntSprite;
        else
            spriteRenderer.sprite = greenSprite;
    }

    public void SetGreenSprite()
    {
        spriteRenderer.sprite = greenSprite;
    }

    public void SetBurntSprite()
    {
        spriteRenderer.sprite = burntSprite;
    }
}
