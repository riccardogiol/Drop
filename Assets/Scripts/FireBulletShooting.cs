using UnityEngine;

public class FireBulletShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 4.5f;
    public int bulletDamage = 4;
    public float timer = 2f;

    public Transform shootingPoint;
    float countdown;

    public float spawnFlameProbability;

    PlaygroundManager playgroundManager;
    EnemyDirectionController enemyDirection;
    // PlayerAnimationManager animator;

    void Awake()
    {
        enemyDirection = GetComponent<EnemyDirectionController>();
        countdown = 0f;
        playgroundManager = FindFirstObjectByType<PlaygroundManager>();
        
        if (PlayerPrefs.GetInt("EasyMode", 0) == 1)
        {
            timer *= 1.3f;
            bulletSpeed = bulletSpeed * 0.7f;
        }
    }

    void Update()
    {
        if (MenusManager.isPaused)
            return;
        if (countdown > 0)
        {
            countdown -= Time.deltaTime;
        }
        else 
        {
            TryShoot();
            countdown = timer;
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
        GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position + (Vector3)(enemyDirection.lastDirection * 0.2f), Quaternion.LookRotation(Vector3.forward, enemyDirection.lastDirection));
        bullet.GetComponent<FireBullet>().damage = bulletDamage;
        bullet.GetComponent<FireBullet>().playgroundManager = playgroundManager;
        bullet.GetComponent<FireBullet>().shootingEnemyID = gameObject.GetInstanceID();
        bullet.GetComponent<Rigidbody2D>().velocity = enemyDirection.lastDirection * bulletSpeed;
        if (Random.value < spawnFlameProbability)
            bullet.GetComponent<FireBullet>().spawnFlame = true;

    }
}