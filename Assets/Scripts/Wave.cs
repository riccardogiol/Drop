using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    public PlaygroundManager playgroundManager;
    public int damage = 2;

    public GameObject waveExplosion;

    Collider2D waveCollider;
    public bool shootByPlayer = true;
    float timer = 0.9f;
    float tileColliderStop = 0.1f;

    public List<int> touchedIDs = new List<int>();

    void Start()
    {
        tileColliderStop = timer - tileColliderStop;
        if (waveExplosion != null)
        {
            GameObject goRef = Instantiate(waveExplosion, transform.position, transform.rotation);
            goRef.transform.parent = transform;
        }
        waveCollider = GetComponent<CircleCollider2D>();
        if (waveCollider == null)
            waveCollider = GetComponent<PolygonCollider2D>();
    }

    public void SubscribeID(int ID)
    {
        touchedIDs.Add(ID);
    }
    
    void Update()
    {
        if (timer<0)
            waveCollider.enabled = false;
        else
            timer -= Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (touchedIDs.Contains(other.gameObject.GetInstanceID()))
            return;
        if (other.CompareTag("Player"))
            if (!shootByPlayer)
                other.GetComponent<PlayerHealth>().FillReservoir(damage);
        if (other.CompareTag("Wall") && timer > tileColliderStop)
            playgroundManager.WaterOnPosition(other.transform.position);
        if (other.CompareTag("Grass") && timer > tileColliderStop)
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
            {
                other.GetComponent<RiverWave>().TriggerWave(shootByPlayer);
                other.GetComponent<RiverWave>().touchedIDs = touchedIDs;
            }
            
        }
        touchedIDs.Add(other.gameObject.GetInstanceID());
    }

}