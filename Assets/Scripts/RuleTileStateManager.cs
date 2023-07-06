using UnityEngine;
using UnityEngine.Tilemaps;

public class RuleTileStateManager : MonoBehaviour
{
    public RuleTile burntTile;
    public RuleTile cleanTile;
    private Tilemap tilemap;

    public int burntTileDamage;

    // Start is called before the first frame update
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
    }

    public void BurnTile(Vector3Int cell)
    {
        RuleTile currentTile = tilemap.GetTile<RuleTile>(cell);
        if (currentTile)
        {
            tilemap.SetTile(cell, burntTile);
            // show fire animation
        }
    }

    public int WaterTile(Vector3Int cell)
    {
        RuleTile currentTile = tilemap.GetTile<RuleTile>(cell);
        if (currentTile == burntTile)
        {
            tilemap.SetTile(cell, cleanTile);
            // show watering animation
            return burntTileDamage;
        }
        return 0;
    }
}
