using UnityEngine;

public class FireBulletShootingTest : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 4.5f;
    public int bulletDamage = 4;

    public Transform shootingPoint;

    public float spawnFlameProbability;

    PlaygroundManager playgroundManager;
    PlayerDirectionController playerDirection;

    void Awake()
    {
        playerDirection = GetComponent<PlayerDirectionController>();
        playgroundManager = FindFirstObjectByType<PlaygroundManager>();
    }

    void Update()
    {
        if (MenusManager.isPaused)
            return;
        else if(Input.GetKeyDown(KeyCode.F))
        {
            TryShoot();
        }
    }

    public void TryShoot()
    {
        if (MenusManager.isPaused)
            return;
        Shoot();
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position + (Vector3)(playerDirection.lastDirection * 0.2f), Quaternion.LookRotation(Vector3.forward, playerDirection.lastDirection));
        bullet.GetComponent<FireBullet>().damage = bulletDamage;
        bullet.GetComponent<FireBullet>().playgroundManager = playgroundManager;
        bullet.GetComponent<FireBullet>().shootingEnemyID = gameObject.GetInstanceID();
        bullet.GetComponent<Rigidbody2D>().velocity = playerDirection.lastDirection * bulletSpeed;
        if (Random.value < spawnFlameProbability)
            bullet.GetComponent<FireBullet>().spawnFlame = true;

    }
}