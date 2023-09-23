using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlaygroundManager : MonoBehaviour
{
    StageManager stageManager;

    Tilemap walkTilemap;
    Tilemap wallTilemap;
    public int maxX = 40;
    public int maxY = 40;

    GameObject flameParent;
    GameObject waterdropParent;
    public GameObject flamePrefab;
    public GameObject waterdropPrefab;

    int totalTiles = 0;
    int burntTiles = 0;
    float fireValue = 0;
    float progressionPerc = 0;
    ProgressionBarFiller progressionBar;
    public float minProgressionPerc = 0.3f;
    public float loseProgressionPerc = 0.45f;
    public float winProgressionPerc = 0.98f;

    void Awake()
    {
        stageManager = FindFirstObjectByType<StageManager>();

        GameObject auxGO = transform.Find("WalkTilemap").gameObject;
        if (auxGO == null)
            return;
        walkTilemap = auxGO.GetComponent<Tilemap>();
        stageManager = FindFirstObjectByType<StageManager>();

        auxGO = transform.Find("WallTilemap").gameObject;
        if (auxGO == null)
            return;
        wallTilemap = auxGO.GetComponent<Tilemap>();

        flameParent = transform.Find("FlameParent").gameObject;
        waterdropParent = transform.Find("WaterdropParent").gameObject;

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
        totalTiles += wallTilemap.GetComponent<RuleTileStateManager>().numberTiles();
        Debug.Log("Total tiles: " + totalTiles);

        progressionBar = FindFirstObjectByType<ProgressionBarFiller>();
        if (progressionBar == null)
        {
            Debug.LogWarning("No progression bar found");
            return;
        }
        progressionBar.SetGameOverLimit(Math.Max((loseProgressionPerc - minProgressionPerc) / (1-minProgressionPerc), 0));
        
        InvokeRepeating(nameof(RefreshCounters), 3, 3);
        EvaluateCleanSurface();
    }

    public void FlameOnPosition(Vector3 position)
    {
        Vector3Int cell = walkTilemap.WorldToCell(position);
        AddFlame(cell);
    }

    public void AddFlame(Vector3Int cell)
    {
        Vector3 cellCenter = walkTilemap.GetCellCenterWorld(cell);
        GameObject newFlame = Instantiate(flamePrefab, cellCenter, Quaternion.identity);
        newFlame.transform.parent = flameParent.transform;
        flameParent.GetComponent<FireCounter>().flameCounter++;
        fireValue = flameParent.GetComponent<FireCounter>().FireValue();

        FindObjectOfType<AudioManager>().Play("FireBurst");
        BurnCellsAround(cell);
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
        walkTilemap.GetComponent<RuleTileStateManager>().BurnTile(cell);
        wallTilemap.GetComponent<RuleTileStateManager>().BurnTile(cell);
        EvaluateCleanSurface();
    }

    public int WaterOnPosition(Vector3 position)
    {
        Vector3Int cell = walkTilemap.WorldToCell(position);
        return WaterCell(cell);
    }

    public int WaterCell(Vector3Int cell)
    {
        int waterDamage = walkTilemap.GetComponent<RuleTileStateManager>().WaterTile(cell);
        waterDamage += wallTilemap.GetComponent<RuleTileStateManager>().WaterTile(cell);
        if (waterDamage > 0)
            EvaluateCleanSurface();
        return waterDamage;
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
        burntTiles += wallTilemap.GetComponent<RuleTileStateManager>().numberBurntTiles();
        EvaluateLevelProgression();
    }

    void EvaluateLevelProgression()
    {
        progressionPerc = 1 - (fireValue + burntTiles)/totalTiles;
        float progressionPercOnMin = (progressionPerc - minProgressionPerc) / (1-minProgressionPerc);
        progressionBar.SetValue(progressionPercOnMin);
        if (progressionPerc >= winProgressionPerc)
            stageManager.WinGame();
        if (progressionPerc <= loseProgressionPerc)
            stageManager.GameOver();
    }

    void RefreshCounters()
    {
        walkTilemap.GetComponent<RuleTileStateManager>().EvaluateTilesState();
        wallTilemap.GetComponent<RuleTileStateManager>().EvaluateTilesState();
        flameParent.GetComponent<FireCounter>().UpdateFireCounters();
        fireValue = flameParent.GetComponent<FireCounter>().FireValue();
        Debug.Log("Burnt tiles: " + burntTiles);
        Debug.Log("Fire value: " + fireValue);
        Debug.Log("Progression perc: " + progressionPerc);
        EvaluateCleanSurface();
    }

    public void ShowEnergy()
    {
        foreach(Transform child in flameParent.transform)
        {
            if(child.gameObject.CompareTag("Flame"))
                child.GetComponent<EnergyIndicator>().ShowEnergy();
        }
        foreach(Transform child in waterdropParent.transform)
        {
            if(child.gameObject.CompareTag("Waterdrop"))
                child.GetComponent<EnergyIndicator>().ShowEnergy();
        }
    }

    public void HideEnergy()
    {
        foreach(Transform child in flameParent.transform)
        {
            if(child.gameObject.CompareTag("Flame"))
                child.GetComponent<EnergyIndicator>().HideEnergy();
        }
        foreach(Transform child in waterdropParent.transform)
        {
            if(child.gameObject.CompareTag("Waterdrop"))
                child.GetComponent<EnergyIndicator>().HideEnergy();
        }
    }
}
