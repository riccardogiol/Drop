using UnityEngine;

public class PlayerMovementInput: MonoBehaviour
{
    public float moveSpeed = 3.5f;
    
    Rigidbody2D player;
    Vector2 movement;
    PlayerMovementPath pathMovement;
    PlayerDirectionController directionController;
    //Joystick joystick;

    void Start()
    {
        player = GetComponent<Rigidbody2D>();
        pathMovement = GetComponent<PlayerMovementPath>();
        directionController = GetComponent<PlayerDirectionController>();
        //joystick = FindFirstObjectByType<Joystick>();
    }

    void Update()
    {
        if(!MenusManager.isPaused)
        {
            // handle input here
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
            //if (joystick != null && (joystick.Horizontal != 0 || joystick.Vertical != 0))
            //{
            //    movement.x = joystick.Horizontal;
            //    movement.y = joystick.Vertical;
            //}
            if (movement.magnitude > 0.1)
            {
                if (pathMovement != null)
                    pathMovement.InterruptMovement();
            }
        }
        
    }

    //it is like the update with the DT of frames considered. It is not subject to change in framerate
    void FixedUpdate()
    {
        if (movement.magnitude > 0.7)
        {
            player.MovePosition(player.position + movement * moveSpeed * Time.deltaTime);
            directionController.UpdateDirection(movement);
        }
        /*
        if ( Math.Abs(movement.x) > 0.5)
            player.MovePosition(player.position + (moveSpeed * Time.deltaTime * new Vector2(movement.x, 0)));
        if ( Math.Abs(movement.y) > 0.5)
            player.MovePosition(player.position + (moveSpeed * Time.deltaTime * new Vector2(0, movement.y)));
        */
    }
}
