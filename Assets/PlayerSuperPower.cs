using System;
using UnityEngine;

public class PlayerSuperPower : MonoBehaviour
{
    SuperBarAndButtonManager barManager;
    readonly string unlockingCode1 = "SuperPurchased";
    readonly string unlockingCode2 = "Super1Purchased";
    readonly string unlockingCode3 = "Super2Purchased";
    readonly string unlockingCode4 = "Super3Purchased";
    readonly string unlockingCode5 = "Super4Purchased";
    int upgradeLvl = 1;

    PlayerShooting playerShooting;
    PlayerWave playerWave;
    PlaygroundManager playgroundManager;

    public float currentValue = 0;
    int maxValue = 5;

    bool superState = false;
    
    public ParticleSystem lightningSparklesPS;
    public GameObject lightningBurstPrefab;
    public SpriteRenderer playerGFX;
    public DamageIndicator damageIndicator;

    public Material bloomMaterial;
    Material originaleMaterial;

    public GameObject waterBombCloudPrefab;

    void Awake()
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
        playgroundManager = FindFirstObjectByType<PlaygroundManager>();

        originaleMaterial = playerGFX.material;
        if(PlayerPrefs.GetInt(unlockingCode2, 0) == 1)
            upgradeLvl++;
        if(PlayerPrefs.GetInt(unlockingCode3, 0) == 1)
        {
            upgradeLvl++;
            currentValue += 2;
        }
        if(PlayerPrefs.GetInt(unlockingCode4, 0) == 1)
            upgradeLvl++;
        if(PlayerPrefs.GetInt(unlockingCode5, 0) == 1)
            upgradeLvl++;
    }

    void Start()
    {

        barManager.SetButtonInteractable(false);
        barManager.SetSliderMax(maxValue);
        barManager.UpdateSlider(currentValue);
        if (currentValue >= maxValue)
            barManager.SetButtonInteractable(true);
    }

    public void Recharge(int value)
    {
        damageIndicator.ShowEnergy(Math.Min(value, maxValue - (int)currentValue));
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
            currentValue -= Time.deltaTime/1.5f;
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
        // GFX
        GameObject goRef = Instantiate(lightningBurstPrefab, transform.position, Quaternion.identity);
        goRef.GetComponent<ParticleSystem>().emission.SetBurst(0, new ParticleSystem.Burst(0, upgradeLvl));
        playerGFX.material = new Material(bloomMaterial);
        playerGFX.material.SetColor("_Color", new Color(2.0f, 2.0f, 2.0f));
        lightningSparklesPS.Play();
        
        // Power effects
        playerShooting.SetBulletCost(0);
        playerWave.SetWaveCost(0);

        if(PlayerPrefs.GetInt(unlockingCode2, 0) == 1)
            playgroundManager.MakeRain(true, true, true, false, true);

        if (PlayerPrefs.GetInt(unlockingCode4, 0) == 1)
        {
            Transform spot = playgroundManager.GetRandomFlame();
            if (spot != null)
                Instantiate(waterBombCloudPrefab, spot.position, Quaternion.identity);
        }
    }

    void ExitSuperState()
    {
        GameObject goRef = Instantiate(lightningBurstPrefab, transform.position, Quaternion.identity);
        goRef.GetComponent<ParticleSystem>().emission.SetBurst(0, new ParticleSystem.Burst(0, upgradeLvl));
        lightningSparklesPS.Stop();
        playerGFX.material = originaleMaterial;

        playerShooting.SetBulletCost(2);
        playerWave.SetWaveCost(2);

        if(PlayerPrefs.GetInt(unlockingCode2, 0) == 1)
            playgroundManager.MakeRain(false, true, true, false, true);
    }
}
