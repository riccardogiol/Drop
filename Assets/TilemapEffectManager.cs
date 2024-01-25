using System.Collections.Generic;
using UnityEngine;

public class TilemapEffectManager : MonoBehaviour
{
    GameObject particleCollider;
    List<TileFlowerManager> flowerTiles;
    RuleTileStateManager ruleTileStateManager;
    bool flowerAtStart = false;
    bool noFlower = false;
    bool allFlowered = false;
    bool flowersCollected = false;
    
    public Sprite[] petalListLvl1;
    public Sprite[] leafListLvl1;

    public Sprite[] petalListLvl2;
    public Sprite[] leafListLvl2;

    public Sprite[] petalListLvl3;
    public Sprite[] leafListLvl3;

    public Color[] colors;

    public float trySpreadingInterval = 3.0f;
    float trySpreadingTimer;

    void Start()
    {
        if (particleCollider == null)
            particleCollider = Resources.Load<GameObject>("ParticleCollider");
        ruleTileStateManager = GetComponent<RuleTileStateManager>();
        flowerTiles = new List<TileFlowerManager>();
        trySpreadingTimer = trySpreadingInterval;
    }

    public void SpawnParticleColliders(int maxX, int maxY)
    {
        if (particleCollider == null)
            return;
        for (int y = 0; y <= maxY; y++)
        {
            for (int x = 0; x <= maxX; x++)
            {
                RuleTile currentTile = ruleTileStateManager.GetTile(new Vector3Int(x, y, 0));
                if (currentTile != null)
                {
                    if (ruleTileStateManager.IsTileBurnt(currentTile))
                        SpawnParticleCollider(new Vector3(x + 0.5f, y + 0.5f), true); 
                    else
                        SpawnParticleCollider(new Vector3(x + 0.5f, y + 0.5f), false);   
                }
            }
        }
    }
    
    void SpawnParticleCollider(Vector3 position, bool isBurning)
    {
        GameObject goRef = Instantiate(particleCollider, position, Quaternion.identity);
        goRef.transform.parent = transform;
        if (isBurning)
            goRef.GetComponent<TileParticlesManager>().ActivateBurntParticle();
        goRef.GetComponent<TileFlowerManager>().tilemapEffectManager = this;
        InitializeFlowerGrafic( goRef.GetComponent<TileFlowerManager>());
    }

    void InitializeFlowerGrafic(TileFlowerManager tfm)
    {
        int indexLvl1 = Random.Range(0, petalListLvl1.Length);
        int indexLvl2 = Random.Range(0, petalListLvl2.Length);
        int indexLvl3 = Random.Range(0, petalListLvl3.Length);

        FlowerGFXData fgd = new FlowerGFXData(
            petalListLvl1[indexLvl1], leafListLvl1[indexLvl1],
            petalListLvl2[indexLvl2], leafListLvl2[indexLvl2],
            petalListLvl3[indexLvl3], leafListLvl3[indexLvl3],
            colors[Random.Range(0, colors.Length)]);
        tfm.SetFlowerGFX(fgd);
    }

    public void CollectFlowerTiles()
    {
        foreach(Transform child in transform)
        {
            if (child.CompareTag("ParticleCollider"))
            {
                TileFlowerManager tfmAux = child.GetComponent<TileFlowerManager>();
                if (!tfmAux.isObstacle)
                {
                    flowerTiles.Add(tfmAux);
                    if (tfmAux.isFlowering)
                        flowerAtStart = true;
                }
            }
        }
        flowersCollected = true;
    }

    void Update()
    {
        trySpreadingTimer -= Time.deltaTime;
        if (trySpreadingTimer < 0 && flowersCollected)
        {
            allFlowered = true;
            noFlower = true;
            foreach(TileFlowerManager tileFlowerManager in flowerTiles)
            {
                if (tileFlowerManager.isFlowering)
                {
                    noFlower = false;
                    tileFlowerManager.TrySpreadingAround();
                }
                else
                    allFlowered = false;
            }
            trySpreadingTimer = trySpreadingInterval;
            if (allFlowered)
                FindObjectOfType<StageManager>().WinGame();
            if (noFlower && flowerAtStart)
                FindObjectOfType<StageManager>().GameOver("no_flower");
        }
    }

    public void SetFlowerSpreading(float spreadInterval)
    {
        trySpreadingInterval = spreadInterval;
        trySpreadingTimer = spreadInterval;
    }


    public void BurnTile(Vector3 position)
    {
        GameObject particleCollider = GetParticleCollider(position);
        if (particleCollider != null)
        {
            particleCollider.GetComponent<TileParticlesManager>().ActivateBurntParticle();
            StopFloweringAllAround(position);
        }
    }

    public void WaterTile(Vector3 position)
    {
        GameObject particleCollider = GetParticleCollider(position);
        if (particleCollider != null)
            particleCollider.GetComponent<TileParticlesManager>().DesactivateBurntParticle();
    }

    public GameObject GetParticleCollider(Vector3 onCellPoint)
    {
        Collider2D[] results = Physics2D.OverlapPointAll(onCellPoint);
        foreach(Collider2D item in results)
        {
            if (item.gameObject.CompareTag("ParticleCollider"))
                return item.gameObject;
        }
        return null;
    }

    public bool IsTileBurnt(Vector3 pos)
    {
        return ruleTileStateManager.IsTileBurnt(pos);
    }

    void StopFloweringAllAround(Vector3 onCellPoint)
    {
        for (float y = onCellPoint.y - 1; y <= onCellPoint.y + 1; y++)
        {
            for (float x = onCellPoint.x - 1; x <= onCellPoint.x + 1; x++)
            {
                GameObject goRef = GetParticleCollider(new Vector3(x, y));
                if (goRef != null)
                    goRef.GetComponent<TileFlowerManager>().StopFlowering();
            }
        }
    }
}
