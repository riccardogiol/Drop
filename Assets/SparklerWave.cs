using System.Collections;
using UnityEngine;

public class SparklerWave : MonoBehaviour
{
    public GameObject waterdropPrefab;
    public GameObject wavePrefab;
    public GameObject parent;
    PlaygroundManager playgroundManager;

    public float timer = 3;
    public float delay = 0f;
    public float spawnDropProb = 0.5f;

    void Start () {
        playgroundManager = FindFirstObjectByType<PlaygroundManager>();
        if (parent == null)
            parent = gameObject;
		StartCoroutine(SpawnPrefabsWithDelay());
	}

    IEnumerator SpawnPrefabsWithDelay()
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(SpawnPrefabs());
    }

	IEnumerator SpawnPrefabs() {
		while (true) {
            GameObject waveRef = Instantiate(wavePrefab, transform.position, Quaternion.identity);
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
            
			yield return new WaitForSeconds(timer);
		}
	}
}
