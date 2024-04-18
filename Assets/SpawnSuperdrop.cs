using UnityEngine;

public class SpawnSuperdrop : MonoBehaviour
{
    public GameObject superdropPrefab;
    public GameObject lightningBurstPrefab;
    ChangeAspect changeAspect;
    Transform parent;

    public float timer = 3;
    float countdown = 0;

    public float spawnDropProb = 0.5f;
    public Vector3 spawnSpot;

    void Awake()
    {
        if (PlayerPrefs.GetInt("SuperPurchased", 0) == 0)
           enabled = false;
    }

    void Start () {
        parent = transform.parent;
        changeAspect = GetComponent<ChangeAspect>();
        if (spawnSpot == Vector3.zero)
           spawnSpot = transform.position + new Vector3(1, 0, 0);
        else
            spawnSpot = transform.position + spawnSpot;
	}

    void Update ()
    {
        if (changeAspect.isBurnt)
            return;
        countdown -= Time.deltaTime;
        if (countdown <= 0)
        {
            countdown = timer;
            if (Random.value < spawnDropProb)
            {
                Instantiate(lightningBurstPrefab, spawnSpot, Quaternion.identity);
                GameObject goRef = Instantiate(superdropPrefab, spawnSpot, Quaternion.identity);
                goRef.transform.parent = parent;
            }
        }
    }
}
