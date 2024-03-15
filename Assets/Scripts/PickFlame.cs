using UnityEngine;
using System;

public class PickFlame : MonoBehaviour
{
    public bool randomEnergy = true;
    public int energy = 3;

    readonly int[] energyValues = { 1, 3, 5 };
    public readonly int maxEnergy = 5;
    SpriteChangingOnValue spriteChanger;

    public GameObject vaporBurstPrefab;

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
        case "Enemy":
            other.GetComponent<EnemyHealth>().FillReservoir(energy);
            DestroyFlame();
            break;
        case "Flame":
            if (other.GetComponent<PickFlame>().energy == energy)
            {
                if (energy == maxEnergy)
                    break;
                energy++;
            }
            if (other.GetComponent<PickFlame>().energy > energy)
            {
                other.GetComponent<PickFlame>().RechargeEnergy(energy);
                DestroyFlame();
            }
            break;
        case "Grass":
            FindObjectOfType<PlaygroundManager>().FireOnPosition(other.transform.position);
            break;
        case "Decoration":
            DestroyFlame();
            break;
        case "Waterbomb":
            int bombEnergy = other.GetComponent<PickWaterBomb>().bombEnergy;
            if (bombEnergy >= energy)
                DestroyFlame();
            else
            {
                energy -= bombEnergy;
                ScaleOnEnergy();
            }
            other.GetComponent<PickWaterBomb>().DestroyBomb();
            break;
        }
    }

    public void DestroyFlame()
    {
        GetComponent<Collider2D>().enabled = false;
        PlaygroundManager pgRef = FindObjectOfType<PlaygroundManager>();
        if (pgRef != null)
        {
            pgRef.FlameEstinguished();
            pgRef.WaterOnPosition(transform.position);
        }
        
        Instantiate(vaporBurstPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
