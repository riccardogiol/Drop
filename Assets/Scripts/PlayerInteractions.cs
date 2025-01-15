using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    PlaygroundManager playground;

    void Start()
    {
        playground = FindObjectOfType<PlaygroundManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Flame":
                int flameEnergy = other.GetComponent<PickFlame>().energy;
                gameObject.GetComponent<PlayerHealth>().TakeDamage(flameEnergy);
                other.GetComponent<PickFlame>().DestroyFlame();
                break;
            case "Enemy":
                int enemyHealth = other.GetComponent<EnemyHealth>().currentHealth;
                int playerHealth = gameObject.GetComponent<PlayerHealth>().currentHealth;
                other.GetComponent<EnemyHealth>().TakeDamage(playerHealth);
                gameObject.GetComponent<PlayerHealth>().TakeDamage(enemyHealth);
                break;
            case "Grass":
                playground.WaterOnPosition(other.transform.position);
                break;
        }
    }
}
