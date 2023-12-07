using UnityEngine;
using UnityEngine.Tilemaps;
using Pathfinding;

public class PlayerMovementPath : MonoBehaviour
{
    public float moveSpeed = 3.5f;
    public GameObject touchIndicator;
    public Animator animator;

    Rigidbody2D player;
    PlayerDirectionController directionController;

    Vector3 target;
    Path path;
    int currentWaypoint = 0;
    float nextWaypointDistance = 0.1f;
    Seeker seeker;
    Tilemap tilemap;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        tilemap = FindFirstObjectByType<Tilemap>();
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
    
    public void NewTarget(Vector3 newTarget)
    {
        if (MenusManager.isPaused)
            return;
        if (Vector2.Distance(newTarget, (Vector2)transform.position) < 0.7)
        {
            if (path != null)
            {
                InterruptMovement();
            } else {
                directionController.TurnClockwise();
            }
        } else 
        {
            Vector3Int cell = tilemap.WorldToCell(newTarget);
            target = tilemap.GetCellCenterWorld(cell);
            Instantiate(touchIndicator, target, Quaternion.identity);
            UpdatePath();
        }
    }

    public void InterruptMovement()
    {
        path = null;
    }

    public void OnVictorySpot()
    {
        InterruptMovement();
        animator.SetTrigger("Triumph");
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
