using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelTileDecorationManager : MonoBehaviour
{
    public PolygonCollider2D[] decorationAreas;
    List<Vector3> availablePositions;
    public GameObject[] decorationPrefabs;
    List<GameObject> decorations= new List<GameObject>();
    Tilemap tilemap;
    public RuleTile darkGreenTile;
    public RuleTile burnedTile;
    List<Vector3Int> tilePositions;

    void Awake()
    {
        tilemap = FindFirstObjectByType<Tilemap>();
        availablePositions = new List<Vector3>();
        tilePositions = new List<Vector3Int>();

        foreach ( PolygonCollider2D decorationArea in decorationAreas )
        {
            Bounds bounds = decorationArea.bounds;
            for (int x = (int)bounds.min.x; x < bounds.max.x; x++)
            {
                for (int y = (int)bounds.min.y; y < bounds.max.y; y++)
                {
                    Vector3 worldPosition = tilemap.GetCellCenterWorld(new Vector3Int(x, y, 0));
                    if (decorationArea.OverlapPoint(worldPosition))
                    {
                        availablePositions.Add(worldPosition);
                    }
                }
            }
        }
        

        foreach (Vector3 position in availablePositions)
        {
            if (Random.value < 0.9)
            {    
                GameObject goRef = Instantiate(decorationPrefabs[Random.Range(0, decorationPrefabs.Length)], position, Quaternion.identity);
                goRef.transform.parent = transform;
                decorations.Add(goRef);
                ChangeAspect caRef = goRef.GetComponent<ChangeAspect>();
                if (caRef != null)
                {
                    goRef.GetComponent<ChangeAspect>().SetBurntSprite();
                    goRef.transform.SetPositionAndRotation(new Vector3(goRef.transform.position.x + Random.Range(-0.65f, 0.15f), goRef.transform.position.y + Random.Range(-0.65f, 0.15f)), Quaternion.identity);
                } else if (goRef.GetComponent<PickFlame>() != null)
                {
                    goRef.GetComponent<PickFlame>().isDecoration = true;
                    tilemap.SetTile(new Vector3Int((int)position.x, (int)position.y, 0), burnedTile);
                    tilePositions.Add(new Vector3Int((int)position.x, (int)position.y, 0));
                }
            }

            if (Random.value < 0.5)
            {
                tilemap.SetTile(new Vector3Int((int)position.x, (int)position.y, 0), burnedTile);
                tilePositions.Add(new Vector3Int((int)position.x, (int)position.y, 0));
            }
        }
    }

    public void SetGreenValue(float value)
    {   
        if (value <= 0.0f)
            return;
        if (value >= 1.0f)
        {
            foreach ( GameObject go in decorations)
            {
                if (go.GetComponent<ChangeAspect>() != null)
                {
                    go.GetComponent<ChangeAspect>().SetGreenSprite();
                    go.GetComponent<ChangeAspect>().ColorAdjustment(Random.Range(-0.05f, 0.05f), 0.76f);
                }
                else if(go.GetComponent<PickFlame>() != null)
                    Destroy(go);
            }

            foreach ( Vector3Int position  in tilePositions)
                tilemap.SetTile(new Vector3Int(position.x, position.y, 0), darkGreenTile);

            return;
        }
        foreach ( GameObject go in decorations)
        {
            if (Random.value < value)
            {
                Debug.Log("New random value");
                if (go.GetComponent<ChangeAspect>() != null)
                {
                    go.GetComponent<ChangeAspect>().SetGreenSprite();
                    go.GetComponent<ChangeAspect>().ColorAdjustment(Random.Range(-0.05f, 0.05f), 0.76f);
                }
                else if(go.GetComponent<PickFlame>() != null)
                    Destroy(go);
            }
        }

        foreach ( Vector3 position  in tilePositions)
            if (Random.value < value)
                tilemap.SetTile(new Vector3Int((int)position.x, (int)position.y, 0), darkGreenTile);
    }
}
