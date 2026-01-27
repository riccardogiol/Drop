using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;

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

    Vector2 dpadDir;
    bool gamepadInput = false;

    bool azertyLayout = false;

    ChallengeNumberActions challengeNumberActions;

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

        if (PlayerPrefs.GetInt("AzertyLayout") == 1)
            azertyLayout = true;
        
        challengeNumberActions = FindFirstObjectByType<ChallengeNumberActions>();
    }

    void Update()
    {
        if (!MenusManager.isPaused && !movementInterrupted)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            if (azertyLayout)
            {
                movement.x = Input.GetAxisRaw("HorizontalAzerty");
                movement.y = Input.GetAxisRaw("VerticalAzerty");
            }
            
            if (Gamepad.current != null)
            {
                gamepadInput = Gamepad.current.rightShoulder.wasPressedThisFrame;
                dpadDir = Gamepad.current.dpad.ReadValue();
                if (dpadDir.magnitude > 0.5)
                    movement = dpadDir;
            }
            rotate = Input.GetKeyDown(KeyCode.R) || gamepadInput;
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
            Cursor.visible = false;
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
                if (challengeNumberActions != null)
                    challengeNumberActions.IncreaseActionCounter();
                if (playgroundManager.IsObstacleForWalk(target, transform.position) || playgroundManager.IsPushableWithObstacle(target, movement.normalized)) // add onewaycollider on right side
                {
                    Vector2 direction = (target - transform.position).normalized;
                    directionController.UpdateDirection(direction);
                    if (playgroundManager.CheckSparklerAndTrigger(target))
                    {
                        directionController.HitAnimation();
                        InterruptMovement(0.5f);
                    }
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
