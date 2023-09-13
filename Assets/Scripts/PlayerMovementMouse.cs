using UnityEngine;
using UnityEngine.EventSystems;

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
            if (EventSystem.current.IsPointerOverGameObject())
                return;
            Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition); 
            movementPath.NewTarget(target);
        }
    }
}
