using System.Collections;
using UnityEngine;

public class EnemyGroundInteraction : MonoBehaviour
{
    public float checkingInterval = 0.5f;
    public float spawnFlameProbability = 0.5f;
    float timer = 0;

    PlaygroundManager playground;
    Vector3 oldPosition;

    void Awake()
    {
        oldPosition = transform.position;
        playground = FindFirstObjectByType<PlaygroundManager>();

    }

    void Start()
    {
        StartCoroutine(NewPosition());
    }

    void Update()
    {
        if (timer >= 0)
           timer -= Time.deltaTime;
    }
    
    IEnumerator NewPosition()
    {
        while(true)
        {
            if ((transform.position - oldPosition).magnitude > 0.9f)
            {
                if (Random.value < spawnFlameProbability)
                {
                    playground.FlameOnPosition(oldPosition);
                }
                oldPosition = transform.position;
            }
            yield return new WaitForSeconds(checkingInterval);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Grass"))
        {
            if (timer <= 0)
            {
                playground.FireOnPosition(other.transform.position);
                timer = checkingInterval;
            }
        }
    }
}
