using UnityEngine;
using UnityEngine.Tilemaps;
using Pathfinding;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class PlayerMovementPath : MonoBehaviour
{
    public Tilemap tilemap;
    public float moveSpeed = 3.5f;

    Rigidbody2D player;
    PlayerDirectionController directionController;

    Vector3 target;
    Path path;
    int currentWaypoint = 0;
    float nextWaypointDistance = 0.3f;
    Seeker seeker;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        player = GetComponent<Rigidbody2D>();
        directionController = GetComponent<PlayerDirectionController>();
    }

    
    void UpdatePath()
    {
        if (seeker.IsDone())
            seeker.StartPath(transform.position, target, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                    return;
                target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
            else
            {
                if (TouchOnUI())
                    return;
                Debug.Log("touch target: " + Input.GetTouch(0).phase.ToString());
                target = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            } 


            if (Vector2.Distance(target, (Vector2)transform.position) < 0.7)
            {
                if (path != null)
                {
                    InterruptMovement();
                } else {
                    directionController.TurnClockwise();
                }
            } else {
            // do something visual on cell and controle that is a walkable cell!??
            Vector3Int cell = tilemap.WorldToCell(target);
            target = tilemap.GetCellCenterWorld(cell);

            UpdatePath();

            }
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
            Debug.Log(item.gameObject.layer);
            if (item.gameObject.layer == 5)
                return true;
        }
        return false;
    }

    public void InterruptMovement()
    {
        path = null;
    }

    void FixedUpdate()
    {
        if (path == null)
            return;
        if (currentWaypoint >= path.vectorPath.Count)
        {
            path = null;
            return;
        }
        
        Vector2 direction = (path.vectorPath[currentWaypoint] - transform.position).normalized;

        player.MovePosition(player.position + (moveSpeed * Time.deltaTime * direction));
        directionController.UpdateDirection(direction);

        float distance = Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
            currentWaypoint++;

    }
}
