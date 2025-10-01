using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    int maxHealth = 4;
    public int currentHealth = 4;

    public StageManager stageManager;
    public HealthBar healthBar;
    public DamageIndicator damageIndicator;

    PlayerShield playerShield;

    readonly string unlockingCode2 = "Lvl2";
    readonly string unlockingCode3 = "Hero1Purchased";
    // il 2 e per la pioggia in PlaygroundManager
    readonly string unlockingCode5 = "Hero3Purchased";
    readonly string unlockingCode6 = "Hero4Purchased";

    void Start()
    {
        if (PlayerPrefs.GetInt(unlockingCode2, 0) == 1)
            maxHealth += 2;
        if (PlayerPrefs.GetInt(unlockingCode3, 0) == 1)
            maxHealth += 2;
        if (PlayerPrefs.GetInt(unlockingCode5, 0) == 1)
            maxHealth += 2;
        if (PlayerPrefs.GetInt("EasyMode", 0) == 1)
        {
            maxHealth += 2;
            currentHealth += 1;
        }

        playerShield = GetComponent<PlayerShield>();

        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(currentHealth);
    }

    public void TakeDamage(int damage, bool ignoreShield = false)
    {
        if (playerShield != null && !ignoreShield)
            if (playerShield.isActive)
                damage = playerShield.DamageShield(damage);
        currentHealth = Math.Max(currentHealth - damage, 0);
        damageIndicator.ShowDamage(damage);
        healthBar.SetHealth(currentHealth);
        if (currentHealth == 0)
        {
            if (PlayerPrefs.GetInt(unlockingCode6, 0) == 1 && UnityEngine.Random.value < 99.2) // change
            {
                stageManager.Reborn();
                StartCoroutine(RestoreLife());
            }
            else
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

    public int GetEnergy()
    {
        if (playerShield != null)
            return currentHealth + 2 * Mathf.CeilToInt(playerShield.GetEnergy());
        else
            return currentHealth;
    }

    IEnumerator RestoreLife()
    {
        yield return new WaitForSeconds(5);
        currentHealth = 1;
        healthBar.SetHealth(currentHealth);
    }
}
