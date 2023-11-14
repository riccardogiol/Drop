using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    public PlaygroundManager playground;

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
                other.GetComponent<EnemyHealth>().TakeDamage(enemyHealth);
                gameObject.GetComponent<PlayerHealth>().TakeDamage(enemyHealth);
                break;
            case "Grass":
                int damage = playground.WaterOnPosition(other.transform.position);
                if (damage > 0)
                    GetComponent<PlayerHealth>().TakeDamage(damage);
                break;
        }
    }
}
