using UnityEngine;

public class NoiseRandomMovement : MonoBehaviour
{
    public float amplitude = 1f;
    public float speed = 1f;  

    public float seed=0;

    Vector3 startPos;
    float offsetX, offsetY;

    public float multOnX = 1;
    public float multOnY = 1;

    void Start()
    {
        startPos = transform.localPosition;
        if (seed == 0)
        {
            offsetX = Random.Range(0f, 100f);
            offsetY = Random.Range(0f, 100f);
        } else 
        {
            offsetX = seed;
            offsetY = seed;
        }
    }

    void Update()
    {
        float x = Mathf.PerlinNoise(Time.time * speed + offsetX, 0) * 2f - 1f;
        float y = Mathf.PerlinNoise(0, Time.time * speed + offsetY) * 2f - 1f;
        
        transform.localPosition = startPos + new Vector3(x * multOnX, y * multOnY, 0) * amplitude;
    }
}

