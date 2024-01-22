using UnityEngine;
using UnityEngine.Tilemaps;

public class RuleTileStateManager : MonoBehaviour
{
    public RuleTile burntTile;
    public RuleTile cleanTile;
    public bool chessStyle = false;
    public RuleTile cleanDarkTile;

    int minXCell = 0;
    int maxXCell = 40;
    int minYCell = 0;
    int maxYCell = 40;

    private Tilemap tilemap;

    private int tileNumber;
    private int burntTileNumber;

    readonly int burntTileDamage = 0;

    public GameObject particleCollider;

    void Awake()
    {
        tilemap = GetComponent<Tilemap>();
    }
    
    public void SpawnParticleColliders()
    {
        if (particleCollider == null)
            return;
        for (int y = minYCell; y <= maxYCell; y++)
        {
            for (int x = minXCell; x <= maxXCell; x++)
            {
                RuleTile currentTile = tilemap.GetTile<RuleTile>(new Vector3Int(x, y, 0));
                if (currentTile != null)
                {
                    if (currentTile != burntTile)
                        SpawnParticleCollider(new Vector3(x + 0.5f, y + 0.5f), false); 
                    else
                        SpawnParticleCollider(new Vector3(x + 0.5f, y + 0.5f), true);   
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
        goRef.GetComponent<TileFlowerManager>().walkTilemap = this;
    }

    public void SetTilemapLimit(int maxX, int maxY)
    {
        maxXCell = maxX;
        maxYCell = maxY;
        for (int y = minYCell; y <= maxYCell; y++)
        {
            for (int x = minXCell; x <= maxXCell; x++)
            {
                RuleTile currentTile = tilemap.GetTile<RuleTile>(new Vector3Int(x, y, 0));
                if (currentTile != null)
                {
                    if (currentTile != burntTile)
                        SetCleanTile(new Vector3Int(x, y, 0));
                }
            }
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

    public void BurnTile(Vector3Int cell)
    {
        RuleTile currentTile = tilemap.GetTile<RuleTile>(cell);
        if (currentTile != null && currentTile != burntTile)
        {
            tilemap.SetTile(cell, burntTile);
            burntTileNumber++;
            // show fire animation
            GameObject particleCollider = GetParticleCollider(tilemap.CellToWorld(cell) + new Vector3(0.5f, 0.5f));
            if (particleCollider == null)
                return;
            particleCollider.GetComponent<TileParticlesManager>().ActivateBurntParticle();
            StopFloweringAllAround(tilemap.CellToWorld(cell) + new Vector3(0.5f, 0.5f));
        }
    }

    public int WaterTile(Vector3Int cell)
    {
        RuleTile currentTile = tilemap.GetTile<RuleTile>(cell);
        if (currentTile == burntTile)
        {
            SetCleanTile(cell);
            burntTileNumber--;
            // show watering animation
            GameObject particleCollider = GetParticleCollider(tilemap.CellToWorld(cell) + new Vector3(0.5f, 0.5f));
            if (particleCollider != null)
                particleCollider.GetComponent<TileParticlesManager>().DesactivateBurntParticle();
            return burntTileDamage;
        }
        return 0;
    }

    void SetCleanTile(Vector3Int cell)
    {
        if (chessStyle)
        {
            if ((cell.x + cell.y) % 2 == 0)
            {
                tilemap.SetTile(cell, cleanTile);
            } else {
                tilemap.SetTile(cell, cleanDarkTile);
            }
        } else {
            tilemap.SetTile(cell, cleanTile);
        }
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

    public bool IsTileBurnt(Vector3 onCellPoint)
    {
        RuleTile currentTile = tilemap.GetTile<RuleTile>(tilemap.WorldToCell(onCellPoint));
        if (currentTile != null && currentTile == burntTile)
            return true;
        return false;
    }
}
