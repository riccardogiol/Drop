using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSparklesSpawner : MonoBehaviour
{
    public List<GameObject> flameSparkles;
    public float timer = 1.0f;
    float countdown = 0;
    public float fromX = -1, fromY = 1, toX = 1, toY = 2;

    float xPos, yPos;

    void Start()
    {
        countdown = timer;
    }

    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0)
        {
            countdown = timer;
            SpawnPrefabs();
        }
    }

    void SpawnPrefabs() {
        xPos = Random.Range(fromX, toX);
        yPos = Random.Range(fromY, toY);
        GameObject prefRef = Instantiate(flameSparkles[Random.Range(0, flameSparkles.Count)], transform.position + new Vector3(xPos, yPos), Quaternion.identity);  
        prefRef.transform.parent = transform;   
        return;
	}
}
