using System.Collections;
using UnityEngine;

public class PlayerWave : MonoBehaviour
{
    public GameObject wavePrefab;
    int waveEnergy = 2;
    int waveDamage = 2;
    float cooldown = 2.0f;
    float timer;

    public GameObject takeWaterGFX;

    PlaygroundManager playgroundManager;
    ButtonFiller buttonFiller;
    PlayerHealth playerHealth;
    PlayerShield playerShield;
    PlayerMovementInterruption playerMovementInterruption;
    PlayerAnimationManager animator;

    readonly string unlockingCode1 = "WavePurchased";

    readonly string unlockingCode2 = "Wave1Purchased";
    readonly string unlockingCode3 = "Wave2Purchased";
    readonly string unlockingCode4 = "Wave3Purchased";
    readonly string unlockingCode5 = "Wave4Purchased";
    readonly float cooldown2 = 1.0f;

    public int powerUsage;


    void Start()
    {
        if(PlayerPrefs.GetInt(unlockingCode1, 0) == 0)
        {
            enabled = false;
            return;
        }
        playerHealth = GetComponent<PlayerHealth>();
        playerShield = GetComponent<PlayerShield>();
        playerMovementInterruption = GetComponent<PlayerMovementInterruption>();
        timer = 0;
        ButtonFiller[] buttonFillers = FindObjectsOfType<ButtonFiller>();

        if (PlayerPrefs.GetInt(unlockingCode2, 0) == 1)
            cooldown = cooldown2;
        foreach (var bf in buttonFillers)
        {
            if (bf.gameObject.name == "WaveButton")
               buttonFiller = bf;
        }
        buttonFiller.SetMaxValue(cooldown);
        buttonFiller.SetValue(0);
        playgroundManager = FindFirstObjectByType<PlaygroundManager>();
        if (playgroundManager == null)
        {
            Debug.LogWarning("No playgroundManager found");
            return;
        }
        animator = FindFirstObjectByType<PlayerAnimationManager>();

        powerUsage = 0;

    }

    void Update()
    {
        if (MenusManager.isPaused)
            return;
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            buttonFiller.SetValue(timer);
        }
        //keyboard input
        else if(Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            TryWaveAttack();
        }
    }

    //button input
    public void TryWaveAttack()
    {
        if (MenusManager.isPaused)
            return;
        if (timer > 0)
            return;
        if (playerHealth.currentHealth > waveEnergy)
        { 
            playerHealth.TakeDamage(waveEnergy, true);
            playerMovementInterruption.StopInCenterOfCell();
            if (animator != null)
                animator.PlayCastingWave();
            WaveAttack();
            timer = cooldown;
            if(PlayerPrefs.GetInt(unlockingCode3, 0) == 1 && Random.value < 0.20)
                StartCoroutine("DelayedEnergyReward");
        }
        else
        {
            buttonFiller.GetComponent<Animator>().SetTrigger("NoAmmo");
        }
    }

    void WaveAttack()
    {
        FindObjectOfType<AudioManager>().PlayVoice("Shoot");
        powerUsage++;
        GameObject wave = Instantiate(wavePrefab, transform.position, Quaternion.identity);
        wave.GetComponent<Wave>().damage = waveDamage;
        wave.GetComponent<Wave>().playgroundManager = playgroundManager;
        if (PlayerPrefs.GetInt(unlockingCode4, 0) == 1 && playgroundManager.IsRaining())
            wave.GetComponent<Wave>().bigWave = true;
        if (playerShield.isActive && PlayerPrefs.GetInt(unlockingCode5, 0) == 1)
            wave.GetComponent<Wave>().spawnIcemines = true;
    }

    
    public void SetWaveCost(int value)
    {
        waveEnergy = value;
    }

    
    IEnumerator DelayedEnergyReward()
    {
        yield return new WaitForSeconds(0.3f);
        playerHealth.FillReservoir(1);
        Instantiate(takeWaterGFX, transform.position, Quaternion.identity);
    }
}