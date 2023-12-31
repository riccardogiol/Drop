using System;
using UnityEngine;

public class PickWaterdrop : MonoBehaviour
{
    public bool randomEnergy = true;
    public int energy = 6;

    readonly int[] energyValues = {2, 4, 6};
    readonly int maxEnergy = 6;
    SpriteChangingOnValue spriteChanger;


    void Awake()
    {
        spriteChanger = GetComponent<SpriteChangingOnValue>();
        if (randomEnergy)
        {
            int randomIndex = UnityEngine.Random.Range(0, energyValues.Length-1);
            energy = energyValues[randomIndex];
        }
        ScaleOnEnergy();
    }

    public void ScaleOnEnergy()
    {
        spriteChanger.Evaluate(energy);
    }

    public void RechargeEnergy(int energyIncome)
    {
        energy = Math.Min(maxEnergy, energy + energyIncome);
        ScaleOnEnergy();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
        case "Player":
            other.GetComponent<PlayerHealth>().FillReservoir(energy);
            FindObjectOfType<AudioManager>().Play("PickWater");
            Destroy(gameObject);
            break;
        case "Enemy":
            other.GetComponent<EnemyHealth>().TakeDamage(energy);
            Destroy(gameObject);
            break ;
        case "Flame":
            int otherEnergy = other.GetComponent<PickFlame>().energy;
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
            if (other.GetComponent<PickWaterdrop>().energy >= energy)
            {
                other.GetComponent<PickWaterdrop>().RechargeEnergy(energy);
                Destroy(gameObject);
            }
            break;
        case "Grass":
            FindObjectOfType<PlaygroundManager>().WaterOnPosition(transform.position);
            break;
        case "Wall":
            FindObjectOfType<PlaygroundManager>().WaterOnPosition(transform.position);
            // add some visual effect anyway
            Destroy(gameObject);
            break;
        case "Decoration":
            Destroy(gameObject);
            break;
        }
    }
}
