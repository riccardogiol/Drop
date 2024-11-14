using UnityEngine;

public class FireWaveCasting : MonoBehaviour
{
    public GameObject wavePrefab;
    public float timer = 2f;
    float countdown;

    public float spawnFlameProbability;


    PlaygroundManager playgroundManager;

    void Awake()
    {
        countdown = timer;
        playgroundManager = FindFirstObjectByType<PlaygroundManager>();
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
        GameObject wave = Instantiate(wavePrefab, transform.position, Quaternion.identity);
        wave.transform.parent = transform;
        wave.GetComponent<FireWave>().playgroundManager = playgroundManager;
        // if (Random.value < spawnFlameProbability)
        // {
        //     bullet.GetComponent<FireBullet>().flamePrefab = flamePrefab;
        //     bullet.GetComponent<FireBullet>().spawnFlame = true;
        //     bullet.GetComponent<FireBullet>().flameParent = flameParent;

        // }
    }
}