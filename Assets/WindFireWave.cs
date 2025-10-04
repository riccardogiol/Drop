using System.Collections.Generic;
using UnityEngine;

public class WindFireWave : MonoBehaviour
{
    public GameObject fireWavePrefab;

    PlaygroundManager playgroundManager;

    public float delay = 0.3f;
    public float spawnFlameProb = 0.2f;

    public bool triggerWithEnemy = true;

    float countdown = 0;
    bool triggered = false;
    bool notTrigger = false;
    float stopTimer = 2.0f;

    float refreshTimer = 5.0f; 
    float refreshCountdown = 0f;

    public Color windColor;
    public Color fireWindColor;

    public ParticleSystem windFlowPS;
    public ParticleSystem leavesPS;

    List<int> lastTouchedIDs = new List<int>();
    int triggerdByID;

    void Start()
    {
        playgroundManager = FindFirstObjectByType<PlaygroundManager>();
        countdown = delay;
    }

    public void TriggerWave()
    {
        if (notTrigger)
            return;
        triggered = true;
        refreshCountdown = refreshTimer;
    }

    void FixedUpdate()
    {
        if (triggered)
        {
            countdown -= Time.fixedDeltaTime;
            if (countdown <= 0)
            {
                countdown = stopTimer;
                triggered = false;
                notTrigger = true;
                var main1 = windFlowPS.main;
                main1.startColor = fireWindColor;
                main1 = leavesPS.main;
                main1.startColor = fireWindColor;
                SpawnPrefabs();
            }
        }
        else if (notTrigger)
        {
            countdown -= Time.fixedDeltaTime;
            if (countdown <= 0)
            {
                countdown = delay;
                notTrigger = false;
            }
        }

        refreshCountdown -= Time.deltaTime;
        if (refreshCountdown <= 0)
        {
            bool touchFlame = false;
            Collider2D[] results = Physics2D.OverlapPointAll(transform.position);
            foreach(Collider2D item in results)
            {
                if (item.gameObject.CompareTag("Flame"))
                    touchFlame = true;
                if (item.gameObject.CompareTag("Enemy"))
                    touchFlame = true;
            }
            if (touchFlame)
                TriggerWave();
            else
            {
                var main1 = windFlowPS.main;
                main1.startColor = windColor;
                main1 = leavesPS.main;
                main1.startColor = windColor;
            }
            refreshCountdown = refreshTimer;
        }
    }

    void SpawnPrefabs()
    {
        GameObject waveRef = Instantiate(fireWavePrefab, transform.position, fireWavePrefab.transform.rotation);
        waveRef.transform.parent = transform;
        waveRef.GetComponent<FireWave>().damage = 2;
        waveRef.GetComponent<FireWave>().playgroundManager = playgroundManager;
        waveRef.GetComponent<FireWave>().spawnFlameProb = spawnFlameProb;
        waveRef.GetComponent<FireWave>().touchedIDs = lastTouchedIDs;
        lastTouchedIDs = new List<int>();
        if (triggerdByID != 0)
        {
            waveRef.GetComponent<FireWave>().shootByID = triggerdByID;
            triggerdByID = 0;
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && triggerWithEnemy)
        {
            triggerdByID = other.gameObject.GetInstanceID();
            TriggerWave();
        }
        if (other.CompareTag("Flame"))
        {
            triggerdByID = other.gameObject.GetInstanceID();
            TriggerWave();
        }
        if (other.CompareTag("FireWave"))
        {
            lastTouchedIDs = other.GetComponent<FireWave>().touchedIDs;
            TriggerWave();
        }
        if (other.CompareTag("FireBullet"))
            TriggerWave();
    }
}
