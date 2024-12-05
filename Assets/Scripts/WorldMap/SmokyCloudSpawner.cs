using UnityEngine;

public class SmokyCloudSpawner : MonoBehaviour
{
    public GameObject cloudySmokePrefab;

    Vector3[] positions = {new(0f,0f,0f), new(-0.67f, 0.9f, 0f), new(-0.53f, -0.78f, 0f)};
    void Start()
    {
        foreach(Vector3 pos in positions)
        {
            SpawnSmokyCloud(pos);
        }
        
    }

    void SpawnSmokyCloud(Vector3 position)
    {
        GameObject goRef = Instantiate(cloudySmokePrefab, transform.position + position, Quaternion.identity);
        goRef.transform.parent = transform;
        //goRef.GetComponent<SpriteChangingOnValue>().Evaluate(Random.Range(0, 3));
    }

}
