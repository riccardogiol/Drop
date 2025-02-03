using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelTileDecorationManager : MonoBehaviour
{
    public PolygonCollider2D[] decorationAreas;
    public GameObject[] decorationPrefabs;
    List<GameObject> decorations= new List<GameObject>();
    Tilemap tilemap;
    public RuleTile darkGreenTile;
    public RuleTile burnedTile;
    List<Vector3Int> tilePositions;

    void Awake()
    {
        tilemap = FindFirstObjectByType<Tilemap>();
        List<Vector3> availablePositions = new List<Vector3>();
        tilePositions = new List<Vector3Int>();

        foreach (Transform child in transform)
            if (child.GetComponent<ChangeAspect>() != null)
            {
                child.GetComponent<ChangeAspect>().SetBurntSprite();
                decorations.Add(child.gameObject);
            }
        
        foreach ( PolygonCollider2D decorationArea in decorationAreas )
        {
            Bounds bounds = decorationArea.bounds;
            for (float x = bounds.min.x; x < bounds.max.x; x += 0.25f)
            {
                for (float y = bounds.min.y; y < bounds.max.y; y += 0.25f)
                {
                    Vector3 worldPosition = new Vector3(x, y);
                    if (decorationArea.OverlapPoint(worldPosition))
                    {
                        availablePositions.Add(worldPosition);
                    }
                }
            }
        }


        foreach (Vector3 position in availablePositions)
        {
            if (Random.value < 0.2)
            {    
                GameObject goRef = Instantiate(decorationPrefabs[Random.Range(0, decorationPrefabs.Length)], position, Quaternion.identity);
                goRef.transform.parent = transform;
                decorations.Add(goRef);
                ChangeAspect caRef = goRef.GetComponent<ChangeAspect>();
                if (caRef != null)
                    goRef.GetComponent<ChangeAspect>().SetBurntSprite();
                else if (goRef.GetComponent<PickFlame>() != null)
                {
                    goRef.GetComponent<PickFlame>().isDecoration = true;
                    tilemap.SetTile(new Vector3Int((int)position.x, (int)position.y, 0), burnedTile);
                    tilePositions.Add(new Vector3Int((int)position.x, (int)position.y, 0));
                }
            }

            if (Random.value < -0.5)
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
