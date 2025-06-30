using UnityEngine;

public class SpawnIceshielddrop : MonoBehaviour
{
    public GameObject iceshielddropPrefab;
    public GameObject iceshieldBurstPrefab;
    Transform parent;

    public float timer = 3;
    float countdown = 0;

    public float spawnDropProb = 0.5f;
    public Vector3 spawnSpot;

    void Start () {
        countdown = timer;
        parent = transform;
        spawnSpot = transform.position + spawnSpot;
	}

    void Update ()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0)
        {
            countdown = timer;
            if (Random.value < spawnDropProb)
            {
                Instantiate(iceshieldBurstPrefab, spawnSpot, Quaternion.identity);
                GameObject goRef = Instantiate(iceshielddropPrefab, spawnSpot, Quaternion.identity);
                goRef.transform.parent = parent;
            }
        }
    }
}
