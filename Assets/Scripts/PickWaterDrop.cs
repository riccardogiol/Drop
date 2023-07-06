using UnityEngine;

public class PickWaterdrop : MonoBehaviour
{
    public float energy = 10;

    void Start()
    {
        energy = Random.Range(10f, 17f);
        // regulate size in function of the energy
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealth>().FillReservoir((int)energy);
            Destroy(gameObject);
            // add particle effect
        }
        if (other.CompareTag("Flame"))
        {
            // check energy difference
            // update size on one or othe rside
            Destroy(other.gameObject);
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
