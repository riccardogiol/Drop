using System;
using UnityEngine;

public class PickSuperdrop : MonoBehaviour
{
    public bool randomEnergy = false;
    public int energy = 3;

    readonly int[] energyValues = {1, 2, 3};
    readonly int maxEnergy = 3;
    SpriteChangingOnValue spriteChanger;

    public GameObject takeLightningBurstPrefab;

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
        PlayLightningBurst();
        ScaleOnEnergy();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
        case "Player":
            other.GetComponent<PlayerSuperPower>().Recharge(energy);
            FindObjectOfType<AudioManager>().Play("PickWater");
            PlayLightningBurst();
            DestroySuperdrop();
            break;
        case "Enemy":
            other.GetComponent<EnemyHealth>().FillReservoir(energy);
            DestroySuperdrop();
            break ;
        case "Flame":
            other.GetComponent<PickFlame>().RechargeEnergy(energy);
            other.GetComponent<PickFlame>().ScaleOnEnergy();
            DestroySuperdrop();
            break;
        case "Waterdrop":
            other.GetComponent<PickWaterdrop>().DestroyWaterdrop();
            break;
        case "Superdrop":
            if (other.GetComponent<PickSuperdrop>().energy >= energy)
            {
                other.GetComponent<PickSuperdrop>().RechargeEnergy(energy);
                DestroySuperdrop();
            }
            break;
        case "Wall":
            DestroySuperdrop();
            break;
        case "Decoration":
            DestroySuperdrop();
            break;
        }
    }

    public void PlayLightningBurst()
    {
        Instantiate(takeLightningBurstPrefab, transform.position, Quaternion.identity);
    }

    public void DestroySuperdrop()
    {
        Destroy(gameObject);
    }
}
