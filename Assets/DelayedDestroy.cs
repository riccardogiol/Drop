using UnityEngine;

public class DelayedDestroy : MonoBehaviour
{
    public float delay = 0.5f;
    float timer = 0.0f;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > delay)
            Destroy(gameObject);
        
    }
}
