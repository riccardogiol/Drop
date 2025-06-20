using UnityEngine;

public class FlamesCountdown : MonoBehaviour
{
    public ParticleSystem flameGFX;
    public float timer = 1.0f;
    public int numberOfFlames = 0;

    int counter = 0;
    float countdown = 0.0f;

    ParticleSystem.Burst burst;

    void Start()
    {
        burst = flameGFX.emission.GetBurst(0);
    }

    public void PlayCountdown(float interval, int NoF)
    {
        timer = interval;
        numberOfFlames = NoF;
        burst.count = counter = 1;
        flameGFX.emission.SetBurst(0, burst);
        flameGFX.Play();
        countdown = timer / numberOfFlames;
    }

    void Update()
    {
        if (counter >= numberOfFlames)
            return;
        countdown -= Time.deltaTime;
        if (countdown <= 0.0f)
        {
            counter++;
            burst.count = counter;
            flameGFX.emission.SetBurst(0, burst);
            flameGFX.Play();
            countdown = timer / numberOfFlames;
        }
    }
}
