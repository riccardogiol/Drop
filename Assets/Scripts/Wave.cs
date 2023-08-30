using UnityEngine;

public class Wave : MonoBehaviour
{
    public PlaygroundManager playgroundManager;
    public float damage = 5;

    public float timer = 1;

    void Start()
    {
        playgroundManager.WaveOnPosition(transform.position);
    }

    void Update()
    {
        if (timer<0)
        {
            Destroy(gameObject);
        } else {
            timer -= Time.deltaTime;
        }
    }

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
    }
}

