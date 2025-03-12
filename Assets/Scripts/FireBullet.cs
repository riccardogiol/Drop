using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FireBullet : MonoBehaviour
{
    public PlaygroundManager playgroundManager;
    public int shootingEnemyID;
    public int damage = 2;

    public ParticleSystem trailParticles;
    public GameObject explosionEffect;

    public bool spawnFlame = false;

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
        switch (other.tag)
        {
            case "Enemy":
                if (shootingEnemyID == other.gameObject.GetInstanceID())
                    return;
                other.GetComponent<EnemyHealth>().FillReservoir(damage);
                DestroyBullet();
                break;
            case "Grass":
                playgroundManager.FireOnPosition(other.transform.position);
                return;
            case "Player":
                other.GetComponent<PlayerHealth>().TakeDamage(damage);
                spawnFlame = false;
                DestroyBullet();
                break;
            case "Wall":
                playgroundManager.FireOnPosition(other.transform.position);
                DestroyBullet();
                break;
            case "Decoration":
                if (other.GetComponent<ChangeAspect>() != null)
                {
                    if (other.GetComponent<ChangeAspect>().reactOnWater)
                    {
                        playgroundManager.FireOnPosition(other.transform.position);
                        other.GetComponent<ChangeAspect>().SetBurntSprite();
                    }
                } else if (other.GetComponent<RootTriggerLogic>() != null)
                {
                    if (other.GetComponent<RootTriggerLogic>().reactOnWater)
                    {
                        playgroundManager.FireOnPosition(other.transform.position);
                        other.GetComponent<RootTriggerLogic>().SetBurntSprite();
                    }
                }
                DestroyBullet();
                break;
            case "DecorationNoFire":
                if (other.GetComponent<RiverWave>() != null)
                    break;
                DestroyBullet();
                break;
            case "Insect":
                playgroundManager.FireOnPosition(other.transform.position);
                if (other.GetComponent<ChangeAspect>() != null)
                {
                    if (other.GetComponent<ChangeAspect>().reactOnWater)
                        other.GetComponent<ChangeAspect>().SetBurntSprite();
                } else if (other.GetComponent<RootTriggerLogic>() != null)
                {
                    if (other.GetComponent<RootTriggerLogic>().reactOnWater)
                        other.GetComponent<RootTriggerLogic>().SetBurntSprite();
                }
                DestroyBullet();
                break;
            case "Waterdrop":
                int otherEnergy = other.GetComponent<PickWaterdrop>().energy;
                if (otherEnergy <= damage)
                    other.GetComponent<PickWaterdrop>().DestroyWaterdrop();
                else {
                    other.GetComponent<PickWaterdrop>().energy -= damage;
                    other.GetComponent<PickWaterdrop>().ScaleOnEnergy();
                    spawnFlame = false;
                }
                otherPosition = other.transform.position;
                delayedEffect = true;
                DestroyBullet();
                break;
            case "WaterBullet":
                other.GetComponent<Bullet>().DestroyBullet();
                spawnFlame = false;
                DestroyBullet();
                break;
            case "Wave":
                DestroyBullet();
                spawnFlame = false;
                break;
            case "Flame":
                if (other.GetComponent<PickFlame>().energy >= 5)
                    break;
                other.GetComponent<PickFlame>().RechargeEnergy(2);
                spawnFlame = false;
                DestroyBullet();
                break;
            case "Waterbomb":
                other.GetComponent<PickWaterBomb>().TriggerBomb();
                DestroyBullet();
                break;
        }
    }

    void DestroyBullet()
    {
        FindObjectOfType<AudioManager>().Play("BulletExplosion");
        Instantiate(explosionEffect, transform.position, transform.rotation);
        spriteRenderer.enabled = false;
        rigidbody2D.velocity = new Vector2(0f, 0f);
        collider2D.enabled = false;
        trailParticles.Stop();
        if (spawnFlame)
            playgroundManager.AddFlame(FindFirstObjectByType<Tilemap>().WorldToCell(transform.position), 1, true, false);
        StartCoroutine(DelayDestroy());
    }

    IEnumerator DelayDestroy()
    {
        yield return new WaitForSeconds(0.01f);
        if (delayedEffect)
            playgroundManager.FireOnPosition(otherPosition);
        yield return new WaitForSeconds(2.8f);
        Destroy(gameObject);
    }
}

