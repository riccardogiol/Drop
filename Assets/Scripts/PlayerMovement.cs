using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D player;

    Vector2 movement;

    // Update is called once per frame
    void Update()
    {
        // handle input here
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }

    //it is like the update with the DT of frames considered. It is not subject to change in framerate
    void FixedUpdate()
    {
        //hendle movement here
        player.MovePosition(player.position + movement * moveSpeed * Time.deltaTime);
        if (movement.magnitude > 0)
        {
            gameObject.GetComponent<PlayerGroundInteraction>().NewPosition();
        }
    }
}
