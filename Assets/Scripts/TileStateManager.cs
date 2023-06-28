using UnityEngine;
using UnityEngine.Tilemaps;

public class TileStateManager : MonoBehaviour
{
    public Tile burntTile;
    public Tile greenTile;
    private Tilemap tilemap;

    public int burntTileDamage;

    // Start is called before the first frame update
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
    }

    public void BurnTile(Vector3Int cell)
    {
        Tile currentTile = tilemap.GetTile<Tile>(cell);
        if (currentTile)
        {
            tilemap.SetTile(cell, burntTile);
            // show fire animation
        }
    }

    public int WaterTile(Vector3Int cell)
    {
        Tile currentTile = tilemap.GetTile<Tile>(cell);
        if (currentTile == burntTile)
        {
            tilemap.SetTile(cell, greenTile);
            // show fire animation
            return burntTileDamage;
        }
        return 0;
    }
}
