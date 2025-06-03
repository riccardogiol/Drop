using UnityEngine;

public class RotationMovement : MonoBehaviour
{

    public float speed = 1.0f;
    
    void FixedUpdate()
    {
        transform.Rotate(0f, 0f, speed * Time.fixedDeltaTime);
    }
}
