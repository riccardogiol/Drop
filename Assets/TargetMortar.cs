using System;
using UnityEngine;

public class TargetMortar : MonoBehaviour
{
    public Transform target;
    public float movementTime = 3.0f;
    public float height = 4.0f;
    public GameObject bombShadow;

    public GameObject bulletPrefab;
    public GameObject wavePrefab;
    float bulletSpeed = 5f;


    float ay1, ay2, yMax, viy, currentY, startingY, startingX, currentX, vx, shadowY, vy;
    float t2, currentT = 0.0f, currentT2;

    PlaygroundManager playgroundManager;

    void Awake()
    {
        playgroundManager = FindFirstObjectByType<PlaygroundManager>();  
    }

    void Start()
    {
        startingX = transform.position.x;
        startingY = transform.position.y;
        yMax = Math.Max(startingY, target.position.y);
        yMax += height;
        t2 = movementTime / 2f;
        float Dy = yMax - startingY;
        viy = 2 * Dy / t2;
        ay1 = -viy / t2;

        Dy = target.position.y - yMax;
        float vf2 = 2 * Dy / t2;
        ay2 = vf2 / t2;

        vx = (target.position.x - startingX) / movementTime;
        vy = (target.position.y - startingY) / movementTime;
    }

    void FixedUpdate()
    {
        currentT += Time.fixedDeltaTime;

        currentX = startingX + vx * currentT;
        shadowY = startingY + vy * currentT;

        if (currentT <= t2)
            currentY = startingY + viy * currentT + 0.5f * ay1 * currentT * currentT;
        else if (currentT <= movementTime)
        {
            currentT2 = currentT - t2;
            currentY = yMax + 0.5f * ay2 * currentT2 * currentT2;
        }
        else
            Detonate();

        transform.position = new Vector3(currentX, currentY);
        bombShadow.transform.position = new Vector3(currentX, shadowY);
    }

    void Detonate()
    {
        Shoot(new Vector3(1, 0));
        Shoot(new Vector3(-1, 0));
        Shoot(new Vector3(0, 1));
        Shoot(new Vector3(0, -1));
        WaveAttack();

        Destroy(transform.parent.gameObject);
    }

    void Shoot(Vector3 direction)
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position + (direction * 0.2f), Quaternion.LookRotation(Vector3.forward, direction));
        //bullet.GetComponent<FireBullet>().shootByPlayer = false;
        bullet.GetComponent<FireBullet>().playgroundManager = playgroundManager;
        bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
    }
    
    void WaveAttack()
    {
        GameObject wave = Instantiate(wavePrefab, transform.position, Quaternion.identity);
        //wave.GetComponent<FireWave>().shootByPlayer = true;
        wave.GetComponent<FireWave>().playgroundManager = playgroundManager;
    }

}
