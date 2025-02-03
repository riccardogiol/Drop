using UnityEngine;

public class SmokyCloudSpawner : MonoBehaviour
{
    public GameObject cloudySmokePrefab;
    void Start()
    {
        foreach(Transform child in transform)
            SpawnSmokyCloud(child);
    }

    void SpawnSmokyCloud(Transform goPos)
    {
        GameObject goRef = Instantiate(cloudySmokePrefab, goPos.position, Quaternion.identity);
        goRef.transform.parent = goPos;
    }

}
