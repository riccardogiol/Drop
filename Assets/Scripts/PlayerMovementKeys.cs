using UnityEngine;

public class PlayerMovementKeys: MonoBehaviour
{
    public float moveSpeed = 3.5f;
    
    Rigidbody2D player;
    Vector2 movement;
    PlayerMovementPath pathMovement;
    PlayerDirectionController directionController;

    void Start()
    {
        player = GetComponent<Rigidbody2D>();
        pathMovement = GetComponent<PlayerMovementPath>();
        directionController = GetComponent<PlayerDirectionController>();
    }

    void Update()
    {
        if(!MenusManager.isPaused)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
            if (movement.magnitude > 0.1)
            {
                if (pathMovement != null)
                    pathMovement.InterruptMovement();
            }
        }
        
    }

    void FixedUpdate()
    {
        if (movement.magnitude > 0.7)
        {
            player.MovePosition(player.position + movement * moveSpeed * Time.deltaTime);
            directionController.UpdateDirection(movement);
        }
    }
}
