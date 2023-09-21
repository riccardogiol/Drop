using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class PlayerMovementMouse : MonoBehaviour
{
    PlayerMovementPath movementPath;

    void Awake()
    {
        if (Application.isMobilePlatform)
            enabled = false;
    }

    void Start()
    {
        movementPath = GetComponent<PlayerMovementPath>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (ClickOnUI())
                return;
            Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition); 
            movementPath.NewTarget(target);
        }
    }
    
    bool ClickOnUI()
    {
        PointerEventData eventDataCurrentPosition = new(EventSystem.current) {
            position = Input.mousePosition
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
