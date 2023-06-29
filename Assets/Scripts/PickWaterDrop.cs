using UnityEngine;

public class PickWaterdrop : MonoBehaviour
{
    public float energy = 10;

    void Start()
    {
        energy = Random.Range(10f, 17f);
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
            Destroy(other.gameObject);
        }
        if (other.CompareTag("Waterdrop"))
        {
            if (other.GetComponent<PickWaterdrop>().energy > energy)
            {
                Destroy(gameObject);
            }
        }
    }
}
