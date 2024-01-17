using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    float bulletSpeed = 6f;
    int bulletEnergy = 2;
    int bulletDamage = 4;
    float bulletRange = 3.0f;
    float cooldown = 1.5f;

    readonly string unlockingCode1 = "Lvl2";
    /*
    readonly string unlockingCode2 = "Lvl6";
    readonly float bulletRange2 = 6.0f;
    readonly string unlockingCode3 = "Lvl7";
    readonly int bulletDamege3 = 6; // or bulletEnergy = 2
    */

    public Transform shootingPoint;
    float timer;

    PlaygroundManager playgroundManager;
    ButtonFiller buttonFiller;
    PlayerDirectionController playerDirection;
    PlayerMovementPath playerMovement;
    PlayerHealth playerHealth;
    PlayerAnimationManager animator;

    void Start()
    {
        if(PlayerPrefs.GetInt(unlockingCode1, 0) == 0)
        {
            enabled = false;
            return;
        }
        playerDirection = GetComponent<PlayerDirectionController>();
        playerMovement = GetComponent<PlayerMovementPath>();
        playerHealth = GetComponent<PlayerHealth>();
        timer = 0;
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
         /*
        if (PlayerPrefs.GetInt(unlockingCode2, 0) == 1)
            bulletRange = bulletRange2;
        if (PlayerPrefs.GetInt(unlockingCode3, 0) == 1)
            bulletDamage = bulletDamege3;
        */

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
            if (animator != null)
                animator.PlayShooting();
            Shoot();
            timer = cooldown;
        }
        // else sound finished ammos
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position + (Vector3)(playerDirection.lastDirection * 0.2f), Quaternion.LookRotation(Vector3.forward, playerDirection.lastDirection));
        bullet.GetComponent<Bullet>().energy = bulletEnergy;
        bullet.GetComponent<Bullet>().damage = bulletDamage;
        bullet.GetComponent<Bullet>().range = bulletRange;
        bullet.GetComponent<Bullet>().playgroundManager = playgroundManager;
        bullet.GetComponent<Rigidbody2D>().velocity = playerDirection.lastDirection * bulletSpeed;
    }
}