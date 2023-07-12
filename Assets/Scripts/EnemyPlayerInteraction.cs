using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlayerInteraction : MonoBehaviour
{
    public GameObject player;

    void Update()
    {
        if (Vector2.Distance(player.transform.position, transform.position) < 1)
        {
            player.GetComponent<PlayerHealth>().TakeDamage(GetComponent<EnemyHealth>().currentHealth);
            //play animation and sound
            Destroy(gameObject);
        }
    }
}
