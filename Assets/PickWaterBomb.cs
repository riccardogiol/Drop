using UnityEngine;

public class PickWaterBomb : MonoBehaviour
{
    public GameObject bulletPrefab;
    float bulletSpeed = 7f;
    int bulletEnergy = 2;
    int bulletDamage = 4;

    public GameObject wavePrefab;
    int waveDamage = 4;

    public int bombEnergy = 10;


    PlaygroundManager playgroundManager;

    void Start()
    {
        playgroundManager = FindFirstObjectByType<PlaygroundManager>();
        if (playgroundManager == null)
        {
            Debug.LogWarning("No playgroundManager found");
            return;
        }
    }

    public void TriggerBomb()
    {
        Shoot(new Vector3(1, 0));
        Shoot(new Vector3(-1, 0));
        Shoot(new Vector3(0, 1));
        Shoot(new Vector3(0, -1));
        WaveAttack();

        DestroyBomb();
    }

    void Shoot(Vector3 direction)
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position + (direction * 0.2f), Quaternion.LookRotation(Vector3.forward, direction));
        bullet.GetComponent<Bullet>().shootByPlayer = false;
        bullet.GetComponent<Bullet>().energy = bulletEnergy;
        bullet.GetComponent<Bullet>().damage = bulletDamage;
        bullet.GetComponent<Bullet>().playgroundManager = playgroundManager;
        bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
    }
    
    void WaveAttack()
    {
        GameObject wave = Instantiate(wavePrefab, transform.position, Quaternion.identity);
        wave.GetComponent<Wave>().damage = waveDamage;
        wave.GetComponent<Wave>().playgroundManager = playgroundManager;
    }

    public void DestroyBomb()
    {
        FindObjectOfType<AudioManager>().Play("BulletExplosion");
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
        case "Grass":
            FindObjectOfType<PlaygroundManager>().WaterOnPosition(other.transform.position);
            break;
        }
    }
    
}
