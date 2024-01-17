using UnityEngine.UI;
using UnityEngine;

public class Fading : MonoBehaviour
{
    public float finalAlpha = 0f;
    public float alphaTimer = 0.3f;
    public float delayTimer = 0.4f;

    public Image imageUI;

    private float elapsedTime;
    private SpriteRenderer spriteRenderer;
    private float red, green, blue, startingAlpha;
    private bool delayExpired = false;

    void Start()
    {
        if (imageUI != null)
        {
            red = imageUI.color.r;
            green = imageUI.color.g;
            blue = imageUI.color.b;
            startingAlpha = imageUI.color.a;
        } else {
            spriteRenderer = GetComponent<SpriteRenderer>();
            red = spriteRenderer.color.r;
            green = spriteRenderer.color.g;
            blue = spriteRenderer.color.b;
            startingAlpha = spriteRenderer.color.a;
        }
    }

    void FixedUpdate()
    {
        elapsedTime += Time.deltaTime;
        if (!delayExpired && elapsedTime > delayTimer)
        {
            elapsedTime = 0;
            delayExpired = true;
        } else if (delayExpired) {
            if (elapsedTime > alphaTimer)
            {
                if (imageUI != null)
                    imageUI.color = new Color(red, green, blue, finalAlpha);
                else
                    spriteRenderer.color = new Color(red, green, blue, finalAlpha);
                this.enabled = false;
                return;
            }
            float currentAlpha = startingAlpha - ((startingAlpha - finalAlpha) * elapsedTime/alphaTimer);
            if (imageUI != null)
                imageUI.color = new Color(red, green, blue, currentAlpha);
            else
                spriteRenderer.color = new Color(red, green, blue, currentAlpha);
        }
    }
}
