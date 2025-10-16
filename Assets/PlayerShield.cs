using System;
using UnityEngine;

public class PlayerShield : MonoBehaviour
{

    public float timer = 3f;
    float countdown = 0.0f;

    public bool isActive = false;
    int reduceFactor = 2;

    public Animator shieldGFX;
    public FlamesCountdown flamesCountdown;
    public HealthBar healthBar;


    bool graphicalCountdownPlayed = false;

    // TODO add some code to make it last loger
    readonly string unlockingCode1 = "ExtraTimeShieldUnlocked";

    void Awake()
    {
        healthBar = FindFirstObjectByType<HealthBar>();
        if (PlayerPrefs.GetInt(unlockingCode1, 0) == 1)
            timer += 5;
        if (PlayerPrefs.GetInt("EasyMode", 0) == 1)
            timer *= 1.3f;
    }

    void Update()
    {
        if (isActive)
        {
            countdown -= Time.deltaTime;
            healthBar.SetShield(countdown / timer);
            // togliere? modificare? iun ogni caso mettere condizione se é minore di 1 ma maggiore di zero nel caso prenda danni e scatti da +2 a zero
            if (countdown <= 1 && !graphicalCountdownPlayed)
            {
                flamesCountdown.PlayCountdown(1.0f, 3);
                graphicalCountdownPlayed = true;
            }
            if (countdown <= 0)
            {
                isActive = false;
                FindObjectOfType<AudioManager>().Play("IceShield", transform.position);
                shieldGFX.SetBool("isActive", false);
            }
        }
    }

    public void Activate()
    {
        countdown = timer;
        isActive = true;
        FindObjectOfType<AudioManager>().Play("IceShield", transform.position);
        shieldGFX.SetBool("isActive", true);
        graphicalCountdownPlayed = false;
    }

    public int DamageShield(int damage)
    {
        damage /= reduceFactor;
        PlayReflex();
        FindObjectOfType<AudioManager>().Play("IceShield", transform.position);
        int extraDamage = Math.Max(Mathf.FloorToInt(damage - countdown), 0);
        countdown = Math.Max(countdown - damage, 0);
        return extraDamage;
    }

    public void PlayReflex()
    {
        shieldGFX.SetTrigger("touched");
    }

    public float GetEnergy()
    {
        return Math.Max(countdown, 0);
    }
}
