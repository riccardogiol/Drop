using System.Collections;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject iceBulletPrefab;
    float bulletSpeed = 6f;
    int bulletEnergy = 2;
    int bulletDamage = 4;
    float cooldown = 1.5f;

    readonly string unlockingCode1 = "Lvl3";
    
    readonly string unlockingCode2 = "Waterbullet1Purchased";
    readonly string unlockingCode3 = "Waterbullet2Purchased";
    readonly string unlockingCode4 = "Waterbullet3Purchased";
    readonly string unlockingCode5 = "Waterbullet4Purchased";
    readonly float cooldown2 = 0.8f;

    public Transform shootingPoint;
    float timer;

    public GameObject takeWaterGFX;

    PlaygroundManager playgroundManager;
    ButtonFiller buttonFiller;
    PlayerDirectionController playerDirection;
    PlayerMovementInterruption playerMovementInterruption;
    PlayerHealth playerHealth;
    PlayerAnimationManager animator;
    PlayerShield playerShield;

    public int powerUsage;

    void Start()
    {
        if(PlayerPrefs.GetInt(unlockingCode1, 0) == 0)
        {
            enabled = false;
            return;
        }
        playerDirection = GetComponent<PlayerDirectionController>();
        playerMovementInterruption = GetComponent<PlayerMovementInterruption>();
        playerHealth = GetComponent<PlayerHealth>();
        playerShield = GetComponent<PlayerShield>();
        timer = 0;
        
        if (PlayerPrefs.GetInt(unlockingCode3, 0) == 1)
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
            playerHealth.TakeDamage(bulletEnergy, true);
            playerMovementInterruption.StopInCenterOfCell();
            if (animator != null)
                animator.PlayShooting();
            Shoot();
            timer = cooldown;
            if(PlayerPrefs.GetInt(unlockingCode3, 0) == 1 && Random.value < 0.20) // decidere s elasciare così o meno
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
        GameObject bullet;
        if (PlayerPrefs.GetInt(unlockingCode5, 0) == 1 && playerShield.isActive)
        {
            playerShield.DamageShield(2);
            bullet = Instantiate(iceBulletPrefab, shootingPoint.position + (Vector3)(playerDirection.lastDirection * 0.2f), Quaternion.LookRotation(Vector3.forward, playerDirection.lastDirection));
            bullet.GetComponent<Bullet>().damage = bulletDamage + 2;
        }
        else
        {
            bullet = Instantiate(bulletPrefab, shootingPoint.position + (Vector3)(playerDirection.lastDirection * 0.2f), Quaternion.LookRotation(Vector3.forward, playerDirection.lastDirection));
            bullet.GetComponent<Bullet>().damage = bulletDamage;
        }
        if (PlayerPrefs.GetInt(unlockingCode2, 0) == 1)
            bullet.GetComponent<Bullet>().piercingShoot = true;

        bullet.GetComponent<Bullet>().energy = bulletEnergy;
        bullet.GetComponent<Bullet>().playgroundManager = playgroundManager;
        //if (PlayerPrefs.GetInt(unlockingCode4, 0) == 1 && playgroundManager.IsRaining())
        if (PlayerPrefs.GetInt(unlockingCode4, 0) == 1 && Random.value < 0.15f)
            bullet.GetComponent<Bullet>().castWave = true;
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