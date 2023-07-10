using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 50;
    public int currentHealth;

    //add/show? public HealthBar healthBar;

    void Start()
    {
        currentHealth = maxHealth;
        ScaleOnHealth();
    }

    public void ScaleOnHealth()
    {
        transform.localScale = new Vector3((float)currentHealth/maxHealth, (float)currentHealth/maxHealth, 1);
    }

    public void TakeDamage(int damage)
    {
        currentHealth = Math.Max( currentHealth - damage, 0);
        ScaleOnHealth();
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
    

    public void FillReservoir(int value)
    {
        currentHealth = Math.Min( currentHealth + value, maxHealth);
        ScaleOnHealth();
        // condition it dies!
    }
}
