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

    void Start()
    {
        walkTilemap.GetComponent<RuleTileStateManager>().EvaluateTilesState();
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
        EvaluateCleanSurface();
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
    }

    public int WaterOnPosition(Vector3 position)
    {
        Vector3Int cell = walkTilemap.WorldToCell(position);
        return WaterCell(cell);
    }

    public int WaterCell(Vector3Int cell)
    {
        int waterDamage = walkTilemap.GetComponent<RuleTileStateManager>().WaterTile(cell);
        if (waterDamage > 0)
        {
            waterDamage += wallTilemap.GetComponent<RuleTileStateManager>().WaterTile(cell);
            EvaluateCleanSurface();
        }
        return waterDamage;
    }

    void EvaluateCleanSurface()
    {
        if (walkTilemap.GetComponent<RuleTileStateManager>().numberBurntTiles() == 0)
            stageManager.WinGame();
    }
}
