using UnityEngine;
using UnityEngine.Tilemaps;

public class RuleTileStateManager : MonoBehaviour
{
    public RuleTile burntTile;
    public RuleTile cleanTile;

    int minXCell = 0;
    int maxXCell = 40;
    int minYCell = 0;
    int maxYCell = 40;

    private Tilemap tilemap;

    private int tileNumber;
    private int burntTileNumber;

    readonly int burntTileDamage = 0;

    void Awake()
    {
        tilemap = GetComponent<Tilemap>();
    }

    public void SetTilemapLimit(int maxX, int maxY)
    {
        maxXCell = maxX;
        maxYCell = maxY;
    }

    public void EvaluateTilesState()
    {
        tileNumber = 0;
        burntTileNumber = 0;
        for (int y = minYCell; y <= maxYCell; y++)
        {
            for (int x = minXCell; x <= maxXCell; x++)
            {
                RuleTile currentTile = tilemap.GetTile<RuleTile>(new Vector3Int(x, y, 0));
                if (currentTile != null)
                {
                    tileNumber++;
                    if (currentTile == burntTile)
                        burntTileNumber++;
                }
            }
        }
    }

    public int numberBurntTiles()
    {
        return burntTileNumber;
    }

    public int numberTiles()
    {
        return tileNumber;
    }

    public void BurnTile(Vector3Int cell)
    {
        RuleTile currentTile = tilemap.GetTile<RuleTile>(cell);
        if (currentTile == cleanTile)
        {
            tilemap.SetTile(cell, burntTile);
            burntTileNumber++;
            // show fire animation
        }
    }

    public int WaterTile(Vector3Int cell)
    {
        RuleTile currentTile = tilemap.GetTile<RuleTile>(cell);
        if (currentTile == burntTile)
        {
            tilemap.SetTile(cell, cleanTile);
            burntTileNumber--;
            // show watering animation
            return burntTileDamage;
        }
        return 0;
    }
}
