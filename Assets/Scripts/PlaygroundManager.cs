using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlaygroundManager : MonoBehaviour
{
    StageManager stageManager;
    CameraEffectManager cameraEffectManager;

    Tilemap walkTilemap;
    Tilemap wallTilemap;
    TilemapEffectManager tilemapEffectManager;
    TilemapWallEffectManager tilemapWallEffectManager;
    public int maxX = 40;
    public int maxY = 40;

    GameObject flameParent;
    GameObject waterdropParent;
    DecorationManager decorationManager;
    ParticleSystem rainEffect;
    public GameObject flamePrefab;
    public GameObject waterdropPrefab;

    int totalTiles = 0;
    int burntTiles = 0;
    float fireValue = 0;
    float progressionPerc = 0;
    bool isRaining = false;
    float rainInterval = 3.0f;
    ProgressionBarFiller progressionBar;
    public float minProgressionPerc = 0.3f;
    public float loseProgressionPerc = 0.45f;
    public float rainProgressionPerc = 1.0f;
    public float winProgressionPerc = 0.98f;

    public bool winByFlower = true;

    bool reachedWinningCondition;

    void Awake()
    {
        stageManager = FindFirstObjectByType<StageManager>();
        cameraEffectManager = FindFirstObjectByType<CameraEffectManager>();

        GameObject auxGO = transform.Find("WalkTilemap").gameObject;
        if (auxGO == null)
            return;
        walkTilemap = auxGO.GetComponent<Tilemap>();

        auxGO = transform.Find("WallTilemap").gameObject;
        if (auxGO == null)
            return;
        wallTilemap = auxGO.GetComponent<Tilemap>();

        flameParent = transform.Find("FlameParent").gameObject;
        waterdropParent = transform.Find("WaterdropParent").gameObject;
        decorationManager = FindObjectOfType<DecorationManager>();
        GameObject rainGO = transform.Find("RainEffect").gameObject;
        if(rainGO!= null)
            rainEffect = rainGO.GetComponent<ParticleSystem>();
        reachedWinningCondition = false;

        if (!winByFlower)
        {
            FlowerBarFiller goRef = FindFirstObjectByType<FlowerBarFiller>();
            if (goRef != null)
                goRef.gameObject.SetActive(false);
        }

    }

    void Start()
    {
        walkTilemap.GetComponent<RuleTileStateManager>().SetTilemapLimit(maxX, maxY);
        wallTilemap.GetComponent<RuleTileStateManager>().SetTilemapLimit(maxX, maxY);
        walkTilemap.GetComponent<RuleTileStateManager>().EvaluateTilesState();
        wallTilemap.GetComponent<RuleTileStateManager>().EvaluateTilesState();
        flameParent.GetComponent<FireCounter>().UpdateFireCounters();
        fireValue = flameParent.GetComponent<FireCounter>().FireValue();
        totalTiles = walkTilemap.GetComponent<RuleTileStateManager>().numberTiles();
        Debug.Log("Total tiles: " + totalTiles);

        progressionBar = FindFirstObjectByType<ProgressionBarFiller>();
        if (progressionBar == null)
        {
            Debug.LogWarning("No progression bar found");
            return;
        }
        progressionBar.SetGameOverLimit(Math.Max((loseProgressionPerc - minProgressionPerc) / (1-minProgressionPerc), 0));
        progressionBar.SetRainLimit((rainProgressionPerc - minProgressionPerc) / (1-minProgressionPerc));
        
        EvaluateCleanSurface();

        tilemapEffectManager = walkTilemap.GetComponent<TilemapEffectManager>();
        tilemapEffectManager.SpawnParticleColliders(maxX, maxY);
        tilemapWallEffectManager = wallTilemap.GetComponent<TilemapWallEffectManager>();
        if(tilemapWallEffectManager != null)
            tilemapWallEffectManager.SpawnParticleColliders(maxX, maxY);
        StartCoroutine(StartFlowerCounter());
    }

    IEnumerator StartFlowerCounter()
    {
        yield return new WaitForSeconds(1);
        tilemapEffectManager.CollectFlowerTiles();
    }

    public void FlameOnPosition(Vector3 position)
    {
        Vector3Int cell = walkTilemap.WorldToCell(position);
        AddFlame(cell);
    }

    public void AddFlame(Vector3Int cell)
    {
        Vector3 cellCenter = walkTilemap.GetCellCenterWorld(cell);
        GameObject auxFlame = GetFlameInPosition(cellCenter);
        if (auxFlame == null)
        {
            GameObject newFlame = Instantiate(flamePrefab, cellCenter, Quaternion.identity);
            newFlame.transform.parent = flameParent.transform;
            flameParent.GetComponent<FireCounter>().flameCounter++;
            fireValue = flameParent.GetComponent<FireCounter>().FireValue();

            FindObjectOfType<AudioManager>().Play("FireBurst");
            BurnCellsAround(cell);
        } else {
            PickFlame pickFlame = auxFlame.GetComponent<PickFlame>();
            if (pickFlame.energy == pickFlame.maxEnergy)
                return;
            else
            {
                pickFlame.RechargeEnergy(2);
                FindObjectOfType<AudioManager>().Play("FireBurst");
                BurnCellsAround(cell);
            }
        }
    }

    public void AddWaterdrop(Vector3Int cell)
    {
        Vector3 cellCenter = walkTilemap.GetCellCenterWorld(cell);
        bool isOnPlayground = IsOnPlayground(cellCenter);
        if (isOnPlayground)
        {
            GameObject newWaterdrop = Instantiate(waterdropPrefab, cellCenter, Quaternion.identity);
            newWaterdrop.transform.parent = waterdropParent.transform;
            //FindObjectOfType<AudioManager>().Play("FireBurst");
        }
    }

    GameObject GetFlameInPosition(Vector3 cellCenter)
    {
        foreach(Transform child in flameParent.transform)
        {
            if(child.gameObject.CompareTag("Flame"))
            {
                if (child.transform.position == cellCenter)
                    return child.gameObject;
            }
        }
        return null;
    }

    //rewrite this with getTile?
    bool IsOnPlayground(Vector3 cellCenter)
    {
        foreach(Transform child in walkTilemap.transform)
        {
            if (child.transform.position == cellCenter)
                    return true;
        }
        return false;
    }

    public bool IsObstacleForFlame(Vector3 onCellPoint)
    {
        Collider2D[] results = Physics2D.OverlapPointAll(onCellPoint);
        foreach(Collider2D item in results)
        {
            if (item.gameObject.layer == 6)
                return true;
            if (item.gameObject.CompareTag("Waterdrop"))
                return true;
            if (item.gameObject.CompareTag("Flame"))
                return true;
            if (item.gameObject.CompareTag("Waterbomb"))
                return true;
            if (item.gameObject.CompareTag("OneWayCollider"))
                return true;
        }
        return false;
    }

    public bool IsObstacle(Vector3 onCellPoint)
    {
        Collider2D[] results = Physics2D.OverlapPointAll(onCellPoint);
        foreach(Collider2D item in results)
        {
            if (item.gameObject.layer == 6)
                return true;
            if (item.gameObject.CompareTag("Waterbomb"))
                return true;
            if (item.gameObject.CompareTag("OneWayCollider"))
                return true;
        }
        return false;
    }
    
    public Vector3 GetCellCenter(Vector3 onCellPoint)
    {
        Vector3Int tileCoordinate = walkTilemap.WorldToCell(onCellPoint);
        return walkTilemap.GetCellCenterWorld(tileCoordinate);
    }

    public void BurnCellsAround(Vector3Int cell)
    {
        for (int x = cell.x - 1; x <= cell.x + 1; x++)
        {
            BurnCell(new Vector3Int(x, cell.y, 0));
        }
        for (int y = cell.y - 1; y <= cell.y + 1; y++)
        {
            BurnCell(new Vector3Int(cell.x, y, 0));
        }
    }

    public void FireOnPosition(Vector3 position)
    {
        Vector3Int cell = walkTilemap.WorldToCell(position);
        BurnCell(cell);
    }

    public void BurnCell(Vector3Int cell)
    {
        Collider2D[] results = Physics2D.OverlapPointAll(walkTilemap.GetCellCenterWorld(cell));
        foreach(Collider2D item in results)
        {
            if (item.gameObject.CompareTag("DecorationNoFire"))
                return;
        }
        bool statechange = walkTilemap.GetComponent<RuleTileStateManager>().BurnTile(cell);
        bool statechange2 = wallTilemap.GetComponent<RuleTileStateManager>().BurnTile(cell);
        if (statechange || statechange2)
            EvaluateCleanSurface();
    }

    public void WaterOnPosition(Vector3 position)
    {
        Vector3Int cell = walkTilemap.WorldToCell(position);
        WaterCell(cell);
    }

    public void WaterCell(Vector3Int cell)
    {
        bool statechange = walkTilemap.GetComponent<RuleTileStateManager>().WaterTile(cell);
        bool statechange2 = wallTilemap.GetComponent<RuleTileStateManager>().WaterTile(cell);
        if (statechange || statechange2)
            EvaluateCleanSurface();
    }

    public void FlameEstinguished()
    {
        flameParent.GetComponent<FireCounter>().flameCounter--;
        fireValue = flameParent.GetComponent<FireCounter>().FireValue();
        EvaluateLevelProgression();
    }

    public void WildfireEstinguished()
    {
        flameParent.GetComponent<FireCounter>().wildfireCounter--;
        fireValue = flameParent.GetComponent<FireCounter>().FireValue();
        EvaluateLevelProgression();
    }

    void EvaluateCleanSurface()
    {
        burntTiles = walkTilemap.GetComponent<RuleTileStateManager>().numberBurntTiles();
        EvaluateLevelProgression();
    }

    void EvaluateLevelProgression()
    {
        progressionPerc = 1.0f - (fireValue + burntTiles)/totalTiles;
        float progressionPercOnMin = (progressionPerc - minProgressionPerc) / (1-minProgressionPerc);
        progressionBar.SetValue(progressionPercOnMin);

        if (cameraEffectManager != null)
        {
            cameraEffectManager.SetEffect(progressionPercOnMin);
        }
        
        Debug.Log("Burnt tiles: " + burntTiles);
        Debug.Log("Fire value: " + fireValue);
        Debug.Log("Progression perc: " + progressionPerc);
        if (progressionPerc >= winProgressionPerc && !reachedWinningCondition)
        {
            StopAllCoroutines();
            isRaining = true;
            reachedWinningCondition = true;
            MakeRain(isRaining);
            tilemapEffectManager.SetFlowerSpreading(0.5f);
            if (!winByFlower)
                stageManager.WinGame();
        }
        if (!isRaining && progressionPerc > (rainProgressionPerc + 0.05))
        {
            isRaining = true;
            MakeRain(isRaining);
            StartCoroutine(Raining());
            tilemapEffectManager.SetFlowerSpreading(0.7f);

        } else if (isRaining && progressionPerc < (rainProgressionPerc - 0.05))
        {
            isRaining = false;
            MakeRain(isRaining);
            StopAllCoroutines();
            tilemapEffectManager.SetFlowerSpreading(3f);
        }
        if (progressionPerc <= loseProgressionPerc)
            stageManager.GameOver("heat");
    }

    public void MakeRain(bool isRaining)
    {
        if (isRaining)
        {
            rainEffect.Play();
            if (decorationManager != null)
                decorationManager.SetGreenSprites();
        }
        else
            rainEffect.Stop();
    }

    IEnumerator Raining()
    {
        while(true)
        {
            Vector3Int raindropPos;
            bool isOnPlayground;
            do
            {
                raindropPos = new Vector3Int(UnityEngine.Random.Range(0, maxX), UnityEngine.Random.Range(0, maxY), 0);
                Vector3 cellCenter = walkTilemap.GetCellCenterWorld(raindropPos);
                isOnPlayground = IsOnPlayground(cellCenter);
            }while(!isOnPlayground);
            AddWaterdrop(raindropPos);
            Debug.Log("Rain on position " + raindropPos);
            yield return new WaitForSeconds(rainInterval);
        }
    }

    public void ShowEnergy()
    {
        foreach(Transform child in flameParent.transform)
        {
            if(child.gameObject.CompareTag("Flame"))
                child.GetComponent<EnergyIndicator>().ShowEnergy();
            if(child.gameObject.CompareTag("Enemy"))
                child.GetComponent<EnergyIndicator>().ShowEnergy();
        }
        foreach(Transform child in waterdropParent.transform)
        {
            if(child.gameObject.CompareTag("Waterdrop"))
                child.GetComponent<EnergyIndicator>().ShowEnergy();
        }
        stageManager.ShowButtonDescription();
    }

    public void HideEnergy()
    {
        foreach(Transform child in flameParent.transform)
        {
            if(child.gameObject.CompareTag("Flame"))
                child.GetComponent<EnergyIndicator>().HideEnergy();
            if(child.gameObject.CompareTag("Enemy"))
                child.GetComponent<EnergyIndicator>().HideEnergy();
        }
        foreach(Transform child in waterdropParent.transform)
        {
            if(child.gameObject.CompareTag("Waterdrop"))
                child.GetComponent<EnergyIndicator>().HideEnergy();
        }
        stageManager.HideButtonDescription();
    }
}
