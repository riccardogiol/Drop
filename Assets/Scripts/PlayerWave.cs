using UnityEngine;

public class PlayerWave : MonoBehaviour
{
    public GameObject wavePrefab;
    int waveEnergy = 2;
    int waveDamage = 2;
    float cooldown = 1.5f;
    float timer;

    PlaygroundManager playgroundManager;
    ButtonFiller buttonFiller;
    PlayerHealth playerHealth;
    PlayerMovementPath playerMovement;
    PlayerMovementKeys playerMovementKeys;
    PlayerAnimationManager animator;

    readonly string unlockingCode1 = "Lvl5";


    void Start()
    {
        if(PlayerPrefs.GetInt(unlockingCode1, 0) == 0)
        {
            enabled = false;
            return;
        }
        playerHealth = GetComponent<PlayerHealth>();
        playerMovement = GetComponent<PlayerMovementPath>();
        playerMovementKeys = GetComponent<PlayerMovementKeys>();
        timer = 0;
        ButtonFiller[] buttonFillers = FindObjectsOfType<ButtonFiller>();
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
        else if(Input.GetKeyDown(KeyCode.LeftShift))
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
            playerHealth.TakeDamage(waveEnergy);
            playerMovement.InterruptMovement();
            playerMovementKeys.InterruptMovement(0.5f);
            if (animator != null)
                animator.PlayCastingWave();
            WaveAttack();
            timer = cooldown;
        }
        else
        {
            buttonFiller.GetComponent<Animator>().SetTrigger("NoAmmo");
        }
    }

    void WaveAttack()
    {
        GameObject wave = Instantiate(wavePrefab, transform.position, Quaternion.identity);
        wave.GetComponent<Wave>().damage = waveDamage;
        wave.GetComponent<Wave>().playgroundManager = playgroundManager;
    }
}