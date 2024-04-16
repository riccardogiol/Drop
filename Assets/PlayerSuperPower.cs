using System;
using UnityEngine;

public class PlayerSuperPower : MonoBehaviour
{
    SuperBarAndButtonManager barManager;
    readonly string unlockingCode1 = "SuperPurchased";

    PlayerShooting playerShooting;
    PlayerWave playerWave;

    public float currentValue = 0;
    int maxValue = 5;

    bool superState = false;

    void Start()
    {
        
        if(PlayerPrefs.GetInt(unlockingCode1, 0) == 0)
        {
            enabled = false;
            return;
        }
        barManager = FindFirstObjectByType<SuperBarAndButtonManager>();
        if(barManager == null)
        {
            Debug.Log("No super bar manager found");
            enabled = false;
            return ;
        }
        playerShooting = GetComponent<PlayerShooting>();
        playerWave = GetComponent<PlayerWave>();

        barManager.SetButtonInteractable(false);
        barManager.SetSliderMax(maxValue);
        barManager.UpdateSlider(currentValue);
        if (currentValue >= maxValue)
            barManager.SetButtonInteractable(true);
    }

    public void Recharge(int value)
    {
        currentValue = Math.Min(currentValue + value, maxValue);
        barManager.UpdateSlider(currentValue);
        if (currentValue >= maxValue)
            barManager.SetButtonInteractable(true);
    }

    public void TryActivate()
    {
        if (currentValue < maxValue || superState)
           return;
        superState = true;
        barManager.SetButtonInteractable(false);
        EnterSuperState();
    }

    void Update()
    {
        if(superState)
        {
            currentValue -= Time.deltaTime;
            barManager.UpdateSlider(currentValue);
            if(currentValue <= 0)
            {
                superState = false;
                ExitSuperState();
                currentValue = 0;
                barManager.UpdateSlider(0);
            }
        } else if(Input.GetKeyDown(KeyCode.E))
        {
            TryActivate();
        }
    }

    void EnterSuperState()
    {
        playerShooting.SetBulletCost(0);
        playerWave.SetWaveCost(0);
    }

    void ExitSuperState()
    {
        playerShooting.SetBulletCost(2);
        playerWave.SetWaveCost(2);
    }
}
