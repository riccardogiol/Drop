using UnityEngine;

public class TargetRicochet : MonoBehaviour
{
    public Transform target;
    public bool rotateSprite = false;
    public Vector3 startingDirection;
    public float speed = 2.0f;

    Rigidbody2D rigidbody2D;

    Vector2 direction;

    void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        rigidbody2D.velocity = startingDirection * speed;
    }

    void FixedUpdate()
    {
        direction = (target.position - transform.position).normalized;
        rigidbody2D.velocity = direction * speed;
        if (rotateSprite)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
