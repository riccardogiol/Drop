using UnityEngine;

public class PlayerGroundInteraction : MonoBehaviour
{
    public PlaygroundManager playground;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            int enemyHealth = other.GetComponent<EnemyHealth>().currentHealth;
            other.GetComponent<EnemyHealth>().TakeDamage(enemyHealth);
            gameObject.GetComponent<PlayerHealth>().TakeDamage(enemyHealth);
            return;
        }
        if (other.CompareTag("Grass"))
        {
            int damage = playground.WaterOnPosition(other.transform.position);
            GetComponent<PlayerHealth>().TakeDamage(damage);
        }
    }
}
