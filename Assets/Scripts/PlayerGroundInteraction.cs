using Unity.VisualScripting;
using UnityEngine;

public class PlayerGroundInteraction : MonoBehaviour
{
    public PlaygroundManager playground;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Grass"))
        {
            int damage = playground.WaterOnPosition(other.transform.position);
            GetComponent<PlayerHealth>().TakeDamage(damage);
        }
    }
}
