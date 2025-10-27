using UnityEngine;

public class NoiseRandomAlpha : MonoBehaviour
{
    public float fromAlpha = 0.5f;
    public float toAlpha = 1f;
    float diff, a;
    Color startColor;

    public float speed = 1f;
    public float seed = 0;
    float offset;


    SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();  
        startColor = spriteRenderer.color;
    }

    void Start()
    {
        if (seed == 0)
            offset = Random.Range(0f, 100f);
        else
            offset = seed;
        diff = toAlpha - fromAlpha;
    }

    void Update()
    {
        a = fromAlpha + diff * Mathf.PerlinNoise(Time.time * speed + offset, 0);
        spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, a);
    }
}
