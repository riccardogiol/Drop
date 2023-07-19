using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public StageManager stageManager;

    public int maxHealth = 100;
    public int currentHealth;

    public HealthBar healthBar;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(currentHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth = Math.Max( currentHealth - damage, 0);
        healthBar.SetHealth(currentHealth);
        if(currentHealth == 0)
        {
            //add some effect
            stageManager.GameOver();
        }
    }
    

    public void FillReservoir(int value)
    {
        currentHealth = Math.Min( currentHealth + value, maxHealth);
        healthBar.SetHealth(currentHealth);
        // condition it dies!
    }
}
