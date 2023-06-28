using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickWaterDrop : MonoBehaviour
{
    public int waterValue = 10;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealth>().FillReservoir(waterValue);
            Destroy(gameObject);
            // add particle effect
        }
        if (other.CompareTag("Flame"))
        {
            // check energy difference
            Destroy(other.gameObject);
        }
    }
}
