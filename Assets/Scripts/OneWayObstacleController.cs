using UnityEngine;

public class OneWayObstacleController : MonoBehaviour
{
    public bool blockingFromBelow;
    public bool blockingFromAbove;
    public bool blockingFromLeft;
    public bool blockingFromRight;

    SpriteRenderer spriteRenderer;
    PlatformEffector2D platformEffector2D;

    public Sprite spriteAbove;
    public Sprite spriteBelow;
    public Sprite spriteLeft;
    public Sprite spriteRight;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        platformEffector2D = GetComponent<PlatformEffector2D>();
        
        if (blockingFromAbove)
        {
            spriteRenderer.sprite = spriteAbove;
            platformEffector2D.rotationalOffset = 0;
        } else if (blockingFromBelow)
        {
            spriteRenderer.sprite = spriteBelow;
            platformEffector2D.rotationalOffset = 180;
        } else if (blockingFromLeft)
        {
            spriteRenderer.sprite = spriteLeft;
            platformEffector2D.rotationalOffset = 90;
        } else
        {
            spriteRenderer.sprite = spriteRight;
            platformEffector2D.rotationalOffset = 270;
        }
    }

}
