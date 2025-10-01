using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class PlayerMovementKeys: MonoBehaviour
{
    public float moveSpeed = 3.5f;
    
    Rigidbody2D player;
    Vector3 movement;
    PlayerMovementPath pathMovement;
    PlayerMovementInterruption playerMovementInterrupt;
    PlayerDirectionController directionController;
    Tilemap tilemap;
    PlaygroundManager playgroundManager;

    Vector3 target;
    bool hasTarget;
    float nextWaypointDistance = 0.08f;
    bool movementInterrupted;
    bool rotate;

    void Start()
    {
        player = GetComponent<Rigidbody2D>();
        pathMovement = GetComponent<PlayerMovementPath>();
        playerMovementInterrupt = GetComponent<PlayerMovementInterruption>();
        directionController = GetComponent<PlayerDirectionController>();
        tilemap = FindFirstObjectByType<Tilemap>();
        playgroundManager = FindFirstObjectByType<PlaygroundManager>();
        target = transform.position;
        hasTarget = false;
        movementInterrupted = false;
    }

    void Update()
    {
        if (!MenusManager.isPaused && !movementInterrupted)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
            rotate = Input.GetKeyDown(KeyCode.R);
        }
    }

    void FixedUpdate()
    {
        if (hasTarget)
        {
            if (Vector2.Distance(transform.position, target) < nextWaypointDistance)
            {
                hasTarget = false;
                if (movement.magnitude < 0.7)
                    InterruptMovement();
            }
            else
            {
                Vector2 direction = (target - transform.position).normalized;
                Vector2 newPosition = player.position + (moveSpeed * Time.deltaTime * direction);
                if (Vector2.Distance(newPosition, target) < nextWaypointDistance) // what if it goes over? it shouldn't...
                    player.MovePosition(target);
                else
                    player.MovePosition(newPosition);
                directionController.UpdateDirection(direction);
            }
        }

        if (!hasTarget && rotate)
        {
            directionController.TurnClockwise();
            rotate = false;
            return;
        }

        if (!hasTarget && movement.magnitude > 0.7)
        {
            if (Math.Abs(movement.x) >= Math.Abs(movement.y))
                movement.y = 0;
            else
                movement.x = 0;
            Vector3 newTarget = transform.position + movement.normalized;
            if (Vector3.Distance(newTarget, target) > 0.8)
            {
                Vector3Int cell = tilemap.WorldToCell(newTarget);
                target = tilemap.GetCellCenterWorld(cell);
                if (playgroundManager.IsObstacleForWalk(target, transform.position) || playgroundManager.IsPushableWithObstacle(target, movement.normalized)) // add onewaycollider on right side
                {
                    Vector2 direction = (target - transform.position).normalized;
                    directionController.UpdateDirection(direction);
                    return;
                }
                pathMovement.InterruptMovement();
                hasTarget = true;
            }
        }
    }

    public void InterruptMovement(float delay = 0f)
    {
        playerMovementInterrupt.SetIsMoving(false);
        hasTarget = false;
        target = transform.position;
        movement = new Vector3(0, 0);
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
        target = transform.position;
    }
}
