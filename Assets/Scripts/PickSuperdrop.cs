using System;
using UnityEngine;

public class PickSuperdrop : MonoBehaviour
{
    public bool randomEnergy = false;
    public int energy = 3;

    readonly int[] energyValues = {1, 2, 3};
    readonly int maxEnergy = 3;
    //SpriteChangingOnValue spriteChanger;

    public GameObject takeWaterBurstPrefab;
    public GameObject vaporBurstPrefab;

    void Awake()
    {
        //spriteChanger = GetComponent<SpriteChangingOnValue>();
        if (randomEnergy)
        {
            int randomIndex = UnityEngine.Random.Range(0, energyValues.Length-1);
            energy = energyValues[randomIndex];
        }
        ScaleOnEnergy();
    }

    public void ScaleOnEnergy()
    {
        //spriteChanger.Evaluate(energy);
    }

    public void RechargeEnergy(int energyIncome)
    {
        energy = Math.Min(maxEnergy, energy + energyIncome);
        if (energyIncome > 0)
            PlayWaterBurst();
        else
            PlayVaporBurst();
        ScaleOnEnergy();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
        case "Player":
            other.GetComponent<PlayerSuperPower>().Recharge(energy);
            FindObjectOfType<AudioManager>().Play("PickWater");
            PlayWaterBurst();
            DestroyWaterdrop();
            break;
        /*
        case "Enemy":
            other.GetComponent<EnemyHealth>().TakeDamage(energy);
            PlayVaporBurst();
            DestroyWaterdrop();
            break ;
        case "Flame":
            int otherEnergy = other.GetComponent<PickFlame>().energy;
            if (otherEnergy < energy)
            {
                RechargeEnergy(-otherEnergy);
                other.GetComponent<PickFlame>().DestroyFlame();
            } else {
                other.GetComponent<PickFlame>().energy -= energy;
                other.GetComponent<PickFlame>().ScaleOnEnergy();
                PlayVaporBurst();
                DestroyWaterdrop();
            }
            break;
        case "Waterdrop":
            if (other.GetComponent<PickWaterdrop>().energy >= energy)
            {
                other.GetComponent<PickWaterdrop>().RechargeEnergy(energy);
                DestroyWaterdrop();
            }
            break;
        case "Grass":
            FindObjectOfType<PlaygroundManager>().WaterOnPosition(transform.position);
            break;
        case "Wall":
            FindObjectOfType<PlaygroundManager>().WaterOnPosition(transform.position);
            PlayWaterBurst();
            DestroyWaterdrop();
            break;
        case "Decoration":
            PlayWaterBurst();
            DestroyWaterdrop();
            break;
        */
        }
    }

    public void PlayWaterBurst()
    {
        Instantiate(takeWaterBurstPrefab, transform.position, Quaternion.identity);
    }

    public void PlayVaporBurst()
    {
        Instantiate(vaporBurstPrefab, transform.position, Quaternion.identity);
    }

    public void DestroyWaterdrop()
    {
        Destroy(gameObject);
    }
}
