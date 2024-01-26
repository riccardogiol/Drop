using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovementKeys: MonoBehaviour
{
    public float moveSpeed = 3.5f;
    
    Rigidbody2D player;
    Vector3 movement;
    PlayerMovementPath pathMovement;
    PlayerDirectionController directionController;
    Tilemap tilemap;

    Vector3 target;
    bool hasTarget;
    float nextWaypointDistance = 0.1f;
    bool movementInterrupted;

    void Start()
    {
        player = GetComponent<Rigidbody2D>();
        pathMovement = GetComponent<PlayerMovementPath>();
        directionController = GetComponent<PlayerDirectionController>();
        tilemap = FindFirstObjectByType<Tilemap>();
        target = transform.position;
        hasTarget = false;
        movementInterrupted = false;
    }

    void Update()
    {
        if(!MenusManager.isPaused && !movementInterrupted)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
        }
    }

    void FixedUpdate()
    {
        if (movement.magnitude > 0.7)
        {
            Vector3 newTarget = transform.position + movement.normalized;
            if (Vector3.Distance(newTarget, target) > 0.8)
            {
                Vector3Int cell = tilemap.WorldToCell(newTarget);
                target = tilemap.GetCellCenterWorld(cell);
                pathMovement.InterruptMovement();
                hasTarget = true;
            }
        }
        if (hasTarget)
        {
            if(Vector2.Distance(transform.position, target) < nextWaypointDistance)
                hasTarget = false;

            Vector2 direction = (target - transform.position).normalized;
            player.MovePosition(player.position + (moveSpeed * Time.deltaTime * direction));
            directionController.UpdateDirection(direction);
        }
    }

    public void InterruptMovement(float delay = 0f)
    {
        hasTarget = false;
        target = transform.position;
        movement = new Vector3(0, 0);
        if (delay > 0.1f)
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
}
