using System.Collections;
using UnityEngine;

public class BossFireBulletShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 4.5f;
    public int bulletDamage = 4;
    public float timer = 2f;
    public float secondShootsDelay = 0.2f;

    public Transform shootingPoint1;
    public Transform shootingPoint2;
    public Transform shootingPoint3;
    float countdown;

    PlaygroundManager playgroundManager;
    // PlayerAnimationManager animator;

    void Awake()
    {
        countdown = 0f;
        playgroundManager = FindFirstObjectByType<PlaygroundManager>();
        
        if (PlayerPrefs.GetInt("EasyMode", 0) == 1)
            timer *= 1.3f;
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
        GameObject bullet = Instantiate(bulletPrefab, shootingPoint2.position, Quaternion.Euler(0, 0, 180));
        bullet.GetComponent<FireBullet>().damage = bulletDamage;
        bullet.GetComponent<FireBullet>().playgroundManager = playgroundManager;
        bullet.GetComponent<FireBullet>().shootingEnemyID = gameObject.GetInstanceID();
        bullet.GetComponent<Rigidbody2D>().velocity = Vector2.down * bulletSpeed;
        StartCoroutine(ShootSecondaryBullets());
    }

    IEnumerator ShootSecondaryBullets()
    {
        yield return new WaitForSeconds(secondShootsDelay);
        GameObject bullet = Instantiate(bulletPrefab, shootingPoint1.position, Quaternion.Euler(0, 0, 180));
        bullet.GetComponent<FireBullet>().damage = bulletDamage;
        bullet.GetComponent<FireBullet>().playgroundManager = playgroundManager;
        bullet.GetComponent<FireBullet>().shootingEnemyID = gameObject.GetInstanceID();
        bullet.GetComponent<Rigidbody2D>().velocity = Vector2.down * bulletSpeed;
        bullet = Instantiate(bulletPrefab, shootingPoint3.position, Quaternion.Euler(0, 0, 180));
        bullet.GetComponent<FireBullet>().damage = bulletDamage;
        bullet.GetComponent<FireBullet>().playgroundManager = playgroundManager;
        bullet.GetComponent<FireBullet>().shootingEnemyID = gameObject.GetInstanceID();
        bullet.GetComponent<Rigidbody2D>().velocity = Vector2.down * bulletSpeed;
    }
}