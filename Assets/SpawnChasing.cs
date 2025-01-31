using UnityEngine;

public class SpawnChasing : MonoBehaviour
{
    PlaygroundManager playgroundManager;
    public float timer = 5.0f;
    float countdown;
    public Vector3 spawnSpot;
    public GameObject chasingEnemyPrefab;
    FireCounter fireCounter;

    void Start()
    {
        playgroundManager = FindFirstObjectByType<PlaygroundManager>();
        fireCounter = FindFirstObjectByType<FireCounter>();
        countdown = 5.0f;
    }

    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0)
        {
            countdown = timer;
            GameObject spawnEnemy = Instantiate(chasingEnemyPrefab, spawnSpot, Quaternion.identity);
            spawnEnemy.GetComponent<EnemyHealth>().maxHealth = 5;
            spawnEnemy.GetComponent<EnemyGroundInteraction>().spawnFlameProbability = 0.01f;
            spawnEnemy.GetComponent<EnemyAIChasingMovement>().jumpInterval = 1.3f;
            spawnEnemy.transform.parent = fireCounter.transform;
        }
        
    }
}
