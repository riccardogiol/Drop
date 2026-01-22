using UnityEngine;

public class DelayActive : MonoBehaviour
{
    public bool random = false;
    public float maxSecs = 0;
    float delay = 0;
    public ParticleSystem particleSystemTarget;

    void Awake()
    {
        delay = maxSecs;
        if (random)
            delay = Random.Range(0, maxSecs);
        if (delay >= 0.01)
        {
            if (particleSystemTarget != null)
                particleSystemTarget.Stop();
        }
    }

    void Update()
    {
        delay -= Time.deltaTime;
        if (delay <= 0)
        {
            if (particleSystemTarget != null)
                particleSystemTarget.Play();
            enabled = false;
        }
        
    }
}
