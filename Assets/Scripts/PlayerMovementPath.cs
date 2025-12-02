using UnityEngine;
using UnityEngine.Tilemaps;
using Pathfinding;
using System.Collections;

public class PlayerMovementPath : MonoBehaviour
{
    public float moveSpeed = 3.5f;
    bool movementInterrupted = false;
    public GameObject touchIndicator;
    public Animator animator;

    Rigidbody2D player;
    PlayerDirectionController directionController;

    Vector3 target;
    Path path;
    int currentWaypoint = 0;
    float nextWaypointDistance = 0.1f;
    Seeker seeker;
    public Tilemap tilemap;

    PlaygroundManager playgroundManager;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        if (tilemap == null)
            tilemap = FindFirstObjectByType<Tilemap>();
        player = GetComponent<Rigidbody2D>();
        directionController = GetComponent<PlayerDirectionController>();
        
        playgroundManager = FindFirstObjectByType<PlaygroundManager>();
        movementInterrupted = false;
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
    
    public void NewTarget(Vector3 newTarget, bool visualTouch = false)
    {
        if (movementInterrupted)
            return;
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
            if (visualTouch)
                Instantiate(touchIndicator, newTarget, Quaternion.identity);
            else
                Instantiate(touchIndicator, target, Quaternion.identity);
            UpdatePath();
        }
    }

    public void InterruptMovement(float delay = 0f)
    {
        path = null;
        GetComponent<PlayerMovementInterruption>().SetIsMoving(false);
        if (delay >= 0.1f)
        {
            movementInterrupted = true;
            StartCoroutine(InterruptInputDelay(delay));
        }
    }
    
    IEnumerator InterruptInputDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        movementInterrupted = false;
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
            InterruptMovement();
            return;
        }
        
        Vector2 direction = (path.vectorPath[currentWaypoint] - transform.position).normalized;

        player.MovePosition(player.position + (moveSpeed * Time.deltaTime * direction));
        directionController.UpdateDirection(direction);

        float distance = Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
            currentWaypoint++;

        if (playgroundManager != null && distance < 1 && currentWaypoint == (path.vectorPath.Count - 1))
            if (playgroundManager.CheckSparklerAndTrigger(target))
            {
                directionController.UpdateDirection(target - transform.position);
                directionController.HitAnimation();
                InterruptMovement(0.5f);
            }

    }
}
