using UnityEngine;

public class PlayerShield : MonoBehaviour
{

    public float timer = 3f;
    float countdown;

    public bool isActive = false;

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
            timer += 2;
        if (PlayerPrefs.GetInt("EasyMode", 0) == 1)
            timer += 1;
    }

    void Update()
    {
        if (isActive)
        {
            countdown -= Time.deltaTime;
            healthBar.SetShield(countdown / timer);
            if (countdown <= 1 && !graphicalCountdownPlayed)
            {
                flamesCountdown.PlayCountdown(1.0f, 3);
                graphicalCountdownPlayed = true;
            }
            if (countdown <= 0)
            {
                isActive = false;
                shieldGFX.SetBool("isActive", false);
                // make shield slider dissapear
            }
        }
    }

    public void Activate()
    {
        // make shield slider dissapear
        countdown = timer;
        isActive = true;
        shieldGFX.SetBool("isActive", true);
        graphicalCountdownPlayed = false;
    }

    public void PlayReflex()
    {
        shieldGFX.SetTrigger("touched");
    }
}
