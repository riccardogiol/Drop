using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class OutOfWallDecorationManager : MonoBehaviour
{
    public List<GameObject> decorationsPrefabs;
    public List<GameObject> tallDecorationsPrefabs;
    List<GameObject> burntDecorations;
    List<GameObject> cleanDecorations;
    public RuleTileStateManager walkTileStateManager;
    public RuleTileStateManager wallTileStateManager;

    bool[,] availableTiles;
    bool[,] availableTilesTall;

    float currentCleanValue = 0;
    int amountOfDecorations;

    float newValue;
    bool firstTime = true;

    float timer = 2;
    float countdown = 0;

    void Awake()
    {
        burntDecorations = new List<GameObject>();
        cleanDecorations = new List<GameObject>();
        countdown = timer;
    }

    public void SpawnDecorations(int maxX, int maxY)
    {
        availableTiles = new bool[maxX + 24, maxY + 16];
        availableTilesTall = new bool[maxX + 24, maxY + 16];
        for (int y = -5; y <= maxY + 5; y++)
        {
            for (int x = -10; x <= maxX + 7; x ++)
            {
                RuleTile currentTile = walkTileStateManager.GetTile(new Vector3Int(x, y, 0));
                if (currentTile == null)
                    SetAvailableTile(x, y, true);
                else
                {
                    SetAvailableTile(x, y, false);
                    SetAvailableTileTall(x, y - 1, false);
                    SetAvailableTileTall(x, y - 2, false);
                }
            }
        }
        for (int y = -5; y <= maxY + 5; y++)
        {
            for (int x = -10; x <= maxX + 5; x ++)
            {
                if (GetAvailableTile(x, y))
                {
                    if (UnityEngine.Random.value < 0.6)
                        SpawnRandomDecoration(x, y);   
                }
            }
        }
        amountOfDecorations = burntDecorations.Count;
    }
    
    void SpawnRandomDecoration(int x, int y)
    {
        if (UnityEngine.Random.value < 0.5)
        {
            wallTileStateManager.SetCleanTile(new Vector3Int(x, y));
            SetAvailableTile(x, y, false);
            return;
        }

        GameObject deco;
        if (GetAvailableTileTall(x, y))
        {
            if (UnityEngine.Random.value < 0.6)
                deco = tallDecorationsPrefabs[UnityEngine.Random.Range(0, tallDecorationsPrefabs.Count)];
            else
                deco = decorationsPrefabs[UnityEngine.Random.Range(0, decorationsPrefabs.Count)];
        }
        else
        {
            deco = decorationsPrefabs[UnityEngine.Random.Range(0, decorationsPrefabs.Count)];
        }
        foreach (int2 point in deco.GetComponent<ChangeAspect>().touchingCellsCoordinates)
        {
            if (!GetAvailableTile(x + point.x, y + point.y))
                return;
        }
        GameObject goRef = Instantiate(deco, new Vector3(x, y), Quaternion.identity);
        deco.GetComponent<ChangeAspect>().SetBurntSprite(false);
        if (UnityEngine.Random.value > 0.5)
            deco.GetComponent<ChangeAspect>().FlipX();
        goRef.transform.parent = transform;
        burntDecorations.Add(goRef);
        foreach(int2 point in deco.GetComponent<ChangeAspect>().touchingCellsCoordinates)
            SetAvailableTile(x + point.x, y + point.y, false);
    }

    void SetAvailableTile(int x, int y, bool state)
    {
        availableTiles[x +10, y+5] = state;
        availableTilesTall[x +10, y+5] = state;
    }

    void SetAvailableTileTall(int x, int y, bool state)
    {
        availableTilesTall[x +10, y+5] = state;
    }

    bool GetAvailableTile(int x, int y)
    {
        return availableTiles[x +10, y+5];
    }

    bool GetAvailableTileTall(int x, int y)
    {
        return availableTilesTall[x +10, y+5];
    }

    void Update()
    {
        countdown += Time.deltaTime;
        if (countdown > timer)
        {
            countdown = 0;
            UpdateGraphic();
            if (firstTime)
                firstTime = false;
        }

    }

    void UpdateGraphic()
    {
        Debug.Log("Value and initialisation:" + newValue + " " + firstTime);
        float valueDiff = newValue - currentCleanValue;
        if (valueDiff > 0)
        {
            int decoToClean = (int)math.floor(valueDiff * amountOfDecorations);
            for (int i = 0; i < decoToClean && burntDecorations.Count > 0; i ++)
            {
                int index = UnityEngine.Random.Range(0, burntDecorations.Count);
                GameObject deco = burntDecorations[index];
                deco.GetComponent<ChangeAspect>().SetGreenSprite(false, !firstTime);
                cleanDecorations.Add(deco);
                burntDecorations.Remove(deco);
            }
            currentCleanValue = (float)cleanDecorations.Count / (float)amountOfDecorations;
        } else
        {
            int decoToBurn = (int)math.floor((-valueDiff) * amountOfDecorations);
            for (int i = 0; i < decoToBurn && cleanDecorations.Count > 0; i ++)
            {
                int index = UnityEngine.Random.Range(0, cleanDecorations.Count);
                GameObject deco = cleanDecorations[index];
                deco.GetComponent<ChangeAspect>().SetBurntSprite(!firstTime);
                burntDecorations.Add(deco);
                cleanDecorations.Remove(deco);
            }
            currentCleanValue = (float)cleanDecorations.Count / (float)amountOfDecorations;
        }
    }

    public void SetCleanValue(float value)
    {
        newValue = value;
    }
}
