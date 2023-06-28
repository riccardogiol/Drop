using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickFlame : MonoBehaviour
{
    public float fireDamage = 10;

    void Start()
    {
        fireDamage = Random.Range(7f, 15f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealth>().TakeDamage((int)fireDamage);
            Destroy(gameObject);
            // add particle effect
        }
        if (other.CompareTag("Flame"))
        {
            if (other.GetComponent<PickFlame>().fireDamage>fireDamage)
            {
                Debug.Log("flame on flame");
                Destroy(gameObject);
            }
        }
    }
}
