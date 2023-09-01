using UnityEngine;

public class PickFlame : MonoBehaviour
{
    public float energy = 10;
    public float maxEnergy = 15f;
    public Transform flameGFX;

    void Awake()
    {
        energy = Random.Range(energy, maxEnergy);
        ScaleOnEnergy();   
    }

    public void ScaleOnEnergy()
    {
        flameGFX.localScale = new Vector3(energy/maxEnergy, energy/maxEnergy, 1);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Player":
                other.GetComponent<PlayerHealth>().TakeDamage((int)energy);
                DestroyFlame();
                break;
            case "Enemy":
                float enemyHealthDiff = other.GetComponent<EnemyHealth>().maxHealth - other.GetComponent<EnemyHealth>().currentHealth;
                other.GetComponent<EnemyHealth>().FillReservoir((int)energy);
                if (enemyHealthDiff > energy)
                {
                    DestroyFlame();
                } else {
                    energy -= enemyHealthDiff;
                    ScaleOnEnergy();
                }
                break;
            case "Flame":
                if (other.GetComponent<PickFlame>().energy>energy)
                    DestroyFlame();
                break;
        }
    }

    public void DestroyFlame()
    {
        PlaygroundManager pgRef = FindObjectOfType<PlaygroundManager>();
        if (pgRef != null)
            pgRef.FlameEstinguished();
        Destroy(gameObject);
    }
}
