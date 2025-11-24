using UnityEngine;
using UnityEngine.UI;
using System;

public class LevelEnterTrigger : MonoBehaviour
{
    public int stageCode = 1;
    CircleCollider2D thisCollider;
    public LevelTileManager levelTileManager;
    public Button button;

    string orderedSpotDirectionUp = "";
    string orderedSpotDirectionDown = "";
    string orderedSpotDirectionLeft = "";
    string orderedSpotDirectionRight = "";

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
        FindFirstObjectByType<PlayerMovementKeysMap>().SetSelectedStage(levelTileManager.codeLvl, stageCode);
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

    public void RegisterSpotOrder(GameObject spotBefore, GameObject spotAfter)
    {
        if (spotBefore != null)
        {
            if (spotBefore.transform.position.x < transform.position.x)
                orderedSpotDirectionLeft = "before";
            else
                orderedSpotDirectionRight = "before";
            if (spotBefore.transform.position.y < transform.position.y)
                orderedSpotDirectionDown = "before";
            else
               orderedSpotDirectionUp = "before";
        }
        if (spotAfter != null)
        {
            if (spotAfter.transform.position.x < transform.position.x)
                orderedSpotDirectionLeft = "after";
            else
                orderedSpotDirectionRight = "after";
            if (spotAfter.transform.position.y < transform.position.y)
                orderedSpotDirectionDown = "after";
            else
               orderedSpotDirectionUp = "after";
        }
    }

    public string GetDestinationSpot(Vector3 destination)
    {
        if (Math.Abs(destination.x) > Math.Abs(destination.y))
        {
            if (destination.x > 0 && orderedSpotDirectionRight != "")
                return orderedSpotDirectionRight;
            else if (destination.x < 0 && orderedSpotDirectionLeft != "")
                return orderedSpotDirectionLeft;
        } else
        {
            if (destination.y > 0 && orderedSpotDirectionUp != "")
                return orderedSpotDirectionUp;
            else if (destination.y < 0 && orderedSpotDirectionDown != "")
                return orderedSpotDirectionDown;
        }
        return "";
    }
}
