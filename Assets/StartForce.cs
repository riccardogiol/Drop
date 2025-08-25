using UnityEngine;

public class StartForce : MonoBehaviour
{
    public Vector2 force = new Vector2(0, 1);

    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.AddForce(force);
    }

}
