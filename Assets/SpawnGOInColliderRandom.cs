using UnityEngine;

public class SpawnGOInColliderRandom : MonoBehaviour
{
    Collider2D col;
    Bounds collBounds;

    public GameObject GOPrefab;
    public float timer;
    float countdown;
    public bool randomCountdown;

    void Awake()
    {
        col = GetComponent<Collider2D>();
        collBounds = col.bounds;
        if (randomCountdown)
            countdown = UnityEngine.Random.Range(timer*0.1f, timer*1.9f);
        else
            countdown = timer;
    }

    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0)
        {
            if (randomCountdown)
                countdown = UnityEngine.Random.Range(timer*0.1f, timer*1.9f);
            else
                countdown = timer;
            Vector3 spawningPoint = GetRandomPointInsideCollider();
            GameObject goRef = Instantiate(GOPrefab, spawningPoint, Quaternion.identity);
            goRef.transform.parent = transform;
        }
    }
    
    public Vector3 GetRandomPointInsideCollider()
    {
        Vector3 point;
        do
        {
            point = new Vector3( UnityEngine.Random.Range(collBounds.min.x, collBounds.max.x), UnityEngine.Random.Range(collBounds.min.y, collBounds.max.y), 0);
        } while (!col.ClosestPoint(point).Equals(point));
        return point;
    }
}
