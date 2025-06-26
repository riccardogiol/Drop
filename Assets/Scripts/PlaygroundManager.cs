using System;
using System.Collections;
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
    OutOfWallDecorationManager OOWdecorationManager;
    ParticleSystem rainEffect;
    public GameObject flamePrefab;
    public GameObject waterdropPrefab;

    int totalTiles = 0;
    int burntTiles = 0;
    float fireValue = 0;
    float progressionPerc = 0;
    bool isRaining = false;
    bool isSuper = false;
    RainManager rainManager;
    ProgressionBarFiller progressionBar;
    public float minProgressionPerc = 0.3f;
    public float loseProgressionPerc = 0.45f;
    public float rainProgressionPerc = 1.0f;
    public float winProgressionPerc = 0.98f;
    public bool bossWin = false;

    //public bool winByFlower = true;

    bool reachedWinningCondition;

    void Awake()
    {
        stageManager = FindFirstObjectByType<StageManager>();
        cameraEffectManager = FindFirstObjectByType<CameraEffectManager>();
        progressionBar = FindFirstObjectByType<ProgressionBarFiller>();
        rainManager = FindFirstObjectByType<RainManager>();


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
        OOWdecorationManager = FindObjectOfType<OutOfWallDecorationManager>();
        
        if (PlayerPrefs.GetInt("EasyMode", 0) == 1)
            loseProgressionPerc -= 0.05f;

        reachedWinningCondition = false;
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

        if (!bossWin)
        {
            progressionBar.SetGameOverLimit(Math.Max((loseProgressionPerc - minProgressionPerc) / (1-minProgressionPerc), 0));
            progressionBar.SetRainLimit((rainProgressionPerc - minProgressionPerc) / (1-minProgressionPerc));
        }
        
        tilemapEffectManager = walkTilemap.GetComponent<TilemapEffectManager>();
        tilemapEffectManager.SpawnParticleColliders(maxX, maxY);
        tilemapWallEffectManager = wallTilemap.GetComponent<TilemapWallEffectManager>();
        if(tilemapWallEffectManager != null)
            tilemapWallEffectManager.SpawnParticleColliders(maxX, maxY);
        if(OOWdecorationManager != null)
            OOWdecorationManager.SpawnDecorations(maxX, maxY);

        EvaluateCleanSurface();
        StartCoroutine(StartFlowerCounter());
    }

    IEnumerator StartFlowerCounter()
    {
        yield return new WaitForSeconds(1);
        tilemapEffectManager.CollectFlowerTiles();
    }

    public void FlameOnPosition(Vector3 position, int energy = 0, bool randomMovement = false, bool compleatlyRandom = true)
    {
        Vector3Int cell = walkTilemap.WorldToCell(position);
        AddFlame(cell, energy, randomMovement, compleatlyRandom);
    }

    public void AddFlame(Vector3Int cell, int energy = 0, bool randomMovement = false, bool compleatlyRandom = true)
    {
        Vector3 cellCenter = walkTilemap.GetCellCenterWorld(cell);
        GameObject auxFlame = GetFlameInPosition(cellCenter);
        if (auxFlame == null)
        {
            GameObject newFlame = Instantiate(flamePrefab, cellCenter, Quaternion.identity);
            if (energy > 0)
            {
                newFlame.GetComponent<PickFlame>().energy = energy;
                newFlame.GetComponent<PickFlame>().ScaleOnEnergy();
            }
            if (randomMovement)
            {
                newFlame.GetComponent<RandomMovement>().enabled = true;
                newFlame.GetComponent<RandomMovement>().completelyRandom = compleatlyRandom;

            }
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

    public void SubscribeFlame(GameObject flame)
    {
        flame.transform.parent = flameParent.transform;
        flameParent.GetComponent<FireCounter>().flameCounter++;
        fireValue = flameParent.GetComponent<FireCounter>().FireValue();
        FindObjectOfType<AudioManager>().Play("FireBurst");
        BurnCellsAround(walkTilemap.WorldToCell(flame.transform.position));
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

    public void SubscribeWaterdrop(GameObject waterdrop)
    {
        waterdrop.transform.parent = waterdropParent.transform;
        //FindObjectOfType<AudioManager>().Play("FireBurst");
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
    public bool IsOnPlayground(Vector3 cellCenter)
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
            if (item.gameObject.CompareTag("Enemy"))
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
            //if (item.gameObject.CompareTag("OneWayCollider"))
                //return true;
        }
        return false;
    }

    public bool IsObstacleForWalk(Vector3 onCellPoint)
    {
        Collider2D[] results = Physics2D.OverlapPointAll(onCellPoint);
        foreach(Collider2D item in results)
        {
            if (item.gameObject.CompareTag("MovingRock"))
                return false;
            if (item.gameObject.layer == 6)
                return true;
        }
        return false;
    }

    public bool IsObstacleForRock(Vector3 onCellPoint)
    {
        Collider2D[] results = Physics2D.OverlapPointAll(onCellPoint);
        foreach(Collider2D item in results)
        {
            if (item.gameObject.layer == 6)
                return true;
            if (item.gameObject.CompareTag("Waterbomb"))
                return true;
            if (item.gameObject.CompareTag("Enemy"))
                return true;
            //if (item.gameObject.CompareTag("OneWayCollider"))
                //return true;
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
            if (item.gameObject.CompareTag("Decoration"))
            {
                if (item.gameObject.GetComponent<ChangeAspect>() != null)
                    if (!item.gameObject.GetComponent<ChangeAspect>().reactOnWater)
                        return;
                
                if (item.gameObject.GetComponent<RootTriggerLogic>() != null)
                   if (!item.gameObject.transform.parent.GetComponent<ChangeAspect>().reactOnWater)
                   {
                        Debug.Log("GET INTO CONDITION NO BURN");
                        return;
                   }
            }
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

    public void FlameGenerated()
    {
        flameParent.GetComponent<FireCounter>().flameCounter++;
        fireValue = flameParent.GetComponent<FireCounter>().FireValue();
        EvaluateLevelProgression();
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
        if (!bossWin)
            progressionBar.SetValue(progressionPercOnMin);

        if (cameraEffectManager != null)
        {
            cameraEffectManager.SetEffect(progressionPercOnMin);
        }
        if (OOWdecorationManager != null)
        {
            OOWdecorationManager.SetCleanValue(progressionPercOnMin);
        }
        
        Debug.Log("Burnt tiles: " + burntTiles);
        Debug.Log("Fire value: " + fireValue);
        Debug.Log("Progression perc: " + progressionPerc);
        if (progressionPerc >= winProgressionPerc && !reachedWinningCondition)
        {
            StopAllCoroutines();
            isRaining = true;
            reachedWinningCondition = true;
            tilemapEffectManager.SetFlowerSpreading(0.5f);
            if (!bossWin)
                stageManager.WinGame();
        }

        if (!isRaining && progressionPerc > (rainProgressionPerc + 0.05))
        {
            isRaining = true;
            MakeRain(isRaining, false, true, false);
            tilemapEffectManager.SetFlowerSpreading(0.5f);

        } else if (isRaining && progressionPerc < (rainProgressionPerc - 0.05) && !isSuper && !reachedWinningCondition)
        {
            isRaining = false;
            MakeRain(isRaining);
            tilemapEffectManager.SetFlowerSpreading(3f);
        }

        if (progressionPerc <= loseProgressionPerc)
        {
            if (!bossWin)
                stageManager.GameOver("heat");
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
        foreach(Transform child in decorationManager.transform)
        {
            if(child.gameObject.CompareTag("Superdrop"))
                child.GetComponent<EnergyIndicator>().ShowEnergy();
            if(child.gameObject.CompareTag("DecorationNoFire"))
            {
                if (child.gameObject.GetComponent<EnergyIndicator>() != null)
                    child.GetComponent<EnergyIndicator>().ShowEnergy();
            }
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
        foreach(Transform child in decorationManager.transform)
        {
            if(child.gameObject.CompareTag("Superdrop"))
                child.GetComponent<EnergyIndicator>().HideEnergy();
            if(child.gameObject.CompareTag("DecorationNoFire"))
            {
                if (child.gameObject.GetComponent<EnergyIndicator>() != null)
                    child.GetComponent<EnergyIndicator>().HideEnergy();
            }
        }
        stageManager.HideButtonDescription();
    }

    public void EstinguishAllFlames()
    {
        foreach(Transform child in flameParent.transform)
        {
            if (child.GetComponent<PickFlame>() != null)
                child.GetComponent<PickFlame>().DestroyFlame();
        }
    }

    public void SetGreenDecorations()
    {
        if (decorationManager != null)
            decorationManager.SetGreenSprites();
    }

    public void MakeRain(bool rainingState, bool waterTiles = false, bool spawnWaterdrops = false, bool win = true, bool super = false)
    {
        if (win)
            reachedWinningCondition = true;
        isSuper = rainingState && super;
        if (rainingState == false)
        {
            if (progressionPerc > (rainProgressionPerc + 0.05))
            {
                isRaining = true;
                MakeRain(true, false, true, false);
                tilemapEffectManager.SetFlowerSpreading(0.5f);
                return;
            }
        }

        isRaining = rainingState;
        rainManager.MakeRain(rainingState, waterTiles, spawnWaterdrops, win);
    }
}
