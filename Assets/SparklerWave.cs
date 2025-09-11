using UnityEngine;

public class SparklerWave : MonoBehaviour
{
    public GameObject waterdropPrefab;
    public GameObject wavePrefab;
    public GameObject parent;
    PlaygroundManager playgroundManager;

    public float timer = 3;
    public float delay = 0.3f;
    public float spawnDropProb = 0.5f;
    float countdown = 0;

    public bool bigWave = false;

    void Start () {
        playgroundManager = FindFirstObjectByType<PlaygroundManager>();
        if (parent == null)
            parent = gameObject;
        countdown = delay;

        if (PlayerPrefs.GetInt("EasyMode", 0) == 1)
            timer = timer * 0.7f;
	}

    void FixedUpdate()
    {
        countdown -= Time.fixedDeltaTime;
        if (countdown <= 0)
        {
            countdown = timer;
            SpawnPrefabs();
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
                if (!playgroundManager.IsObstacle(randomPos))
                {
                    GameObject goRef = Instantiate(waterdropPrefab, randomPos, Quaternion.identity);
                    playgroundManager.SubscribeWaterdrop(goRef);
                    goRef.GetComponent<PickWaterdrop>().randomEnergy = false;
                    goRef.GetComponent<PickWaterdrop>().energy = 2;
                    goRef.GetComponent<PickWaterdrop>().ScaleOnEnergy();
                    waveRef.GetComponent<Wave>().SubscribeID(goRef.GetInstanceID());
                }
            }   
	}
}
