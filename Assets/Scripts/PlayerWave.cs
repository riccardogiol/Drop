using UnityEngine;

public class PlayerWave : MonoBehaviour
{
    public GameObject wavePrefab;
    public float waveEnergy = 5f;
    public float waveDamage = 5f;

    public float cooldown = 1.5f;
    float timer;

    PlaygroundManager playgroundManager;

    ButtonFiller buttonFiller;
    
    PlayerHealth playerHealth;

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
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
    }

    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            buttonFiller.SetValue(timer);
        }
        //keyboard input
        else if(Input.GetKeyDown(KeyCode.E))
        {
            TryWaveAttack();
        }
    }

    //button input
    public void TryWaveAttack()
    {
        if (timer > 0)
            return;
        if (playerHealth.currentHealth > waveEnergy)
        { 
            playerHealth.TakeDamage((int)waveEnergy);
            WaveAttack();
            timer = cooldown;
        }
        // else sound finished ammos
    }

    void WaveAttack()
    {
        GameObject wave = Instantiate(wavePrefab, transform.position, Quaternion.identity);
        wave.GetComponent<Wave>().damage = waveDamage;
        wave.GetComponent<Wave>().playgroundManager = playgroundManager;
    }
}