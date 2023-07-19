using UnityEngine;

public class PickWaterdrop : MonoBehaviour
{
    public float energy = 10;
    public float maxEnergy = 20f;
    public Transform waterdropGFX;


    void Awake()
    {
        energy = Random.Range(energy, maxEnergy);
        ScaleOnEnergy();
    }

    public void ScaleOnEnergy()
    {
        //gradient and different images
        waterdropGFX.localScale = new Vector3(energy/maxEnergy, energy/maxEnergy, 1);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealth>().FillReservoir((int)energy);
            Destroy(gameObject);
            // add particle effect
        }
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyHealth>().TakeDamage((int)energy);
            Destroy(gameObject);
            // add particle effect
        }
        if (other.CompareTag("Flame"))
        {
            // check energy difference
            float otherEnergy = other.GetComponent<PickFlame>().energy;
            if (otherEnergy < energy)
            {
                energy -= otherEnergy;
                ScaleOnEnergy();
                Destroy(other.gameObject);
                // show particle effect
            } else {
                other.GetComponent<PickFlame>().energy -= energy;
                other.GetComponent<PickFlame>().ScaleOnEnergy();
                Destroy(gameObject);
                // show particle effect
            }
            // update size on one or othe rside
        }
        if (other.CompareTag("Waterdrop"))
        {
            if (other.GetComponent<PickWaterdrop>().energy > energy)
            {
                // regulate size on other side
                Destroy(gameObject);
            }
        }
    }
}
