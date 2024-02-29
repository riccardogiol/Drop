using UnityEngine;
using UnityEngine.UI;

public class LevelEnterTrigger : MonoBehaviour
{
    public int stageCode = 1;
    CircleCollider2D thisCollider;
    public LevelTileManager levelTileManager;
    public Button button;

    void Awake()
    {
        thisCollider = GetComponent<CircleCollider2D>();
        thisCollider.enabled = false;
        if (levelTileManager == null)
        {
            if (transform.parent.parent.GetComponent<LevelTileManager>() != null)
                levelTileManager = transform.parent.parent.GetComponent<LevelTileManager>();
        }
        //button = transform.Find("Canvas").Find("Button").GetComponent<Button>();
    }

    public void MovePlayerAndActivate()
    {
        FindFirstObjectByType<PlayerMovementPath>().NewTarget(transform.position);
        thisCollider.enabled = true;
        FindFirstObjectByType<MapMoveCamera>().Exit();
    }

    public void ActivateButton(bool state)
    {
        button.interactable = state;
    }

    public void ActivateCollider(bool state)
    {
        thisCollider.enabled = state;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        levelTileManager.StartLevel(stageCode);
        thisCollider.enabled = false;
    }
}
