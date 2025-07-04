using System.Collections;
using UnityEngine;

public class RiverWave : MonoBehaviour
{
    // find parent for flame
    public GameObject waterdropPrefab;
    public GameObject wavePrefab;
    public GameObject parent;
    PlaygroundManager playgroundManager;

    public float delay = 0.3f;
    public float spawnDropProb = 0.0f;
    float countdown = 0;
    bool triggered = false;
    bool notTrigger = false;
    float stopTimer = 1.0f;

    public bool dryRiver = false;
    SpriteRenderer riverGFX;
    Sprite waterSprite;
    public Sprite[] dryRiverSprites;
    public GameObject waterComing;
    public ParticleSystem waterFlowPS;


    void Awake()
    {
        riverGFX = GetComponent<SpriteRenderer>();
        waterSprite = riverGFX.sprite;
        if (dryRiver)
        {
            riverGFX.sprite = dryRiverSprites[Random.Range(0, dryRiverSprites.Length)];
            waterFlowPS.Stop();
            waterFlowPS.Clear();
            var main = waterFlowPS.main;
            main.prewarm = false;
        }
    }

    void Start () {
        
        playgroundManager = FindFirstObjectByType<PlaygroundManager>();
        if (parent == null)
            parent = gameObject;
        countdown = delay;
	}

    public void TriggerWave(bool shootByPlayer = true)
    {
        if (dryRiver && shootByPlayer)
                return;
        if (notTrigger)
            return;
        triggered = true;
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
                SpawnPrefabs();
            }
        }
        if (notTrigger)
        {
            countdown -= Time.fixedDeltaTime;
            if (countdown <= 0)
            {
                countdown = delay;
                notTrigger = false;
            }
        }
    }

	void SpawnPrefabs() {
        if (dryRiver)
        {
            dryRiver = false;
            StartCoroutine(ChangeGFXDelayed());
            Instantiate(waterComing, transform.position, Quaternion.identity);
            waterFlowPS.Play();
        }
        GameObject waveRef = Instantiate(wavePrefab, transform.position, wavePrefab.transform.rotation);
        waveRef.transform.parent = transform;
        waveRef.GetComponent<Wave>().shootByPlayer = false;
        waveRef.GetComponent<Wave>().damage = 2;
        waveRef.GetComponent<Wave>().playgroundManager = playgroundManager;
        if (Random.value < spawnDropProb)
        {
            Vector3 randomPos = transform.position + new Vector3(Random.Range(-1, 2), Random.Range(-1, 2));
            if (!playgroundManager.IsObstacle(randomPos))
            {
                GameObject goRef = Instantiate(waterdropPrefab, randomPos, Quaternion.identity);
                goRef.GetComponent<PickWaterdrop>().randomEnergy = false;
                goRef.GetComponent<PickWaterdrop>().energy = 2;
            }
        }   
	}

    IEnumerator ChangeGFXDelayed()
    {
        yield return new WaitForSeconds(0.15f);
        riverGFX.sprite = waterSprite;
    }
}
