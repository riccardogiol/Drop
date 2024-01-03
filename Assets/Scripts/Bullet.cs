using UnityEngine;

public class Bullet : MonoBehaviour
{
    public PlaygroundManager playgroundManager;
    public bool shootByPlayer = true;
    public int energy = 3;
    public int damage = 5;
    public float range = 4;

    //Vector2 startingPosition;

    void Start()
    {
        //startingPosition = transform.position;
    }

    void FixedUpdate()
    {
        //if (Vector2.Distance(startingPosition, transform.position) > range)
            //DestroyBullet();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Enemy":
                other.GetComponent<EnemyHealth>().TakeDamage(damage);
                break;
            case "Grass":
                playgroundManager.WaterOnPosition(other.transform.position);
                return;
            case "Player":
                if (shootByPlayer)
                    return;
                else
                    break;
            case "Wave":
                return;
            case "OneWayCollider":
                return;
            case "Wall":
                playgroundManager.WaterOnPosition(other.transform.position);
                break;
            case "Flame":
                int otherEnergy = other.GetComponent<PickFlame>().energy;
                if (otherEnergy <= damage)
                    other.GetComponent<PickFlame>().DestroyFlame();
                else {
                    other.GetComponent<PickFlame>().energy -= damage;
                    other.GetComponent<PickFlame>().ScaleOnEnergy();
                }
                break;
            case "Waterdrop":
                other.GetComponent<PickWaterdrop>().RechargeEnergy(energy);
                break;
            case "Waterbomb":
                other.GetComponent<PickWaterBomb>().TriggerBomb();
                break;
        }
        DestroyBullet();
    }

    void DestroyBullet()
    {
        FindObjectOfType<AudioManager>().Play("BulletExplosion");
        Destroy(gameObject);
    }
}

