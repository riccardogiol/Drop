using System.Collections;
using UnityEngine;

public class PlayerMovementInterruption : MonoBehaviour
{

    PlayerMovementPath playerMovementPath;
    PlayerMovementKeys playerMovementKeys;
    PlayerDirectionController playerDirectionController;
    Rigidbody2D playerRB;
    PlaygroundManager playgroundManager;


    Vector3 lastFramePosition;
    float secondsNotMoving;
    float secondsNotMovingLimit = 0.6f;
    bool isMoving;

    bool noChecking = false;

    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        playgroundManager = FindFirstObjectByType<PlaygroundManager>();
        playerMovementPath = GetComponent<PlayerMovementPath>();
        playerMovementKeys = GetComponent<PlayerMovementKeys>();
        playerDirectionController = GetComponent<PlayerDirectionController>();
        lastFramePosition = transform.position;
        secondsNotMoving = 0f;
        isMoving = false;
    }

    public void SetIsMoving(bool value)
    {
        isMoving = value;
    }

    void FixedUpdate()
    {
        if (noChecking)
            return;
        if (Vector2.Distance(transform.position, lastFramePosition) < 0.01)
            {
                if (isMoving)
                {
                    secondsNotMoving += Time.fixedDeltaTime;
                    if (secondsNotMoving > secondsNotMovingLimit)
                    {
                        playerMovementKeys.InterruptMovement();
                        playerMovementPath.InterruptMovement();
                        Vector2 respawnPosition = playgroundManager.GetCellCenter((Vector2)transform.position - playerDirectionController.lastDirection * 0.5f);
                        playerRB.MovePosition(respawnPosition);
                        lastFramePosition = respawnPosition;
                        secondsNotMoving = 0;
                        isMoving = false;
                        return;
                    }
                }
            }
            else
            {
                isMoving = true;
            }
        lastFramePosition = transform.position;
    }

    public void StopInCenterOfCell()
    {
        playerMovementKeys.InterruptMovement(0.5f);
        playerMovementPath.InterruptMovement();
        Vector2 respawnPosition = playgroundManager.GetCellCenter((Vector2)transform.position);
        playerRB.MovePosition(respawnPosition);
        lastFramePosition = respawnPosition;
        secondsNotMoving = 0;
        isMoving = false;
        noChecking = true;
        StartCoroutine(PauseChecking());
    }

    IEnumerator PauseChecking()
    {
        yield return new WaitForSeconds(0.2f);
        noChecking = false;
    }
}