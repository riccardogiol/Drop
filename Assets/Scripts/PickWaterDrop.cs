using UnityEngine;

public class PickWaterdrop : MonoBehaviour
{
    public bool randomEnergy = true;
    public float energy = 5;
    public float maxEnergy = 20f;
    public SpriteChangingOnValue spriteChanger;


    void Awake()
    {
        if (randomEnergy)
            energy = Random.Range(energy, maxEnergy);
        ScaleOnEnergy();
    }

    public void ScaleOnEnergy()
    {
        spriteChanger.Evaluate(energy);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
        case "Player":
            other.GetComponent<PlayerHealth>().FillReservoir((int)energy);
            FindObjectOfType<AudioManager>().Play("PickWater");
            Destroy(gameObject);
            break;
        case "Enemy":
            other.GetComponent<EnemyHealth>().TakeDamage((int)energy);
            Destroy(gameObject);
            break ;
        case "Flame":
            float otherEnergy = other.GetComponent<PickFlame>().energy;
            if (otherEnergy < energy)
            {
                energy -= otherEnergy;
                ScaleOnEnergy();
                other.GetComponent<PickFlame>().DestroyFlame();
            } else {
                other.GetComponent<PickFlame>().energy -= energy;
                other.GetComponent<PickFlame>().ScaleOnEnergy();
                Destroy(gameObject);
            }
            break;
        case "Waterdrop":
            if (other.GetComponent<PickWaterdrop>().energy > energy)
            {
                Destroy(gameObject);
            }
            break;
        case "Grass":
            FindObjectOfType<PlaygroundManager>().WaterOnPosition(transform.position);
            break;
        }
    }
}
