using UnityEngine.UI;
using UnityEngine;

public class Fading : MonoBehaviour
{
    public bool setStartingAlpha = false;
    public float startingAlpha;
    public float finalAlpha = 0f;
    public float alphaTimer = 0.3f;
    public float delayTimer = 0.4f;

    private Image imageUI;
    private SpriteRenderer spriteRenderer;


    private float elapsedTime;
    private float red, green, blue;
    private bool delayExpired = false;

    void Start()
    {
        imageUI = GetComponent<Image>();
        if (imageUI != null)
        {
            red = imageUI.color.r;
            green = imageUI.color.g;
            blue = imageUI.color.b;
            if (!setStartingAlpha)
                startingAlpha = imageUI.color.a;
            else
                imageUI.color = new Color(red, green, blue, startingAlpha);
        } else {
            spriteRenderer = GetComponent<SpriteRenderer>();
            red = spriteRenderer.color.r;
            green = spriteRenderer.color.g;
            blue = spriteRenderer.color.b;
            if (!setStartingAlpha)
                startingAlpha = spriteRenderer.color.a;
            else
                spriteRenderer.color = new Color(red, green, blue, startingAlpha);
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
