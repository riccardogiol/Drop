using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 3.5f;
    
    Rigidbody2D player;
    Animator playerAnimator;
    Joystick joystick;
    
    Vector2 movement;
    public Vector3 lastDirection;

    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        player = GetComponent<Rigidbody2D>();
        joystick = FindFirstObjectByType<Joystick>();
    }

    void Update()
    {
        if(!MenusManager.isPaused)
        {
            // handle input here
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
            if (joystick != null && (joystick.Horizontal != 0 || joystick.Vertical != 0))
            {
                movement.x = joystick.Horizontal;
                movement.y = joystick.Vertical;
            }
            if (movement.magnitude > 0.1)
            {
                //update direction
                Vector3 newDirection = lastDirection;
                if (movement.x >= 0.3f) {
                    newDirection = new Vector3(1, 0, 0);
                } else if (movement.x <= -0.3f){
                    newDirection = new Vector3(-1, 0, 0);
                } else if (movement.y >= 0.3f){
                    newDirection = new Vector3(0, 1, 0);
                } else if (movement.y <= -0.3f){
                    newDirection = new Vector3(0, -1, 0);
                }
                if (lastDirection != newDirection)
                {
                    playerAnimator.SetFloat("LastHorizontal", newDirection.x);
                    playerAnimator.SetFloat("LastVertical", newDirection.y);
                }
                lastDirection = newDirection;
            }
            playerAnimator.SetFloat("Horizontal", movement.x);
            playerAnimator.SetFloat("Vertical", movement.y);
            playerAnimator.SetFloat("Speed", movement.sqrMagnitude);
        }
        
    }

    //it is like the update with the DT of frames considered. It is not subject to change in framerate
    void FixedUpdate()
    {
        //hendle movement here -->> to change anyway
        //if (movement.magnitude > 0.7)
            //player.MovePosition(player.position + movement * moveSpeed * Time.deltaTime);
        if ( Math.Abs(movement.x) > 0.5)
            player.MovePosition(player.position + (moveSpeed * Time.deltaTime * new Vector2(movement.x, 0)));
        if ( Math.Abs(movement.y) > 0.5)
            player.MovePosition(player.position + (moveSpeed * Time.deltaTime * new Vector2(0, movement.y)));
    }
}
