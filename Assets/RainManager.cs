using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RainManager : MonoBehaviour
{
    PlaygroundManager playgroundManager;
    int maxX, maxY;

    float rainInterval = 3.0f;
    ParticleSystem rainEffect;
    public Image lighthningFlashImage;

    void Awake()
    {
        playgroundManager = FindFirstObjectByType<PlaygroundManager>();
        maxX = playgroundManager.maxX;
        maxY = playgroundManager.maxY;
        GameObject rainGO = transform.Find("RainEffect").gameObject;
        if(rainGO!= null)
            rainEffect = rainGO.GetComponent<ParticleSystem>();
    }

    public void MakeRain(bool isRaining, bool waterTiles = false, bool spawnWaterdrops = false, bool win = true)
    {
        if (isRaining)
        {
            rainEffect.Play();
            StartCoroutine(FlashPlay());
            if (win == true) 
                playgroundManager.SetGreenDecorations();
            if (waterTiles)
                InvokeRepeating("RainingWaterTiles", 0.5f, rainInterval/5f);
            if (spawnWaterdrops)
                InvokeRepeating("RainingSpawnWaterdrop", 0.5f, rainInterval);
        }
        else
        {
            rainEffect.Stop();
            //StartCoroutine(FlashPlay());
            CancelInvoke("RainingWaterTiles");
            CancelInvoke("RainingSpawnWaterdrop");
        }
    }

    void RainingSpawnWaterdrop()
    {
        Vector3Int raindropPos;
        bool isOnPlayground;
        do
        {
            raindropPos = new Vector3Int(Random.Range(0, maxX), Random.Range(0, maxY), 0);
            Vector3 cellCenter = playgroundManager.GetCellCenter(raindropPos);
            isOnPlayground = playgroundManager.IsOnPlayground(cellCenter);
        }while(!isOnPlayground);
        playgroundManager.AddWaterdrop(raindropPos);
    }

    void RainingWaterTiles()
    {
        for (int j = 0; j < maxY; j ++)
            for (int i = 0; i < maxX; i ++)
                if(Random.value < 0.15)
                    playgroundManager.WaterCell(new Vector3Int(i, j));
    }

    IEnumerator FlashPlay()
    {
        Color color = lighthningFlashImage.color;
        lighthningFlashImage.color = new Color(color.r, color.g, color.b, 0.5f);
        yield return new WaitForSeconds(0.2f);
        lighthningFlashImage.color = new Color(color.r, color.g, color.b, 0f);
        yield return new WaitForSeconds(0.2f);
        lighthningFlashImage.color = new Color(color.r, color.g, color.b, 0.2f);
        yield return new WaitForSeconds(0.3f);
        lighthningFlashImage.color = new Color(color.r, color.g, color.b, 0f);
    }
}
