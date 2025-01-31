using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class OutOfWallDecorationManager : MonoBehaviour
{
    public List<GameObject> decorationsPrefabs;
    public List<GameObject> tallDecorationsPrefabs;
    public List<GameObject> levelDecorationsPrefabs;
    public List<int> levelDecorationsUnlockCode;
    public List<GameObject> levelTallDecorationsPrefabs;
    public List<int> levelTallDecorationsUnlockCode;
    public bool randomOOGridPos = false;
    public float decorationDensity = 0.6f;
    public float decorationBrightness = 0.84f;
    List<GameObject> burntDecorations;
    List<GameObject> cleanDecorations;
    public RuleTileStateManager walkTileStateManager;
    public RuleTileStateManager wallTileStateManager;
    public Tilemap outOfWallGrass;
    public RuleTile GrassTile;
    public RuleTile DarkGrassTile;

    bool[,] availableTiles;
    bool[,] availableTilesTall;

    int maxX = 0, maxY = 0;

    float currentCleanValue = 0;
    int amountOfDecorations;

    float newValue;
    bool firstTime = true;

    float timer = 2;
    float countdown = 0;

    void Awake()
    {
        int index = 0;
        foreach(int lvlCode in levelDecorationsUnlockCode)
        {
            if (PlayerPrefs.GetInt("Lvl" + lvlCode, 0) == 1)
            {
                decorationsPrefabs.Add(levelDecorationsPrefabs[index]);
            }
            index ++;
        }
        index = 0;
        foreach(int lvlCode in levelTallDecorationsUnlockCode)
        {
            if (PlayerPrefs.GetInt("Lvl" + lvlCode, 0) == 1)
            {
                tallDecorationsPrefabs.Add(levelTallDecorationsPrefabs[index]);
            }
            index ++;
        }
        burntDecorations = new List<GameObject>();
        cleanDecorations = new List<GameObject>();
        countdown = timer;
        firstTime = true;
        currentCleanValue = 0;
    }

    public void SpawnDecorations(int maxX, int maxY)
    {
        availableTiles = new bool[maxX + 24, maxY + 16];
        availableTilesTall = new bool[maxX + 24, maxY + 16];
        this.maxX = maxX;
        this.maxY = maxY;
        for (int y = -5; y <= maxY + 5; y++)
        {
            for (int x = -10; x <= maxX + 7; x ++)
            {
                RuleTile currentTile = walkTileStateManager.GetTile(new Vector3Int(x, y, 0));
                if (currentTile == null)
                {
                    SetAvailableTile(x, y, true);
                    if (UnityEngine.Random.value < 0.5)
                        outOfWallGrass.SetTile(new Vector3Int(x, y), GrassTile);
                    else
                        outOfWallGrass.SetTile(new Vector3Int(x, y), DarkGrassTile);

                }
                else
                {
                    SetAvailableTile(x, y, false);
                    SetAvailableTileTall(x, y - 1, false);
                    SetAvailableTileTall(x, y - 2, false);
                    SetAvailableTileTall(x, y - 3, false);
                }
            }
        }
        foreach(Transform child in transform)
        {
            ChangeAspect caComp = child.GetComponent<ChangeAspect>();
            if (caComp != null)
            {
                foreach(int2 point in caComp.touchingCellsCoordinates)
                    SetAvailableTile((int)child.position.x + point.x, (int)child.position.y + point.y, false);
            }
        }

        for (int y = -5; y <= maxY + 5; y++)
        {
            for (int x = -10; x <= maxX + 5; x ++)
            {
                if (GetAvailableTile(x, y))
                {
                    if (UnityEngine.Random.value < decorationDensity)
                        SpawnRandomDecoration(x, y);   
                }
            }
        }
        amountOfDecorations = burntDecorations.Count;
    }
    
    void SpawnRandomDecoration(int x, int y)
    {
        if (wallTileStateManager != null && UnityEngine.Random.value < 0.5)
        {
            wallTileStateManager.SetCleanTile(new Vector3Int(x, y));
            SetAvailableTile(x, y, false);
            return;
        }

        GameObject deco;
        if (GetAvailableTileTall(x, y))
        {
            if (UnityEngine.Random.value < 0.6)
            {
                deco = tallDecorationsPrefabs[UnityEngine.Random.Range(0, tallDecorationsPrefabs.Count)];
                SetAvailableTileTall(x, y+1, false);

            }
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
        Vector3 position = new Vector3(x, y);
        if (randomOOGridPos)
            position += new Vector3(UnityEngine.Random.value - 0.5f, UnityEngine.Random.value - 0.5f);
        GameObject goRef = Instantiate(deco, position, Quaternion.identity);
        float currentBrightness = decorationBrightness;
        if (x < 0)
            currentBrightness -= (-x)*0.05f;
        if (x > maxX)
            currentBrightness -= (x-maxX)*0.05f;
        if (y < 0)
            currentBrightness -= (-y)*0.05f;
        if (y > maxY)
            currentBrightness -= (y-maxY)*0.05f;
        goRef.GetComponent<ChangeAspect>().ColorAdjustment(UnityEngine.Random.Range(-0.05f, 0.05f), currentBrightness);
        goRef.GetComponent<ChangeAspect>().SetBurntSprite(false);
        if (UnityEngine.Random.value > 0.5)
            goRef.GetComponent<ChangeAspect>().FlipX();
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
        if (y < -5)
            return;
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
