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
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealth>().TakeDamage((int)energy);
            Destroy(gameObject);
            // add particle effect
        }
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyHealth>().FillReservoir((int)energy);
            Destroy(gameObject);
            // add particle effect
        }
        if (other.CompareTag("Flame"))
        {
            if (other.GetComponent<PickFlame>().energy>energy)
            {
                Destroy(gameObject);
            }
        }
        // add simmetry as for waterdrop if we want flames to be thrown
    }
}
