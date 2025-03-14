using UnityEngine;

public class LinearMovement : MonoBehaviour
{
    public Vector2 startingPosition;
    public Vector2 finalPosition;
    public float timer = 2;

    public bool disableColliderWhileMoving = false;

    private float elapsedTime;
    private bool isMoving = false;

    public bool scale = false;
    public Vector3 startingScale;
    public Vector3 finalScale;

    BoxCollider2D boxCollider;

    void Awake()
    {
        startingScale = finalScale = transform.localScale;
        if (disableColliderWhileMoving)
            boxCollider = GetComponent<BoxCollider2D>();
    }

    public void MoveTo(Vector3 finalPos, float time)
    {
        this.enabled = true;
        startingPosition = transform.position;
        finalPosition = finalPos;
        timer = time;
        elapsedTime = 0;
        isMoving = true;
        if (boxCollider != null)
            boxCollider.enabled = false;
    }

    public void ReverseMovement(float time)
    {
        MoveTo(startingPosition, time);
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > timer)
            {
                isMoving = false;
            if (boxCollider != null)
                boxCollider.enabled = true;
                transform.position = finalPosition;
                if (scale)
                    transform.localScale = finalScale;
                this.enabled = false;
                return;
            }
            transform.position = Vector3.Lerp(startingPosition, finalPosition, elapsedTime/timer);
            if (scale)
                transform.localScale = Vector3.Lerp(startingScale, finalScale, elapsedTime/timer);
        }

    }
}
