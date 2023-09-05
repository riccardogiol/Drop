using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlaygroundManager : MonoBehaviour
{
    public StageManager stageManager;

    public Tilemap walkTilemap;
    public Tilemap wallTilemap;

    public GameObject flameParent;
    public GameObject waterdropParent;

    public GameObject flamePrefab;
    public GameObject waterdropPrefab;


    private int totalTiles = 0;
    private int burntTiles = 0;
    private float fireValue = 0;
    private float progressionPerc = 0;
    public ProgressionBarFiller progressionBar;
    // make it a slider in the unity editor
    public float minProgressionPerc = 0.3f;
    public float loseProgressionPerc = 0.45f;
    public float winProgressionPerc = 0.98f;

    void Start()
    {
        walkTilemap.GetComponent<RuleTileStateManager>().EvaluateTilesState();
        wallTilemap.GetComponent<RuleTileStateManager>().EvaluateTilesState();
        totalTiles = walkTilemap.GetComponent<RuleTileStateManager>().numberTiles();
        totalTiles += wallTilemap.GetComponent<RuleTileStateManager>().numberTiles();
        fireValue = flameParent.GetComponent<FireCounter>().FireValue();
        progressionBar.SetGameOverLimit((loseProgressionPerc - minProgressionPerc) / (1-minProgressionPerc));
        Debug.Log(totalTiles);
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
        Debug.Log("Fire value: " + fireValue);
        EvaluateLevelProgression();
    }

    public void WildfireEstinguished()
    {
        flameParent.GetComponent<FireCounter>().wildfireCounter--;
        fireValue = flameParent.GetComponent<FireCounter>().FireValue();
        Debug.Log("Fire value: " + fireValue);
        EvaluateLevelProgression();
    }

    void EvaluateCleanSurface()
    {
        burntTiles = walkTilemap.GetComponent<RuleTileStateManager>().numberBurntTiles();
        burntTiles += wallTilemap.GetComponent<RuleTileStateManager>().numberBurntTiles();
        Debug.Log(burntTiles);
        EvaluateLevelProgression();
    }

    void EvaluateLevelProgression()
    {
        progressionPerc = 1 - (fireValue + burntTiles)/totalTiles;
        Debug.Log("Progression perc: " + progressionPerc);
        float progressionPercOnMin = (progressionPerc - minProgressionPerc) / (1-minProgressionPerc);
        progressionBar.SetValue(progressionPercOnMin);
        if (progressionPerc >= 0.98)
            stageManager.WinGame();
        if (progressionPerc <= loseProgressionPerc)
            stageManager.GameOver();
    }
}
