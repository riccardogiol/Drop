using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlaygroundManager : MonoBehaviour
{
    public Tilemap walkTilemap;
    public Tilemap wallTilemap;

    public GameObject flameParent;
    public GameObject waterdropParent;

    public GameObject flamePrefab;
    public GameObject waterdropPrefab;

    private int[,] tileStates;

    void Start()
    {
        tileStates = new int[10,10];
    }

    void Update()
    {
        
    }

    public void AddFlame(Vector3Int cell)
    {
        Debug.Log("New flame in cell " + cell);
        Vector3 cellCenter = walkTilemap.GetCellCenterWorld(cell);
        GameObject newFlame = Instantiate(flamePrefab, cellCenter, Quaternion.identity);
        newFlame.transform.parent = flameParent.transform;

        BurnCellsAround(cell);
    }

    public void BurnCellsAround(Vector3Int cell)
    {
        for (int x = cell.x - 1; x <= cell.x + 1; x++)
        {
            for (int y = cell.y - 1; y <= cell.y + 1; y++)
            {
                BurnCell(new Vector3Int(x, y, 0));
            }
        }
    }

    public void BurnCell(Vector3Int cell)
    {
        walkTilemap.GetComponent<TileStateManager>().BurnTile(cell);
        wallTilemap.GetComponent<TileStateManager>().BurnTile(cell);

    }

    public int WaterOnPosition(Vector3 position)
    {
        Vector3Int cell = walkTilemap.WorldToCell(position);
        return WaterCell(cell);
    }

    public int WaterCell(Vector3Int cell)
    {
        int waterDamage = walkTilemap.GetComponent<TileStateManager>().WaterTile(cell);
        waterDamage += wallTilemap.GetComponent<TileStateManager>().WaterTile(cell);
        return waterDamage;
    }
}
