using UnityEngine;

public class LevelEnterTrigger : MonoBehaviour
{
    CircleCollider2D thisCollider;
    public LevelTileManager levelTileManager;

    void Start()
    {
        thisCollider = GetComponent<CircleCollider2D>();
        thisCollider.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        levelTileManager.StartLevel();
        thisCollider.enabled = false;
    }
}
