using UnityEngine;

public class PickFlame : MonoBehaviour
{
    public float energy = 10;

    void Start()
    {
        energy = Random.Range(7f, 15f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealth>().TakeDamage((int)energy);
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
    }
}
