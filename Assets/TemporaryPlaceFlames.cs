using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TemporaryPlaceFlames : MonoBehaviour
{
    public Tilemap tilemap;
    public GameObject playground;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cell = tilemap.WorldToCell(worldPosition);
            playground.GetComponent<PlaygroundManager>().AddFlame(cell);
        }
        
    }

    
}
