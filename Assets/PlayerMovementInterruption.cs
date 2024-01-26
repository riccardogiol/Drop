using UnityEngine;

public class PlayerMovementInterruption : MonoBehaviour
{
    
    PlayerMovementPath playerMovementPath;
    PlayerMovementKeys playerMovementKeys;
    
    Vector3 lastFramePosition;
    float secondsNotMoving;
    float secondsNotMovingLimit = 0.6f;
    bool isMoving;

    void Start()
    {
        playerMovementPath = GetComponent<PlayerMovementPath>();
        playerMovementKeys = GetComponent<PlayerMovementKeys>();
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
