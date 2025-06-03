using UnityEngine;

public class SpawnFlames : MonoBehaviour
{
    PlaygroundManager playgroundManager;
    public float timer = 3.0f;
    float countdown;
    public Vector3[] spawnSpots;
    int currentSpotIndex = 0;

    void Start()
    {
        playgroundManager = FindFirstObjectByType<PlaygroundManager>();
        
        if (PlayerPrefs.GetInt("EasyMode", 0) == 1)
            timer *= 1.3f;
        countdown = timer;
    }

    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0)
        {
            countdown = timer;
            playgroundManager.FlameOnPosition(spawnSpots[currentSpotIndex], 1, true, false);
            currentSpotIndex = (currentSpotIndex + 1)%spawnSpots.Length;
        }
        
    }
}
