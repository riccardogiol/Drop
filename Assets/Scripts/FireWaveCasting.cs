using UnityEngine;

public class FireWaveCasting : MonoBehaviour
{
    public GameObject wavePrefab;
    public int waveDamage = 4;
    PlaygroundManager playgroundManager;

    public float timer = 5f;
    public float delay = 0.3f;
    public float spawnFlameProb = 0.5f;
    float countdown = 0;

    FlamesCountdown flamesCountdown;
    float countdownTimer = 1.5f;
    float timer2;
    int numerOfFlames = 3;

    void Awake()
    {
        playgroundManager = FindFirstObjectByType<PlaygroundManager>();
        flamesCountdown = GetComponent<FlamesCountdown>();
        countdown = delay;

        if (PlayerPrefs.GetInt("EasyMode", 0) == 1)
            timer = timer * 1.3f;

        timer2 = countdownTimer;
    }

    void FixedUpdate()
    {
        countdown -= Time.fixedDeltaTime;
        if (countdown <= timer2)
        {
            if (flamesCountdown != null)
                flamesCountdown.PlayCountdown(countdownTimer, numerOfFlames);
            timer2 = -99;
        }
        if (countdown <= 0)
        {
            countdown = timer;
            CastWave();
            timer2 = countdownTimer;
        }
    }

    void CastWave()
    {
        GameObject wave = Instantiate(wavePrefab, transform.position, Quaternion.identity);
        wave.transform.parent = transform;
        wave.GetComponent<FireWave>().playgroundManager = playgroundManager;
        wave.GetComponent<FireWave>().damage = waveDamage;
        wave.GetComponent<FireWave>().shootByID = gameObject.GetInstanceID();
        wave.GetComponent<FireWave>().spawnFlameProb = spawnFlameProb;
    }
}