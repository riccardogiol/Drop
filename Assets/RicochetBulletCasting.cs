using UnityEngine;

public class RicochetBulletCasting : MonoBehaviour
{
    public GameObject ricochetBulletPrefab;
    public float speedBullet = 2.0f;
    public Transform target;
    public Transform bulletStart;

    PlaygroundManager playgroundManager;

    public float timer = 5f;
    public float delay = 0.3f;
    float countdown = 0;

    FlamesCountdown flamesCountdown;
    public float countdownTimer = 1.5f;
    float timer2;
    public int numerOfFlames = 3;

    public GameObject shootPS;

    Vector2 direction;

    void Awake()
    {
        playgroundManager = FindFirstObjectByType<PlaygroundManager>();
        flamesCountdown = GetComponent<FlamesCountdown>();
        countdown = delay;

        if (target == null)
        {
            target = FindFirstObjectByType<PlayerHealth>().transform;
        }

        if (PlayerPrefs.GetInt("EasyMode", 0) == 1)
        {
            timer = timer * 1.3f;
            speedBullet = speedBullet * 0.8f;
        }

        timer2 = countdownTimer;
    }


    void FixedUpdate()
    {
        direction = (target.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bulletStart.rotation = Quaternion.Euler(0, 0, angle);

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
            CastRicochetBullet();
            timer2 = countdownTimer;
        }

    }

    void CastRicochetBullet()
    {
        Instantiate(shootPS, bulletStart.position, bulletStart.rotation);
        GameObject br = Instantiate(ricochetBulletPrefab, bulletStart.position, Quaternion.identity);
        br.GetComponent<RicochetBulletManager>().SetTargetSpot(target);
        br.GetComponent<RicochetBulletManager>().SetShooterID(gameObject.GetInstanceID());
        br.GetComponent<RicochetBulletManager>().SetPM(playgroundManager);
    }
}
