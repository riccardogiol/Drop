using UnityEngine;

public class Wave : MonoBehaviour
{
    public PlaygroundManager playgroundManager;
    public int damage = 2;

    public GameObject waveExplosion;

    Collider2D collider2D;
    public bool shootByPlayer = true;
    float timer = 0.1f;

    void Start()
    {
        GameObject goRef = Instantiate(waveExplosion, transform.position, transform.rotation);
        goRef.transform.parent = transform;
        collider2D = GetComponent<CircleCollider2D>();
        if (collider2D == null)
            collider2D = GetComponent<PolygonCollider2D>();
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
        if (other.CompareTag("Player"))
            if (!shootByPlayer)
            {
                other.GetComponent<PlayerHealth>().FillReservoir(damage);
                Debug.Log(damage);
            }
        if (other.CompareTag("Wall"))
            playgroundManager.WaterOnPosition(other.transform.position);
        if (other.CompareTag("Grass"))
            playgroundManager.WaterOnPosition(other.transform.position);
        if (other.CompareTag("Enemy"))
            other.GetComponent<EnemyHealth>().TakeDamage(damage);
        if (other.CompareTag("Flame"))
        {
            int otherEnergy = other.GetComponent<PickFlame>().energy;
            if (otherEnergy <= damage)
                other.GetComponent<PickFlame>().DestroyFlame();
            else {
                other.GetComponent<PickFlame>().energy -= damage;
                other.GetComponent<PickFlame>().ScaleOnEnergy();
            }
        }
        if (other.CompareTag("Waterbomb"))
            other.GetComponent<PickWaterBomb>().TriggerBomb();
        if (other.CompareTag("Waterdrop"))
            other.GetComponent<PickWaterdrop>().RechargeEnergy(damage);
        if (other.CompareTag("DecorationNoFire"))
        {
            if (other.GetComponent<SparklerCharge>() != null)
                other.GetComponent<SparklerCharge>().FillReservoir(damage);
            if (other.GetComponent<RiverWave>() != null)
                other.GetComponent<RiverWave>().TriggerWave(shootByPlayer);
            
        }

    }

}