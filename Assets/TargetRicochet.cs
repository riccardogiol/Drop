using UnityEngine;

public class TargetRicochet : MonoBehaviour
{
    public Transform target;
    public float force = 0.2f;
    public bool rotateSprite = false;

    Rigidbody2D rigidbody2D;

    Vector2 direction;

    void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        direction = (target.position - transform.position).normalized;
    }

    void FixedUpdate()
    {
        direction = (target.position - transform.position).normalized;
        rigidbody2D.AddForce(direction * force);
        if (rotateSprite)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
