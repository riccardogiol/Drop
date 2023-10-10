using UnityEngine;
using System;

public class PickFlame : MonoBehaviour
{
    public bool randomEnergy = true;
    public int energy = 3;

    readonly int[] energyValues = { 1, 3, 5 };
    public readonly int maxEnergy = 5;
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
        /*case "Player":
            other.GetComponent<PlayerHealth>().TakeDamage((int)energy);
            Debug.Log("flame touching player");
            DestroyFlame();
            break;*/
        case "Enemy":
            int enemyHealthDiff = other.GetComponent<EnemyHealth>().maxHealth - other.GetComponent<EnemyHealth>().currentHealth;
            other.GetComponent<EnemyHealth>().FillReservoir(energy);
            if (enemyHealthDiff > energy)
            {
                DestroyFlame();
            } else {
                energy -= enemyHealthDiff;
                ScaleOnEnergy();
            }
            break;
        case "Flame":
            if (other.GetComponent<PickFlame>().energy >= energy)
            {
                other.GetComponent<PickFlame>().RechargeEnergy(energy);
                DestroyFlame();
            }
            break;
        case "Decoration":
            DestroyFlame();
            break;
        }
    }

    public void DestroyFlame()
    {
        PlaygroundManager pgRef = FindObjectOfType<PlaygroundManager>();
        if (pgRef != null)
            pgRef.FlameEstinguished();
        Destroy(gameObject);
    }
}
