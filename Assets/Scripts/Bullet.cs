using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.name);
        PlaygroundManager pgm = other.GetComponentInParent<PlaygroundManager>();
        if (pgm != null)
        {
            Vector3 inCellPos = other.ClosestPoint(transform.position);
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            pgm.WaterCollision(inCellPos, rb.velocity);
        }
        Destroy(gameObject);
    }
}
