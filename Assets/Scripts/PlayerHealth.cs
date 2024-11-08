using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    int maxHealth = 4;
    public int currentHealth = 4;

    public StageManager stageManager;
    public HealthBar healthBar;
    public DamageIndicator damageIndicator;
    
    readonly string unlockingCode2 = "Lvl2";
    readonly int maxHealth2 = 6;
    readonly string unlockingCode3 = "Hero1Purchased";
    readonly int maxHealth3 = 8;

    void Start()
    {
        if (PlayerPrefs.GetInt(unlockingCode2, 0) == 1)
            maxHealth = maxHealth2;
        if (PlayerPrefs.GetInt(unlockingCode3, 0) == 1)
            maxHealth = maxHealth3;
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(currentHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth = Math.Max( currentHealth - damage, 0);
        damageIndicator.ShowDamage(damage);
        healthBar.SetHealth(currentHealth);
        if(currentHealth == 0)
        {
            //add some effect
            stageManager.GameOver("health");
        }
    }
    

    public void FillReservoir(int value)
    {
        int energyAbsorbed = Math.Min(maxHealth - currentHealth, value);
        currentHealth += energyAbsorbed;
        damageIndicator.ShowEnergy(energyAbsorbed);
        healthBar.SetHealth(currentHealth);
    }
}
