using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    public float bulletEnergy = 5f;

    public Transform shootingPoint;

    public PlaygroundManager playgroundManager;
    
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
        GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position + playerMovement.lastDirection, Quaternion.LookRotation(Vector3.forward, playerMovement.lastDirection));
        bullet.GetComponent<PickWaterdrop>().energy = bulletEnergy;
        bullet.GetComponent<PickWaterdrop>().maxEnergy = bulletEnergy;
        bullet.transform.localScale = new Vector3(0.5f, 0.5f, 1);
        bullet.GetComponent<Bullet>().playgroundManager = playgroundManager;
        bullet.GetComponent<Rigidbody2D>().velocity = playerMovement.lastDirection * bulletSpeed;
    }
}