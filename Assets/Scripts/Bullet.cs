using UnityEngine;

public class Bullet : MonoBehaviour
{
    public PlaygroundManager playgroundManager;
    public float energy = 5;
    public float damage = 15;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            return;
        if (other.CompareTag("Wall"))
            playgroundManager.WaterOnPosition(other.transform.position);
        if (other.CompareTag("Enemy"))
            other.GetComponent<EnemyHealth>().TakeDamage((int)damage);
        if (other.CompareTag("Flame"))
        {
            float otherEnergy = other.GetComponent<PickFlame>().energy;
            if (otherEnergy < damage)
                Destroy(other.gameObject);
            else {
                other.GetComponent<PickFlame>().energy -= damage;
                other.GetComponent<PickFlame>().ScaleOnEnergy();
            }
        }
        Destroy(gameObject);
    }
}

