using UnityEngine;

public class SparklerWave : MonoBehaviour
{
    public GameObject waterdropPrefab;
    public GameObject wavePrefab;
    public GameObject parent;
    public ParticleSystem waterSparkles;
    public EnergyIndicator energyIndicator;
    PlaygroundManager playgroundManager;

    public float timer = 3;
    public float delay = 0.3f;
    public float spawnDropProb = 0.5f;
    float countdown = 0;
    float countdownLabel = 0;

    public int ammo = -100;

    public bool bigWave = false;

    bool ready = false;

    void Start () {
        playgroundManager = FindFirstObjectByType<PlaygroundManager>();
        if (parent == null)
            parent = gameObject;
        countdown = delay;
        ready = false;

        //TURNAROUND to not change everything after the interaction update
        timer = timer / 2f;

        if (PlayerPrefs.GetInt("EasyMode", 0) == 1)
        {
            timer = timer * 0.7f;
            ammo = -100;
        }

        energyIndicator.ShowEnergy();
        countdownLabel = 1;
	}

    // void FixedUpdate()
    // {
    //     countdown -= Time.fixedDeltaTime;
    //     if (countdown <= 0)
    //     {
    //         countdown = timer;
    //         SpawnPrefabs();
    //     }
    // }

    void FixedUpdate()
    {
        if (countdownLabel > 0)
        {
            countdownLabel -= Time.fixedDeltaTime;
            if (countdownLabel <= 0)
                energyIndicator.HideEnergy();
        }

        if (ready)
            return;
        if (ammo == 0)
            return;
        countdown -= Time.fixedDeltaTime;
        if (countdown <= 0)
        {
            countdown = timer;   
            ready = true;
            waterSparkles.Play();
        }
    }
    

	void SpawnPrefabs() {
        GameObject waveRef = Instantiate(wavePrefab, transform.position, Quaternion.identity);
        waveRef.transform.parent = transform;
        waveRef.GetComponent<Wave>().shootByPlayer = false;
        waveRef.GetComponent<Wave>().damage = 2;
        waveRef.GetComponent<Wave>().playgroundManager = playgroundManager;
        if (bigWave)
            waveRef.GetComponent<Wave>().bigWave = true;
        if (Random.value < spawnDropProb)
        {
            Vector3 randomPos = transform.position + new Vector3(Random.Range(-1, 2), Random.Range(-1, 2));
            if (!playgroundManager.IsObstacle(randomPos) && playgroundManager.IsOnPlayground(playgroundManager.GetCellCenter(randomPos)))
            {
                GameObject goRef = Instantiate(waterdropPrefab, randomPos, Quaternion.identity);
                playgroundManager.SubscribeWaterdrop(goRef);
                goRef.GetComponent<PickWaterdrop>().randomEnergy = false;
                goRef.GetComponent<PickWaterdrop>().energy = 2;
                goRef.GetComponent<PickWaterdrop>().ScaleOnEnergy();
                waveRef.GetComponent<Wave>().SubscribeID(goRef.GetInstanceID());
            }
        }

        ready = false;
        waterSparkles.Stop();
        ammo--;
        energyIndicator.ShowEnergy();
        countdownLabel = 1;
	}

    public void TriggerWave(bool forceTrigger = false)
    {
        energyIndicator.ShowEnergy();
        countdownLabel = 1;
        if (ready || forceTrigger)
            SpawnPrefabs();
    }
}
