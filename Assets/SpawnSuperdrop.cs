using UnityEngine;
using System;

public class SpawnSuperdrop : MonoBehaviour
{
    public GameObject superdropPrefab;
    public GameObject lightningBurstPrefab;
    ChangeAspect changeAspect;
    Transform parent;

    public float timer = 3;
    float countdown = 0;

    public float spawnDropProb = 0.5f;
    public Transform spawnSpot;

    void Awake()
    {
        if (PlayerPrefs.GetInt("SuperPurchased", 0) == 0)
            enabled = false;
        
        if (spawnSpot == null)
            foreach (Transform child in transform)
                if (child.name == "SpawnSpot")
                    spawnSpot = child;
    }

    void Start()
    {
        parent = transform.parent;
        changeAspect = GetComponent<ChangeAspect>();
        if (spawnSpot.position == Vector3.zero)
            spawnSpot.position = transform.position + new Vector3(1, 0, 0);

        spawnSpot.position = new Vector2((float)Math.Floor(spawnSpot.position.x) + 0.5f, (float)Math.Floor(spawnSpot.position.y) + 0.5f);
	}

    void Update ()
    {
        if (changeAspect.isBurnt)
            return;
        countdown -= Time.deltaTime;
        if (countdown <= 0)
        {
            countdown = timer;
            if (UnityEngine.Random.value < spawnDropProb)
            {
                Instantiate(lightningBurstPrefab, spawnSpot.position, Quaternion.identity);
                GameObject goRef = Instantiate(superdropPrefab, spawnSpot.position, Quaternion.identity);
                goRef.transform.parent = parent;
            }
        }
    }
}
