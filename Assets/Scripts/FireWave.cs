using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWave : MonoBehaviour
{
    public PlaygroundManager playgroundManager;
    public int damage = 2;

    public GameObject waveExplosion;
    public GameObject flamePrefab;

    Collider2D waveCollider;
    public int shootByID;
    public float timer = 0.9f;
    float tileColliderStop = 0.1f;
    public float delayForCollider = 0.0f;

    public float spawnFlameProb = 0;

    public List<int> touchedIDs = new List<int>();

    void Awake()
    {
        tileColliderStop = timer - tileColliderStop;
        timer += delayForCollider;
        waveCollider = GetComponent<CircleCollider2D>();
        if (waveCollider == null)
            waveCollider = GetComponent<PolygonCollider2D>();
        if (delayForCollider > 0.0f)
        {
            waveCollider.enabled = false;
            StartCoroutine(delayedCollider());
        }

    }

    void Start()
    {
        GameObject goRef = Instantiate(waveExplosion, transform.position, transform.rotation);
        goRef.transform.parent = transform;

        touchedIDs.Add(shootByID);

        if (Random.value < spawnFlameProb)
        {
            Vector3 randomPos = transform.position + new Vector3(Random.Range(-1, 2), Random.Range(-1, 2));
            if (waveCollider.bounds.Contains(randomPos) && !playgroundManager.IsObstacleForFlame(randomPos))
            {
                GameObject goRef2 = Instantiate(flamePrefab, randomPos, Quaternion.identity);
                goRef2.GetComponent<PickFlame>().randomEnergy = false;
                goRef2.GetComponent<PickFlame>().energy = 1;
                goRef2.GetComponent<PickFlame>().ScaleOnEnergy();
                touchedIDs.Add(goRef2.GetInstanceID());
                playgroundManager.SubscribeFlame(goRef2);
            }
        }
    }

    void Update()
    {
        if (timer < 0)
            waveCollider.enabled = false;
        else
            timer -= Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (touchedIDs.Contains(other.gameObject.GetInstanceID()))
            return;
        if (other.CompareTag("Enemy") && other.gameObject.GetInstanceID() != shootByID)
            other.GetComponent<EnemyHealth>().FillReservoir(damage);
        if (other.CompareTag("Wall") && timer > tileColliderStop)
            playgroundManager.FireOnPosition(other.transform.position);
        if (other.CompareTag("Grass") && timer > tileColliderStop)
            playgroundManager.FireOnPosition(other.transform.position);
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealth>().TakeDamage(damage);
            touchedIDs.Add(other.gameObject.GetInstanceID());
        }
        if (other.CompareTag("Waterdrop"))
        {
            int otherEnergy = other.GetComponent<PickWaterdrop>().energy;
            if (otherEnergy <= damage)
                other.GetComponent<PickWaterdrop>().DestroyWaterdrop();
            else
            {
                other.GetComponent<PickWaterdrop>().energy -= damage;
                other.GetComponent<PickWaterdrop>().ScaleOnEnergy();
            }
        }
        if (other.CompareTag("Waterbomb"))
            other.GetComponent<PickWaterBomb>().TriggerBomb();
        if (other.CompareTag("Flame"))
        {
            other.GetComponent<PickFlame>().RechargeEnergy(2);
            touchedIDs.Add(other.gameObject.GetInstanceID());
        }
        if (other.CompareTag("WaterBullet"))
            other.GetComponent<Bullet>().DestroyBullet(true);
        if (other.CompareTag("Decoration"))
        {
            if (other.GetComponent<ChangeAspect>() != null)
            {
                if (other.GetComponent<ChangeAspect>().reactOnWater)
                {
                    playgroundManager.FireOnPosition(other.transform.position);
                    other.GetComponent<ChangeAspect>().SetBurntSprite();
                }
            }
            else if (other.GetComponent<RootTriggerLogic>() != null)
            {
                if (other.GetComponent<RootTriggerLogic>().reactOnWater)
                {
                    playgroundManager.FireOnPosition(other.transform.position);
                    other.GetComponent<RootTriggerLogic>().SetBurntSprite();
                }
            }
        }
    }

    IEnumerator delayedCollider()
    {
        yield return new WaitForSeconds(delayForCollider);
        waveCollider.enabled = true;
    }

}