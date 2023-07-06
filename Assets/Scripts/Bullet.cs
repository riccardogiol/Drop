using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public PlaygroundManager playgroundManager;

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.name);
        Debug.Log(other.gameObject.transform.position);
        Debug.Log(other.gameObject.transform.parent.name);
        /*
        PlaygroundManager pgm = other.GetComponentInParent<PlaygroundManager>();
        if (pgm != null)
        {
            Vector3 inCellPos = other.ClosestPoint(transform.position);
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            pgm.WaterCollision(inCellPos);
        }
        */
        if (other.transform.parent.name == "Walls")
            playgroundManager.WaterOnPosition(other.transform.position);
        Destroy(gameObject);
    }
}
