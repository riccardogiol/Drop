using UnityEngine;
using UnityEngine.Tilemaps;

public class RuleTileStateManager : MonoBehaviour
{
    public RuleTile burntTile;
    public RuleTile cleanTile;

    public int minXCell = 0;
    public int maxXCell = 0;
    public int minYCell = 0;
    public int maxYCell = 0;

    private Tilemap tilemap;

    private int tileNumber;
    private int burntTileNumber;

    public int burntTileDamage;

    // Start is called before the first frame update
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
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
