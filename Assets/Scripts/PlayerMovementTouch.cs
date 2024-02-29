using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class PlayerMovementTouch : MonoBehaviour
{
    PlayerMovementPath movementPath;

    void Awake()
    {
        if (!Application.isMobilePlatform)
             enabled = false;
    }

    void Start()
    {
        movementPath = GetComponent<PlayerMovementPath>();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase != TouchPhase.Began && Input.GetTouch(0).phase != TouchPhase.Ended)
                return;
            if (TouchOnUI())
                return;
            Vector3 target = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            movementPath.NewTarget(target);
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
