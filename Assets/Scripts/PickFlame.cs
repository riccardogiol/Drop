using UnityEngine;
using System;
using System.Collections;

public class PickFlame : MonoBehaviour
{
    public bool randomEnergy = true;
    public int energy = 3;

    readonly int[] energyValues = { 1, 3, 5 };
    public readonly int maxEnergy = 5;
    SpriteChangingOnValue spriteChanger;

    public GameObject vaporBurstPrefab;
    public GameObject fireBurstPrefab;

    public bool isDecoration = false;

    bool noRecharge = true;

    void Awake()
    {
        spriteChanger = GetComponent<SpriteChangingOnValue>();
        if (randomEnergy)
        {
            int randomIndex = UnityEngine.Random.Range(0, energyValues.Length - 1);
            energy = energyValues[randomIndex];
        }
        ScaleOnEnergy();
    }

    void Start()
    {
        if (!isDecoration)
            Instantiate(fireBurstPrefab, transform.position, Quaternion.identity);
        noRecharge = true;
        StartCoroutine(DelayedRecharge(1.0f));
    }

    public void ScaleOnEnergy()
    {
        spriteChanger.Evaluate(energy);
    }

    public void RechargeEnergy(int energyIncome)
    {
        if (noRecharge)
            return;
        energy = Math.Min(maxEnergy, energy + energyIncome);
        ScaleOnEnergy();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isDecoration)
            return;
        switch (other.tag)
        {
            case "Enemy":
                if (other.GetComponent<EnemyHealth>().maxHealth == other.GetComponent<EnemyHealth>().currentHealth)
                    break;
                other.GetComponent<EnemyHealth>().FillReservoir(energy);
                DestroyFlame(false);
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
                    DestroyFlame(false);
                }
                break;
            case "Grass":
                FindObjectOfType<PlaygroundManager>().FireOnPosition(other.transform.position);
                break;
            case "Wall":
                FindObjectOfType<PlaygroundManager>().FireOnPosition(other.transform.position);
                DestroyFlame(false);
                break;
            case "MovingRock":
                FindObjectOfType<PlaygroundManager>().FireOnPosition(other.transform.position);
                DestroyFlame(false);
                break;
            case "Decoration":
                DestroyFlame();
                break;
        }
    }

    public void DestroyFlame(bool byWater = true)
    {
        GetComponent<Collider2D>().enabled = false;
        PlaygroundManager pgRef = FindObjectOfType<PlaygroundManager>();
        if (pgRef != null)
        {
            pgRef.FlameEstinguished();
            if (byWater)
                pgRef.WaterOnPosition(transform.position);
        }

        Instantiate(vaporBurstPrefab, transform.position, Quaternion.identity);
        FindObjectOfType<AudioManager>().Play("VaporBurstQuick", transform.position);
        Destroy(gameObject);
    }

    IEnumerator DelayedRecharge(float delay)
    {
        yield return new WaitForSeconds(delay);
        noRecharge = false;
    }
}
