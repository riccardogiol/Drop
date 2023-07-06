using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D player;

    Vector2 movement;
    public Vector3 lastDirection;

    void Start()
    {
        lastDirection = new Vector3(1, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        // handle input here
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        if (movement.magnitude > 0)
        {
            //update direction
            if (movement.x != 0)
            {
                lastDirection = new Vector3(movement.x, 0, 0);
            } else {
                lastDirection = new Vector3(0, movement.y, 0);
            }

            //react with tilemap
            gameObject.GetComponent<PlayerGroundInteraction>().NewPosition();
        }
        
    }

    //it is like the update with the DT of frames considered. It is not subject to change in framerate
    void FixedUpdate()
    {
        //hendle movement here
        player.MovePosition(player.position + movement * moveSpeed * Time.deltaTime);
    }
}
