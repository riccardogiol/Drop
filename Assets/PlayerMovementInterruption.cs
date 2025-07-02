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
        if (Vector2.Distance(transform.position, lastFramePosition) < 0.01)
        {
            if(isMoving)
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
        } else {
            isMoving = true;
        }
        lastFramePosition = transform.position;
    }
}

/*
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
    bool justRebounced = false;

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
        if (justRebounced)
        {
            //justRebounced = false;
            //lastFramePosition = transform.position;
            //isMoving = false;
            return;
        }
        if (Vector2.Distance(transform.position, lastFramePosition) < 0.01)
        {
            if (isMoving)
            {
                secondsNotMoving += Time.fixedDeltaTime;
                if (secondsNotMoving > secondsNotMovingLimit)
                {
                    playerMovementKeys.InterruptMovement(0.5f);
                    playerMovementPath.InterruptMovement();
                    Vector2 respawnPosition = playgroundManager.GetCellCenter((Vector2)transform.position - playerDirectionController.lastDirection * 0.5f);
                    playerRB.MovePosition(respawnPosition);
                    lastFramePosition = respawnPosition;
                    secondsNotMoving = 0;
                    isMoving = false;
                    Debug.Log("NotMoving rebouncing");
                    justRebounced = true;
                    StartCoroutine(delayedRebouncing());
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

    public void Rebounce(Vector3 finalPosition)
    {
        playerMovementKeys.InterruptMovement(0.5f);
        playerMovementPath.InterruptMovement();
        playerRB.MovePosition(finalPosition);
        lastFramePosition = finalPosition;
        secondsNotMoving = 0;
        isMoving = false;
        Debug.Log("Shield rebouncing");
        justRebounced = true;
        StartCoroutine(delayedRebouncing());
    }

    IEnumerator delayedRebouncing()
    {
        yield return new WaitForSeconds(0.1f);
        justRebounced = false;
    }
}
*/
