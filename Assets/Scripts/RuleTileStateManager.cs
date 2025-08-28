using UnityEngine;
using UnityEngine.Tilemaps;

public class RuleTileStateManager : MonoBehaviour
{
    public RuleTile burntTile;
    public RuleTile cleanTile;
    public RuleTile riverBorderTile;
    public bool chessStyle = false;
    public RuleTile cleanDarkTile;

    int minXCell = 0;
    int maxXCell = 40;
    int minYCell = 0;
    int maxYCell = 40;

    private Tilemap tilemap;

    private int tileNumber;
    private int burntTileNumber;

    TilemapEffectManager tilemapEffectManager;
    TilemapWallEffectManager tilemapWallEffectManager;

    void Awake()
    {
        tilemap = GetComponent<Tilemap>();
        tilemapEffectManager = GetComponent<TilemapEffectManager>();
        tilemapWallEffectManager = GetComponent<TilemapWallEffectManager>();
    }

    public void SetTilemapLimit(int maxX, int maxY)
    {
        maxXCell = maxX;
        maxYCell = maxY;
        if (chessStyle)
            for (int y = minYCell; y <= maxYCell; y++)
                for (int x = minXCell; x <= maxXCell; x++)
                {
                    RuleTile currentTile = tilemap.GetTile<RuleTile>(new Vector3Int(x, y, 0));
                    if (currentTile != null)
                        if (currentTile != burntTile)
                            SetCleanTile(new Vector3Int(x, y, 0));
                }
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

    public bool BurnTile(Vector3Int cell)
    {
        RuleTile currentTile = tilemap.GetTile<RuleTile>(cell);
        if (currentTile != null && currentTile != burntTile)
        {
            tilemap.SetTile(cell, burntTile);
            burntTileNumber++;
            if (tilemapEffectManager != null)
                tilemapEffectManager.BurnTile(tilemap.CellToWorld(cell) + new Vector3(0.5f, 0.5f));
            if (tilemapWallEffectManager != null)
                tilemapWallEffectManager.BurnTile(tilemap.CellToWorld(cell) + new Vector3(0.5f, 0.5f));
            return true;
        }
        return false;
    }

    public bool WaterTile(Vector3Int cell)
    {
        RuleTile currentTile = tilemap.GetTile<RuleTile>(cell);
        if (currentTile == burntTile)
        {
            SetCleanTile(cell);
            burntTileNumber--;
            if (tilemapEffectManager != null)
                tilemapEffectManager.WaterTile(tilemap.CellToWorld(cell) + new Vector3(0.5f, 0.5f));
            if (tilemapWallEffectManager != null)
                tilemapWallEffectManager.WaterTile(tilemap.CellToWorld(cell) + new Vector3(0.5f, 0.5f));
            return true;
        }
        return false;
    }

    public void RemoveTile(Vector3Int cell)
    {
        tilemap.SetTile(cell, null);
        // play some PS
    }

    public void SetCleanTile(Vector3Int cell)
    {
        if (tilemap.GetTile<RuleTile>(cell) == riverBorderTile)
            return;
        if (chessStyle)
        {
            if ((cell.x + cell.y) % 2 == 0)
            {
                tilemap.SetTile(cell, cleanTile);
            }
            else
            {
                tilemap.SetTile(cell, cleanDarkTile);
            }
        }
        else
        {
            tilemap.SetTile(cell, cleanTile);
        }
    }

    public RuleTile GetTile(Vector3Int cell)
    {
        return tilemap.GetTile<RuleTile>(cell);
    }

    public bool IsTileBurnt(Vector3 onCellPoint)
    {
        RuleTile currentTile = tilemap.GetTile<RuleTile>(tilemap.WorldToCell(onCellPoint));
        if (currentTile != null && currentTile == burntTile)
            return true;
        return false;
    }

    public bool IsTileBurnt(RuleTile ruleTile)
    {
        return ruleTile == burntTile;
    }
}
