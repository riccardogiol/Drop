using UnityEngine;

public class MortarBombCasting : MonoBehaviour
{
    public GameObject mortarPrefab;
    public Transform target;
    public Transform mortarStart;

    PlaygroundManager playgroundManager;

    public float timer = 5f;
    public float delay = 0.3f;
    float countdown = 0;

    FlamesCountdown flamesCountdown;
    public float countdownTimer = 1.5f;
    float timer2;
    public int numerOfFlames = 3;

    public GameObject shootPS;


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
            CastMortar();
            timer2 = countdownTimer;
        }

    }
    
    void CastMortar()
    {
        Vector3 targetCellCenter = playgroundManager.GetCellCenter(target.position);
        Instantiate(shootPS, mortarStart.position, Quaternion.LookRotation(Vector3.forward, new Vector3(0, -1, 0)));
        GameObject mortar = Instantiate(mortarPrefab, mortarStart.position, Quaternion.identity);
        mortar.GetComponent<MortarBombManager>().SetTargetSpot(targetCellCenter);
    }
}
