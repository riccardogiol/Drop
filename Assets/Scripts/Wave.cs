using UnityEngine;

public class Wave : MonoBehaviour
{
    public PlaygroundManager playgroundManager;
    public int damage = 2;
    public float timer = 1;

    public GameObject[] watersparklers;

    void Start()
    {
        float minX=0, maxX=1, minY=0, maxY=1;
        int sector = 0;
        for (int i = 0; i < 12; i++)
        {
            Vector3 destination = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY));
            destination.Normalize();
            SpawnWatersparkle(destination);

            sector = (sector + 1) % 4;
            switch (sector)
            {
                case 0: minX=0; maxX=1; minY=0; maxY=1; break;
                case 1: minX=0; maxX=1; minY=-1; maxY=0; break;
                case 2: minX=-1; maxX=0; minY=-1; maxY=0; break;
                case 3: minX=-1; maxX=0; minY=0; maxY=1; break;
            }
        }
    }

    void Update()
    {
        if (timer<0)
        {
            Destroy(gameObject);
        } else {
            timer -= Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            return;
        if (other.CompareTag("Wall"))
            playgroundManager.WaterOnPosition(other.transform.position);
        if (other.CompareTag("Grass"))
            playgroundManager.WaterOnPosition(other.transform.position);
        if (other.CompareTag("Enemy"))
            other.GetComponent<EnemyHealth>().TakeDamage(damage);
        if (other.CompareTag("Flame"))
        {
            int otherEnergy = other.GetComponent<PickFlame>().energy;
            if (otherEnergy <= damage)
                other.GetComponent<PickFlame>().DestroyFlame();
            else {
                other.GetComponent<PickFlame>().energy -= damage;
                other.GetComponent<PickFlame>().ScaleOnEnergy();
            }
        }
        if (other.CompareTag("Waterbomb"))
            other.GetComponent<PickWaterBomb>().TriggerBomb();
    }

    void SpawnWatersparkle(Vector3 movement)
    {
        GameObject goRef = Instantiate(watersparklers[Random.Range(0, watersparklers.Length)], transform.position + movement/4, Quaternion.LookRotation(Vector3.forward, movement));
        goRef.transform.localScale = new Vector3(0, 0, 0);
        goRef.transform.parent = transform;
        LinearMovement compRef = goRef.GetComponent<LinearMovement>();
        compRef.startingScale = new Vector3(0, 0, 0);
        compRef.scale = true;
        compRef.MoveTo(transform.position + movement, 0.5f);
    }
}

