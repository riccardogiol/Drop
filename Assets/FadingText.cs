using UnityEngine.UI;
using UnityEngine;

public class FadingText : MonoBehaviour
{
    public bool setStartingAlpha = false;
    public float startingAlpha;
    public float finalAlpha = 0f;
    public float alphaTimer = 0.3f;
    public float delayTimer = 0.4f;

    Text text;

    private float elapsedTime;
    private float red, green, blue;
    private bool delayExpired = false;

    void Start()
    {
        text = GetComponent<Text>();
        red = text.color.r;
        green = text.color.g;
        blue = text.color.b;
        if (!setStartingAlpha)
            startingAlpha = text.color.a;
        else
            text.color = new Color(red, green, blue, startingAlpha);

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
                text.color = new Color(red, green, blue, finalAlpha);
                enabled = false;
                return;
            }
            float currentAlpha = startingAlpha - ((startingAlpha - finalAlpha) * elapsedTime/alphaTimer);
            text.color = new Color(red, green, blue, currentAlpha);
        }
    }
}
