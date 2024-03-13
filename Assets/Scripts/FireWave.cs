using UnityEngine;

public class FireWave : MonoBehaviour
{
    public PlaygroundManager playgroundManager;
    public int damage = 2;

    public GameObject waveExplosion;

    Collider2D collider2D;
    public bool shootByEnemy = true;
    float timer = 0.1f;

    void Start()
    {
        Instantiate(waveExplosion, transform.position, Quaternion.identity);
        collider2D = GetComponent<CircleCollider2D>();
    }
    
    void Update()
    {
        if (timer<0)
        {
            collider2D.enabled = false;
        } else {
            timer -= Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
            if (!shootByEnemy)
                other.GetComponent<EnemyHealth>().FillReservoir(damage);
        if (other.CompareTag("Wall"))
            playgroundManager.FireOnPosition(other.transform.position);
        if (other.CompareTag("Grass"))
            playgroundManager.FireOnPosition(other.transform.position);
        if (other.CompareTag("Player"))
            other.GetComponent<PlayerHealth>().TakeDamage(damage);
        if (other.CompareTag("Waterdrop"))
        {
            int otherEnergy = other.GetComponent<PickWaterdrop>().energy;
            if (otherEnergy <= damage)
                other.GetComponent<PickWaterdrop>().DestroyWaterdrop();
            else {
                other.GetComponent<PickWaterdrop>().energy -= damage;
                other.GetComponent<PickWaterdrop>().ScaleOnEnergy();
            }
        }
        if (other.CompareTag("Waterbomb"))
            other.GetComponent<PickWaterBomb>().TriggerBomb();
        if (other.CompareTag("Flame"))
            other.GetComponent<PickFlame>().RechargeEnergy(damage);
        if (other.CompareTag("WaterBullet"))
            other.GetComponent<Bullet>().DestroyBullet();

    }

}