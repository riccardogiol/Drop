using System.Collections;
using UnityEngine;

public class FireBulletRicochet : MonoBehaviour
{
    public PlaygroundManager playgroundManager;
    TargetRicochet targetRicochet;
    public int shootingEnemyID;
    public int damage = 4;

    public ParticleSystem trailParticles;
    public GameObject explosionEffect;
    public GameObject vaporEffect;

    Rigidbody2D rb2D;
    Collider2D col2D;
    public SpriteRenderer spriteRenderer;

    Vector3 otherPosition;
    bool delayedEffect = false;

    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        col2D = GetComponent<BoxCollider2D>();
        targetRicochet = GetComponent<TargetRicochet>();
    }

    void Start()
    {
        if (playgroundManager == null)
            playgroundManager = FindFirstObjectByType<PlaygroundManager>();
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
            case "Player":
                other.GetComponent<PlayerHealth>().TakeDamage(damage);
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
                }
                else if (other.GetComponent<RootTriggerLogic>() != null)
                {
                    if (other.GetComponent<RootTriggerLogic>().reactOnWater)
                    {
                        playgroundManager.FireOnPosition(other.transform.position);
                        other.GetComponent<RootTriggerLogic>().SetBurntSprite();
                    }
                    if (!other.GetComponent<RootTriggerLogic>().tall)
                        break;
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
            case "WaterBullet":
                other.GetComponent<Bullet>().DestroyBullet();
                DestroyBullet(true);
                break;
            case "Wave":
                DestroyBullet(true);
                break;
        }
    }

    void DestroyBullet(bool playSmokeEffect = false)
    {
        Destroy(targetRicochet.target.gameObject);
        targetRicochet.enabled = false;
        FindObjectOfType<AudioManager>().Play("BulletExplosion");
        Instantiate(explosionEffect, transform.position, transform.rotation);
        if (playSmokeEffect)
            Instantiate(vaporEffect, transform.position, transform.rotation);
        spriteRenderer.enabled = false;
        rb2D.velocity = new Vector2(0f, 0f);
        col2D.enabled = false;
        trailParticles.Stop();
        trailParticles.GetComponent<TrailRenderer>().enabled = false;
        StartCoroutine(DelayDestroy());
    }

    IEnumerator DelayDestroy()
    {
        yield return new WaitForSeconds(0.01f);
        if (delayedEffect)
            playgroundManager.FireOnPosition(otherPosition);
        yield return new WaitForSeconds(2.8f);
        Destroy(transform.parent.gameObject);
    }
}

