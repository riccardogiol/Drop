using System.Collections;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    float bulletSpeed = 6f;
    int bulletEnergy = 2;
    int bulletDamage = 4;
    float cooldown = 1.5f;

    readonly string unlockingCode1 = "Lvl3";
    
    readonly string unlockingCode2 = "Waterbullet1Purchased";
    readonly string unlockingCode3 = "Waterbullet2Purchased";
    readonly float cooldown2 = 0.8f;

    public Transform shootingPoint;
    float timer;

    public GameObject takeWaterGFX;

    PlaygroundManager playgroundManager;
    ButtonFiller buttonFiller;
    PlayerDirectionController playerDirection;
    PlayerMovementPath playerMovement;
    PlayerMovementKeys playerMovementKeys;
    PlayerHealth playerHealth;
    PlayerAnimationManager animator;

    public int powerUsage;

    void Start()
    {
        if(PlayerPrefs.GetInt(unlockingCode1, 0) == 0)
        {
            enabled = false;
            return;
        }
        playerDirection = GetComponent<PlayerDirectionController>();
        playerMovement = GetComponent<PlayerMovementPath>();
        playerMovementKeys = GetComponent<PlayerMovementKeys>();
        playerHealth = GetComponent<PlayerHealth>();
        timer = 0;
        
        if (PlayerPrefs.GetInt(unlockingCode2, 0) == 1)
            cooldown = cooldown2;

        ButtonFiller[] buttonFillers = FindObjectsOfType<ButtonFiller>();
        foreach (var bf in buttonFillers)
        {
            if (bf.gameObject.name == "BulletButton")
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
        else if(Input.GetKeyDown(KeyCode.Space))
        {
            TryShoot();
        }
    }

    public void TryShoot()
    {
        if (MenusManager.isPaused)
            return;
        if (timer > 0)
            return;
        if (playerHealth.currentHealth > bulletEnergy)
        { 
            playerHealth.TakeDamage(bulletEnergy);
            playerMovement.InterruptMovement();
            playerMovementKeys.InterruptMovement(0.5f);
            if (animator != null)
                animator.PlayShooting();
            Shoot();
            timer = cooldown;
            if(PlayerPrefs.GetInt(unlockingCode3, 0) == 1 && Random.value < 0.20)
                StartCoroutine("DelayedEnergyReward");

        }
        else
        {
            buttonFiller.GetComponent<Animator>().SetTrigger("NoAmmo");
        }
        // else sound finished ammos
    }

    void Shoot()
    {
        powerUsage++;
        GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position + (Vector3)(playerDirection.lastDirection * 0.2f), Quaternion.LookRotation(Vector3.forward, playerDirection.lastDirection));
        bullet.GetComponent<Bullet>().energy = bulletEnergy;
        bullet.GetComponent<Bullet>().damage = bulletDamage;
        bullet.GetComponent<Bullet>().playgroundManager = playgroundManager;
        bullet.GetComponent<Rigidbody2D>().velocity = playerDirection.lastDirection * bulletSpeed;
    }

    public void SetBulletCost(int value)
    {
        bulletEnergy = value;
    }

    IEnumerator DelayedEnergyReward()
    {
        yield return new WaitForSeconds(0.3f);
        playerHealth.FillReservoir(1);
        Instantiate(takeWaterGFX, transform.position, Quaternion.identity);
    }
}