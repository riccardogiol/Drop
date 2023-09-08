using UnityEngine;

public class Bullet : MonoBehaviour
{
    public PlaygroundManager playgroundManager;
    public float energy = 5;
    public float damage = 10;

    void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Enemy":
                other.GetComponent<EnemyHealth>().TakeDamage((int)damage);
                break;
            case "Grass":
                playgroundManager.WaterOnPosition(other.transform.position);
                return;
            case "Player":
                return;
            case "Wave":
                return;
            case "Wall":
                playgroundManager.WaterOnPosition(other.transform.position);
                break;
            case "Flame":
                float otherEnergy = other.GetComponent<PickFlame>().energy;
                if (otherEnergy < damage)
                    other.GetComponent<PickFlame>().DestroyFlame();
                else {
                    other.GetComponent<PickFlame>().energy -= damage;
                    other.GetComponent<PickFlame>().ScaleOnEnergy();
                }
                break;
        }
        FindObjectOfType<AudioManager>().Play("BulletExplosion");
        Destroy(gameObject);
    }
}

