using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 10;
    public int currentHealth;

    public Transform enemyGFX;

    public GameObject vaporBurstPrefab;

    void Start()
    {
        currentHealth = maxHealth;
        ScaleOnHealth();
    }

    public void ScaleOnHealth()
    {
        float scale = (float)currentHealth/maxHealth*0.4f + 0.6f;
        enemyGFX.localScale = new Vector3(scale, scale, 1);
    }

    public void TakeDamage(int damage)
    {
        currentHealth = Math.Max( currentHealth - damage, 0);
        ScaleOnHealth();
        if (currentHealth <= 0)
        {
            PlaygroundManager pgRef = FindObjectOfType<PlaygroundManager>();
            if (pgRef != null)
                pgRef.WildfireEstinguished();
            Instantiate(vaporBurstPrefab, transform.position, Quaternion.identity);
            Instantiate(vaporBurstPrefab, transform.position + new Vector3(0, 0.5f), Quaternion.identity);
            Destroy(gameObject);
        }
    }
    

    public void FillReservoir(int value)
    {
        currentHealth = Math.Min( currentHealth + value, maxHealth);
        ScaleOnHealth();
    }
}
