using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public PlaygroundManager playgroundManager;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wall"))
            playgroundManager.WaterOnPosition(other.transform.position);
        Destroy(gameObject);
    }
}
