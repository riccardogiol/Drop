using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class PlayerMovementTouch : MonoBehaviour
{
    PlayerMovementPath movementPath;
    PlaygroundManager playgroundManager;

    float countdown = 0f;
    float timer = 0.2f;

    void Awake()
    {
        playgroundManager = FindFirstObjectByType<PlaygroundManager>();
        if (!Application.isMobilePlatform)
            enabled = false;
    }

    void Start()
    {
        movementPath = GetComponent<PlayerMovementPath>();
    }

    void Update()
    {
        if (countdown > 0.0f)
        {
            countdown -= Time.deltaTime;
            return;
        }
        if (Input.touchCount > 0)
            {
                if (Input.GetTouch(0).phase != TouchPhase.Began && Input.GetTouch(0).phase != TouchPhase.Ended)
                    return;
                if (TouchOnUI())
                    return;
                Vector3 target = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                if (!playgroundManager.IsObstacleForWalk(target))
                    movementPath.NewTarget(target);
                countdown = timer;
            }
    }

    bool TouchOnUI()
    {
        PointerEventData eventDataCurrentPosition = new(EventSystem.current) {
            position = Input.GetTouch(0).position
        };
        List<RaycastResult> results = new();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        foreach (var item in results)
        {
            if (item.gameObject.layer == 5)
                return true;
        }
        return false;
    }
}
