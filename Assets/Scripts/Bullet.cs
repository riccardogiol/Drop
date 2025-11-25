using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public PlaygroundManager playgroundManager;
    public bool shootByPlayer = true;
    public int energy = 3;
    public int damage = 5;

    public ParticleSystem trailParticles;
    public GameObject explosionEffect;
    public GameObject smokeEffect;

    public bool piercingShoot = false;
    int touchedCollider = 0;

    public bool castWave = false;
    public GameObject waveGO;

    Rigidbody2D rigidbody2D;
    Collider2D collider2D;
    SpriteRenderer spriteRenderer;

    Vector3 otherPosition;
    bool delayedEffect = false;

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.tag);
        switch (other.tag)
        {
            case "Enemy":
                if (touchedCollider == other.gameObject.GetInstanceID())
                    return;
                touchedCollider = other.gameObject.GetInstanceID();
                int enemyHealth = other.GetComponent<EnemyHealth>().currentHealth;
                if (enemyHealth <= damage)
                {
                    otherPosition = other.transform.position;
                    delayedEffect = true;
                }
                other.GetComponent<EnemyHealth>().TakeDamage(damage);
                DestroyBullet(false, piercingShoot);
                break;
            case "Grass":
                playgroundManager.WaterOnPosition(other.transform.position);
                return;
            case "Player":
                if (!shootByPlayer)
                {
                    other.GetComponent<PlayerHealth>().FillReservoir(energy);
                    DestroyBullet();
                }
                break;
            case "Wall":
                playgroundManager.WaterOnPosition(other.transform.position);
                DestroyBullet();
                break;
            case "Decoration":
                if (other.GetComponent<ChangeAspect>() != null)
                {
                    if (other.GetComponent<ChangeAspect>().reactOnWater)
                    {
                        playgroundManager.WaterOnPosition(other.transform.position);
                        other.GetComponent<ChangeAspect>().SetGreenSprite();
                    }
                } else if (other.GetComponent<RootTriggerLogic>() != null)
                {
                    if (other.GetComponent<RootTriggerLogic>().reactOnWater)
                    {
                        playgroundManager.WaterOnPosition(other.transform.position);
                        other.GetComponent<RootTriggerLogic>().SetGreenSprite();
                    }
                }
                DestroyBullet();
                break;
            case "DecorationNoFire":
                if (other.GetComponent<RiverWave>() != null)
                    break;
                if (other.GetComponent<PushTriggerAvalanche>() != null)
                    break;
                if (other.GetComponent<SparklerCharge>() != null)
                {
                    other.GetComponent<SparklerCharge>().FillReservoir(damage);
                }
                DestroyBullet();
                break;
            case "Insect":
                if (other.GetComponent<ChangeAspect>() != null)
                {
                    if (other.GetComponent<ChangeAspect>().reactOnWater)
                    {
                        playgroundManager.WaterOnPosition(other.transform.position);
                        other.GetComponent<ChangeAspect>().SetGreenSprite();
                    }
                } else if (other.GetComponent<RootTriggerLogic>() != null)
                {
                    if (other.GetComponent<RootTriggerLogic>().reactOnWater)
                    {
                        playgroundManager.WaterOnPosition(other.transform.position);
                        other.GetComponent<RootTriggerLogic>().SetGreenSprite();
                    }
                }
                DestroyBullet();
                break;
            case "Flame":
                if (touchedCollider == other.gameObject.GetInstanceID())
                    return;
                touchedCollider = other.gameObject.GetInstanceID();
                int otherEnergy = other.GetComponent<PickFlame>().energy;
                if (otherEnergy <= damage)
                    other.GetComponent<PickFlame>().DestroyFlame();
                else {
                    other.GetComponent<PickFlame>().energy -= damage;
                    other.GetComponent<PickFlame>().ScaleOnEnergy();
                }
                otherPosition = other.transform.position;
                delayedEffect = true;
                DestroyBullet(false, piercingShoot);
                break;
            case "Waterdrop":
                if (other.GetComponent<PickWaterdrop>().energy < 6)
                {
                    other.GetComponent<PickWaterdrop>().RechargeEnergy(energy);
                    DestroyBullet();
                }
                break;
            case "Waterbomb":
                other.GetComponent<PickWaterBomb>().TriggerBomb();
                DestroyBullet();
                break;
        }
    }

    public void DestroyBullet(bool playSmokeEffect = false, bool pierce = false)
    {
        FindObjectOfType<AudioManager>().Play("WaterBulletSplash", transform.position);
        Instantiate(explosionEffect, transform.position, transform.rotation);
        if (playSmokeEffect)
            Instantiate(smokeEffect, transform.position, transform.rotation);

        if (castWave)
        {
            GameObject wave = Instantiate(waveGO, transform.position, Quaternion.identity);
            wave.GetComponent<Wave>().damage = 2;
            wave.GetComponent<Wave>().playgroundManager = playgroundManager;
            wave.GetComponent<Wave>().shootByPlayer = true;
        }

        if (pierce && Random.value < 0.25f)
            return;

        spriteRenderer.enabled = false;
        rigidbody2D.velocity = new Vector2(0f, 0f);
        collider2D.enabled = false;
        trailParticles.Stop();

        StartCoroutine(DelayDestroy());
    }

    IEnumerator DelayDestroy()
    {
        yield return new WaitForSeconds(0.01f);
        if (delayedEffect)
            playgroundManager.WaterOnPosition(otherPosition);
        yield return new WaitForSeconds(2.8f);
        Destroy(gameObject);
    }
}

