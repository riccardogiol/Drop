using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    public float bulletEnergy = 5f;

    
    PlayerMovement playerMovement;
    PlayerHealth playerHealth;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            playerHealth.TakeDamage((int)bulletEnergy);
            Shoot();
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position + playerMovement.lastDirection, Quaternion.LookRotation(Vector3.forward, playerMovement.lastDirection));
        bullet.GetComponent<PickWaterdrop>().energy = bulletEnergy;
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = playerMovement.lastDirection * bulletSpeed;

    }
}